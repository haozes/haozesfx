using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using Haozes.FxClient.Sip;
using Haozes.FxClient.Core;
using Haozes.FxClient.CommUtil;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

namespace Haozes.FxClient
{
    public class Client
    {
        private User user;
        private LoginMgr loginMgr;
        private ConversationMgr convMgr;
        private bool isAlive = true;

        //获取单个用户的信息
        private Conversation convGetContactInfo;
        //被添加好友会话
        private Conversation convHandleContactRequest;
        private Conversation convAddBuddy;
        private Conversation convDeleteBuddy;

        private Conversation convSetPresence;

        public event EventHandler LoginSucceed;
        public event EventHandler<LoginEventArgs> LoginFailed;

        /// <summary>
        /// 验证码处理函数.必须赋值 
        /// </summary>
        public Func<string, byte[], string> VerifyCodeRequired { get; set; }

        //好友列表完全加载成功
        public event EventHandler Load;
        public event EventHandler<ConversationArgs> MsgReceived;
        public event EventHandler<FxErrArgs> Errored;
        //从其他客户端登陆
        public event EventHandler Deregistered;
        //别人添加自己为好友事件
        public event EventHandler<ConversationArgs> AddBuddyRequest;
        //添加别人为好友事件
        public event EventHandler<ConversationArgs> AddBuddyResult;
        public event EventHandler<ConversationArgs> DeleteBuddyResult;

        public Client()
        {
            ErrManager.Erroring += new EventHandler<FxErrArgs>(this.OnErrored);
        }

        public void Login(string mobile, string pwd)
        {
            this.user = new User(long.Parse(mobile), pwd);
            this.loginMgr = new LoginMgr(this.user);
            this.loginMgr.LoginSucceed += new EventHandler<LoginEventArgs>(this.LoginMgr_LoginSuccess);
            this.loginMgr.LoginFailed += new EventHandler<LoginEventArgs>(this.OnLoginFailed);
            this.loginMgr.VerifyCodeRequired = this.VerifyCodeRequired;
            this.loginMgr.Login();
            this.isAlive = true;
        }

        public void Exit()
        {
            if (this.isAlive == false)
                return;
            this.user.Conncetion.Close();
            ErrManager.Erroring -= new EventHandler<FxErrArgs>(this.OnErrored);
            this.loginMgr.LoginSucceed -= new EventHandler<LoginEventArgs>(this.LoginMgr_LoginSuccess);
            this.loginMgr.LoginFailed -= new EventHandler<LoginEventArgs>(this.OnLoginFailed);
            this.isAlive = false;
        }

        private void LoginMgr_LoginSuccess(object sender, LoginEventArgs e)
        {
            LogUtil.Log.Debug("完全登陆成功,初始化好友列表成功!");

            Thread.Sleep(100);

            this.convMgr = loginMgr.ConversationManager;
            this.convMgr.MsgReceived += new EventHandler<ConversationArgs>(this.MsgReceived);
            this.convMgr.PresenceNotify += new EventHandler<ConversationArgs>(convMgr_PresenceNotify);
            this.convMgr.Deregistered += new EventHandler(this.Deregistered);
            this.convMgr.AddBuddyRequest += new EventHandler<ConversationArgs>(this.AddBuddyRequest);
            //this.convMgr.UpdateBuddyRequest += new EventHandler<ConversationArgs>(this.ConvMgr_UpdateBuddyRequest);
            this.convMgr.SyncUserInfo += new EventHandler<ConversationArgs>(convMgr_SyncUserInfo);

            this.SubPresence();
            if (this.LoginSucceed != null)
            {
                this.LoginSucceed(this, null);
            }
            if (this.Load != null)
            {
                this.Load(this, null);
            }
            this.user.Conncetion.StartKeepLive();
        }


        private void SubPresence()
        {
            SipMessage subPresPkt = PacketFactory.SubPresence();
            this.user.Conncetion.Send(subPresPkt);
        }

        void convMgr_PresenceNotify(object sender, ConversationArgs e)
        {
            this.user.ContactsManager.PresenceNotify(e.Text);
        }

