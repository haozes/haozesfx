using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using Haozes.FxClient.Sip;
using Haozes.FxClient.Core;
using Haozes.FxClient.CommUtil;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace Haozes.FxClient
{
    public class MessageParser
    {
        private ConversationMgr convMgr;

        public MessageParser(ConversationMgr convMgr)
        {
            this.convMgr = convMgr;
        }

        private IList<Contact> GetChatParticipants(string pkBody)
        {
            IList<Contact> list = new List<Contact>();
            Regex regexObj = new Regex(@"a=user:(sip:.+?;p=\d+)", RegexOptions.Singleline | RegexOptions.Multiline);
            Match matchResults = regexObj.Match(pkBody);
            while (matchResults.Success)
            {
                for (int i = 1; i < matchResults.Groups.Count; i++)
                {
                    Group groupObj = matchResults.Groups[i];
                    if (groupObj.Success)
                    {
                        SipUri sipUri = new SipUri(groupObj.Value);
                        Contact c = new Contact(sipUri);
                        list.Add(c);
                    }
                }
                matchResults = matchResults.NextMatch();
            }
            return list;
        }

        private void StartChat(SipMessage packet)
        {
            string a = packet.Authorization.Value.Trim();
            if (a.IndexOf("CS address", StringComparison.CurrentCultureIgnoreCase) < 0)
                return;

            var kvList = StringHelper.GetKeyValuePairs(a);
            string[] chatSvrs = StringHelper.GetKeyFromPairs("CS address", kvList).Split(';');
            string ticksAuth = StringHelper.GetKeyFromPairs("credential", kvList);

            IList<Contact> participants = GetChatParticipants(packet.Body);
            int callID = int.Parse(packet.CallID.Value);

            this.convMgr.Remove(callID);
            string remote = chatSvrs[0].Split(':')[0];
            string port = chatSvrs[0].Split(':')[1];
            ChatConnection conn = this.convMgr.CreateChatConnection(remote, port);

            if (conn != null)
            {
                conn.MessageReceived += new EventHandler<ConversationArgs>(ReceiveSipMessage);
                conn.StartListen();

                var sipdata = PacketFactory.RegToChatServer(callID.ToString(), ticksAuth);
                Conversation newConv = this.convMgr.Create(conn, sipdata, true);
                participants.ToList().ForEach(c => newConv.Participants.Add(c));
                if (newConv != null)
                {
                    newConv.MsgRcv += new EventHandler<ConversationArgs>(this.convMgr.RaiseMsgRcv);
                }
            }
        }

        public void ReceiveSipMessage(object sender, ConversationArgs e)
        {
            SipMessage packet = e.RawPacket;
            DebugWriter.WriteRCVPacket(packet);
            SipRequest req = packet as SipRequest;

            Conversation conv = this.convMgr.Find(packet);
            if (conv != null)
            {
                conv.RcvPacket(packet);
            }

            if (req != null)
            {
                string sipMethod = req.Method;
                int callID = int.Parse(req.CallID.Value);
                switch (sipMethod)
                {
                    case SipMethodName.Invite:
                        this.StartChat(req);
                        break;
                    case SipMethodName.Message:
                        //haozes 11/25 手机客户端发来的ContentType为空
                        if (req.ContentType == null || string.Compare(req.ContentType.Value, "text/plain") == 0 || string.Compare(req.ContentType.Value, "text/html-fragment") == 0)
                        { //短信 
                            if (this.convMgr.Find(req) == null)
                            {
                                Conversation newSMSConv = this.convMgr.Create(req);
                                if (newSMSConv != null)
                                {
                                    newSMSConv.MsgRcv += new EventHandler<ConversationArgs>(this.convMgr.RaiseMsgRcv);
                                    newSMSConv.RcvPacket(req);
                                }
                            }
                        }
                        break;
                    case SipMethodName.Bye:
                        this.convMgr.Remove(callID);
                        break;
                    case SipMethodName.Benotify:
                        if (string.Equals(req.Event.Value, SipEvent.PresenceV4.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            this.convMgr.RaisePresenceNotify(this, e);
                        }
                        else if (string.Equals(req.Event.Value, SipEvent.Registration.ToString(), StringComparison.OrdinalIgnoreCase))
                        { //registration
                            if (req.Body.IndexOf("deregistered") > 0)
                            {
                                this.convMgr.RaiseDeregistered(this, null);
                            }
                        }
                        else if (string.Equals(req.Event.Value, SipEvent.SyncUserInfoV4.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            this.convMgr.RaiseSyncUserInfo(this, e);
                        }

                        else if (string.Equals(req.Event.Value, SipEvent.Contact.ToString(), StringComparison.OrdinalIgnoreCase))
                        { //contact 

                            string eventType = this.GetEventType(e.Text);
                            if (string.Equals(eventType, "AddBuddyApplication", StringComparison.OrdinalIgnoreCase))
                            {
                                convMgr.RaiseAddBuddyApplication(this, new ConversationArgs(IMType.AddBuddyRequest, req));
                            }
                        }
                        else if (string.Equals(req.Event.Value, SipEvent.Conversation.ToString(), StringComparison.CurrentCultureIgnoreCase))
                        {
                            string eventType = GetEventType(e.Text);
                            if (string.Equals(eventType, "UserLeft", StringComparison.CurrentCultureIgnoreCase))
                            {
                                LogUtil.Log.Debug("UserLeft raised!");
                                this.convMgr.UserLeftConvsersation(callID, GetMember(e.Text));
                            }
                        }
                        break;
                    default:
                        break;
                } //switch end
            }
        }

        private string GetEventType(string pkContent)
        {
            string re = string.Empty;
            XmlDocument document = new XmlDocument();
            document.LoadXml(pkContent);
            XmlNode node = document.SelectSingleNode("events/event");
            if (node != null)
            {
                re = XmlHelper.ReadXmlAttributeString(node, "type");
            }
            return re;
        }

        private IList<SipUri> GetMember(string pkContent)
        {
            IList<SipUri> list = new List<SipUri>();
            XDocument doc = XDocument.Parse(pkContent);
            var members = doc.Element("events")
                .Element("event")
                .Elements("member");

            members.ToList().ForEach(n => list.Add(new SipUri(n.Attribute("uri").Value)));
            return list;
        }
    }
}
