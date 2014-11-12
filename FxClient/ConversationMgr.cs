using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Haozes.FxClient.Sip;
using System.Net.Sockets;
using System.Diagnostics;
using Haozes.FxClient.Core;
using Haozes.FxClient.CommUtil;
using System.IO;
using System.Xml.XPath;
using System.Xml;
using System.Xml.Linq;

namespace Haozes.FxClient
{
    public class ConversationMgr
    {
        private object syncFlag = new object();
        private Dictionary<int, Conversation> convList = new Dictionary<int, Conversation>();
        private SipConnection connection = null;
        private IList<ChatConnection> chatConnections = new List<ChatConnection>();
        public event EventHandler<ConversationArgs> MsgReceived;
        /// 他处客户端登录 
        public event EventHandler Deregistered;
        /// 有人添加自己为好友
        public event EventHandler<ConversationArgs> AddBuddyRequest;

        public event EventHandler<ConversationArgs> PresenceNotify;

        public event EventHandler<ConversationArgs> SyncUserInfo;

        public ConversationMgr(SipConnection sipconection)
        {
            this.connection = sipconection;
        }

        public Conversation Create(BaseSipConnection conn, SipMessage packet, bool doSend)
        {
            int callID = int.Parse(packet.CallID.Value);
            Conversation conv = null;
            lock (this.syncFlag)
            {
                if (!this.convList.ContainsKey(callID))
                {
                    conv = new Conversation(conn, packet);
                    this.convList.Add(callID, conv);
                }
            }
            if (doSend)
            {
                conn.Send(packet);
            }
            return conv;
        }
        public Conversation Create(SipMessage packet)
        {
            return this.Create(this.connection, packet, false);
        }

        public Conversation Create(SipMessage packet, bool doSend)
        {
            return this.Create(this.connection, packet, true);
        }


        public ChatConnection CreateChatConnection(string remote, string port)
        {
            ChatConnection conn = new ChatConnection();
            try
            {
                conn.Connect(remote, port);
                this.chatConnections.Add(conn);
            }
            catch (SocketException ex)
            {
                LogUtil.Log.Error(ex);
                conn = null;
            }

            return conn;
        }

        public void SendRawPacket(SipMessage packet)
        {
            this.connection.Send(packet);
        }

        public Conversation Find(int callID)
        {
            Conversation c = null;
            lock (this.convList)
            {
                if (this.convList.ContainsKey(callID))
                {
                    c = this.convList[callID];
                }
            }
            return c;
        }

        public Conversation Find(SipMessage packet)
        {
            int callID = int.Parse(packet.CallID.Value);
            return this.Find(callID);

        }

        public bool HasConversation(Conversation conv)
        {
            return this.convList.ContainsKey(conv.CallID);
        }

        public void Remove(int callID)
        {
            if (this.convList.ContainsKey(callID))
            {
                lock (this.syncFlag)
                {
                    this.convList.Remove(callID);
                }
            }
        }

        public void UserLeftConvsersation(int callid, IList<SipUri> uris)
        {
            Conversation conv = this.Find(callid);
            if (conv != null)
            {
                var conn = conv.Connection;
                var tmp = from p in conv.Participants
                        where uris.Contains(p.Uri) == true
                        select p;
                tmp.ToList().ForEach(p => conv.Participants.Remove(p));
                if (conv.Participants.Count < 2)
                {
                    conv.Connection.Close();
                    var con = (from c in this.chatConnections
                               where c.RemoteIP == conn.RemoteIP && c.RemotePort == conn.RemotePort
                               select c).FirstOrDefault();
                    if (con != null)
                    {
                        this.chatConnections.Remove(con);
                    }
                }
            }
        }

        public virtual void RaiseMsgRcv(object sender, ConversationArgs e)
        {
            if (this.MsgReceived != null)
            {
                this.MsgReceived(sender, e);
            }
        }

        public virtual void RaiseDeregistered(object sender, EventArgs e)
        {
            if (this.Deregistered != null)
            {
                this.Deregistered(sender, null);
            }
        }

        public virtual void RaiseAddBuddyApplication(object send, ConversationArgs e)
        {
            if (AddBuddyRequest != null)
                this.AddBuddyRequest(send, e);
        }

        public virtual void RaiseSyncUserInfo(object sender, ConversationArgs e)
        {
            if (SyncUserInfo != null)
                this.SyncUserInfo(this, e);
        }

        public virtual void RaisePresenceNotify(object send, ConversationArgs e)
        {
            if (PresenceNotify != null)
                this.PresenceNotify(this, e);
        }

        public void SendMsg(SipUri uri, string content)
        {
            SipMessage p = PacketFactory.LeaveMsgPacket(uri, content);
            this.connection.Send(p);
        }

        public void SendMsg(SipMessage rcvPacket, string msgContent)
        {
            SipMessage p = PacketFactory.ReplyMsgPacket(rcvPacket, msgContent);
            this.connection.Send(p);
        }

        public void SendSMS(string mobile, string content)
        {
            this.SendSMS(mobile, content, false);
        }

        /// <summary>
        /// Sends the SMS.
        /// </summary>
        /// <param name="mobile">The mobile.</param>
        /// <param name="content">The content.</param>
        /// <param name="isCatMsg">是否是长短信</param>
        public void SendSMS(string mobile, string content, bool isCatMsg)
        {
            this.SendSMS(new SipUri(mobile), content, isCatMsg);
        }

        public void SendSMS(SipUri uri, string content)
        {
            this.SendSMS(uri, content, false);
        }

        /// <summary>
        /// Sends the SMS.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="content">The content.</param>
        /// <param name="isCatMsg">是否是长短信</param>
        public void SendSMS(SipUri uri, string content, bool isCatMsg)
        {
            SipMessage p = PacketFactory.SMSPacket(uri, content, isCatMsg);
            this.connection.Send(p);
        }
    }
}