       private void convMgr_SyncUserInfo(object sender, ConversationArgs e)
        {
            XDocument doc = XDocument.Parse(e.Text);

           //忽略了其他消息,如用户等级等
            if (doc.Element("events")
                                  .Element("event")
                                      .Element("user-info")
                                          .Element("contact-list") == null)
                return;

            var buddylist = doc.Element("events")
                                 .Element("event")
                                     .Element("user-info")
                                         .Element("contact-list")
                                             .Element("buddies")
                                                 .Elements("buddy");
            if (buddylist!=null)
            {
                foreach (var buddy in buddylist)
                {
                    var action = buddy.Attribute("action").Value;
                    var userid = buddy.Attribute("user-id").Value;
                    switch (action.ToLower())
                    {
                        case "remove":
                            this.user.ContactsManager.Delete(userid);
                            break;
                        case "add":
                            var localName = buddy.Attribute("local-name").Value;
                            var uri = buddy.Attribute("uri").Value;
                            this.user.ContactsManager.Add(new Contact(new SipUri(uri),localName,ContactType.Buddy));
                            break;
                        case "update":
                            var c = this.user.ContactsManager.GetContact(userid);
                            if (c != null)
                            {
                                this.GetContactInfo(c.Uri,c.UserId);
                            }
                            break;
                        default:
                            break;
                    }

                }
            }
        }

        protected virtual void OnLoginFailed(object sender, LoginEventArgs e)
        {
            if (this.LoginFailed != null)
            {
                this.Log.Error(e.ToString());
                this.LoginFailed(this, e);
            }
        }

        protected virtual void OnErrored(object sender, FxErrArgs e)
        {
            this.Log.Error(e.ToString());
            if (this.Errored != null)
            {
                this.Errored(this, e);
            }
        }

        public bool IsALive
        {
            get { return this.isAlive; }
            set { this.isAlive = value; }
        }

        public log4net.ILog Log
        {
            get { return LogUtil.Log; }
            set { LogUtil.Init(value); }
        }

        public User CurrentUser
        {
            get { return this.user; }
        }

        public void Send(Conversation conv, string content)
        {
            if (this.convMgr.HasConversation(conv))
            {
                conv.SendMsg(content);
            }
            else
            {
                this.SendSMS(conv.From, content);
            }
        }

        public void SendMsg(SipUri uri, string content)
        {
            this.convMgr.SendMsg(uri, content);
        }

        public void SendMsg(SipMessage rcvPacket, string msgContent)
        {
            this.convMgr.SendMsg(rcvPacket, msgContent);
        }

        public void SendSMS(string mobile, string content)
        {
            this.SendSMS(new SipUri(mobile), content, false);
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
            this.convMgr.SendSMS(uri, content, isCatMsg);
        }

        public void SendToSelf(string content)
        {
            this.convMgr.SendSMS(this.CurrentUser.Uri, content, true);
        }

        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="uri">sipuri</param>
        /// <param name="desc">description,为空时显示用户昵称</param>
        public void AddBuddy(SipUri uri, string desc)
        {
            if (this.isAlive == false)
                return;
            SipMessage packet = PacketFactory.AddBuddy(uri, desc);
            this.convAddBuddy = this.convMgr.Create(packet, true);
            this.convAddBuddy.MsgRcv += new EventHandler<ConversationArgs>(this.ConvAddBuddy_MsgRcv);
        }

        private void ConvAddBuddy_MsgRcv(object sender, ConversationArgs e)
        {
            SipMessage sipMsg = e.RawPacket;
            SipResponse rsp = sipMsg as SipResponse;
            string result=string.Empty;
            if (rsp != null)
            {
                this.Log.Debug(string.Format("addbuddy result:{0}", e.RawPacket));
                if (rsp.StatusCode == 200)
                {
                    XDocument doc = XDocument.Parse(e.Text);
                    if (doc.Element("results").Element("contacts") != null)
                    {
                        var buddies = doc.Element("results").Element("contacts").Element("buddies").Elements("buddy");
                        foreach (var buddy in buddies)
                        {
                            string localname = buddy.Attribute("local-name").Value;
                            string userid = buddy.Attribute("user-id").Value;
                            string uri = buddy.Attribute("uri").Value;
                            result=uri;
                        }
                    }
                }
                if (this.AddBuddyResult != null)
                {
                    ConversationArgs resultArg = new ConversationArgs(IMType.AddBuddyResult, rsp);
                    resultArg.Text = result;
                    this.AddBuddyResult(this, resultArg);
                    this.convAddBuddy.MsgRcv -= new EventHandler<ConversationArgs>(this.ConvAddBuddy_MsgRcv);
                }
            }
        }

        public void GetContactInfo(SipUri uri,string userid)
        {
            SipMessage packet = PacketFactory.GetContactsInfo(uri,userid);
            this.convGetContactInfo = this.convMgr.Create(packet, true);
            this.convGetContactInfo.MsgRcv += new EventHandler<ConversationArgs>(this.ConvGetContactInfo_MsgRcv);
        }

        private void ConvGetContactInfo_MsgRcv(object sender, ConversationArgs e)
        {
            SipResponse rsp = e.RawPacket as SipResponse;
            if (rsp.StatusCode != 200)
            {
                this.Log.Info("未能获取该好友详细信息");
                return;
            }

            XDocument doc = XDocument.Parse(e.Text);
            if (doc.Element("results").Elements("contact").Count() > 0)
            {
                foreach (var c in doc.Element("results").Elements("contact"))
                {
                    SipUri uri=new SipUri(c.Attribute("uri").Value);
                    string userid=c.Attribute("user-id").Value;
                    string mobile=c.Attribute("mobile-no")!=null?c.Attribute("mobile-no").Value:string.Empty;
                    string name=c.Attribute("name").Value;
                    string nickname=c.Attribute("nickname").Value;
                    string impresa=c.Attribute("impresa").Value;
                    Contact contact = new Contact(uri, userid, string.Empty, nickname, ContactType.Buddy);
                    if (!string.IsNullOrEmpty(mobile))
                        contact.MobileNo = long.Parse(mobile);
                    contact.Impresa = impresa;
                    this.user.ContactsManager.Add(contact);
                }
                this.convGetContactInfo.MsgRcv -= new EventHandler<ConversationArgs>(this.ConvGetContactInfo_MsgRcv);
            }

        }

        // 同意对方添加自己为好友
        public void AgreeAddBuddy(SipUri uri,string userid)
        {
            SipMessage agreePacket = PacketFactory.HandleContactRequest(userid);
            this.convHandleContactRequest = this.convMgr.Create(agreePacket, true);
            this.convHandleContactRequest.MsgRcv += new EventHandler<ConversationArgs>(this.ConvHandleContactRequest_MsgRcv);
            this.user.ContactsManager.Add(new Contact(uri, string.Empty, ContactType.Buddy));
        }

        private void ConvHandleContactRequest_MsgRcv(object sender, ConversationArgs e)
        {
            XDocument doc = XDocument.Parse(e.Text);
            if (doc.Element("results").Element("contacts") != null)
            {
                var buddies = doc.Element("results").Element("contacts").Element("buddies").Elements("buddy");
                foreach (var buddy in buddies)
                {
                    string localname = buddy.Attribute("local-name").Value;
                    string userid = buddy.Attribute("user-id").Value;
                    Contact c=this.user.ContactsManager.GetContact(userid);
                    if(c!=null)
                    {
                        c.LocalName = localname;
                        this.user.ContactsManager.Update(c);
                    }
                }
                this.convHandleContactRequest.MsgRcv -= new EventHandler<ConversationArgs>(this.ConvHandleContactRequest_MsgRcv);
            }
        }
        
        public void DeleteBuddy(SipUri buddyUri)
        {
            Contact c = this.user.ContactsManager.Find(buddyUri);
            if (c != null && !string.IsNullOrEmpty(c.UserId))
            {
                this.DeleteBuddy(c.UserId);
            }
        }

        public void DeleteBuddy(string userid)
        {
            var deletePacket = PacketFactory.DeleteBuddy(userid);
            this.convDeleteBuddy = this.convMgr.Create(deletePacket, true);
            this.convDeleteBuddy.MsgRcv += new EventHandler<ConversationArgs>(this.ConvDeleteBuddy_MsgRcv);
        }

        private void ConvDeleteBuddy_MsgRcv(object sender, ConversationArgs e)
        {
            SipResponse rsp = e.RawPacket as SipResponse;
            if (rsp != null)
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(rsp.Body);
                XmlNode node = document.SelectSingleNode("results/contacts/buddies/buddy");
                if (node != null)
                {
                    string uri = XmlHelper.ReadXmlAttributeString(node, "uri");
                    if (!string.IsNullOrEmpty(uri))
                    {
                        this.user.ContactsManager.Delete(new SipUri(uri));
                    }
                }
                ConversationArgs resultArg = new ConversationArgs(IMType.DeleteBuddyResult, rsp);
                resultArg.Text = rsp.StatusCode.ToString();
                this.DeleteBuddyResult(this, resultArg);
                this.convDeleteBuddy.MsgRcv -= new EventHandler<ConversationArgs>(this.ConvDeleteBuddy_MsgRcv);
            }
        }

        public void SetPresence(PresenceStatus statu)
        {
            SipMessage sipPacket = PacketFactory.SetPresence(statu);
            this.convSetPresence = this.convMgr.Create(sipPacket, true);
            this.convSetPresence.MsgRcv += new EventHandler<ConversationArgs>(convSetPresence_MsgRcv);
        }

        void convSetPresence_MsgRcv(object sender, ConversationArgs e)
        {
            SipMessage sipPacket = PacketFactory.PGSetPresence();
            this.convMgr.SendRawPacket(sipPacket);
            this.convMgr.Remove(int.Parse(e.RawPacket.CallID.Value));
        }
    }
}
