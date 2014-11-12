using System;
using System.Collections.Generic;
using System.Text;
using Haozes.FxClient.Sip;
using System.Text.RegularExpressions;
using System.Net.Sockets;

namespace Haozes.FxClient.Core
{
    public class Conversation
    {
        private BaseSipConnection connection;
        private SipUri from = null;
        private IList<Contact> participants = new List<Contact>();

        public event EventHandler<ConversationArgs> MsgRcv;

        public BaseSipConnection Connection
        {
            get { return connection; }
        }

        public SipUri From
        {
            get { return this.from; }
            set { this.from = value; }
        }

        //public string To { get; set; }
        private int callID = 0;
        public int CallID
        {
            get { return this.callID; }
            set { this.callID = value; }
        }

        private string cseq = string.Empty;
        public string CSeq
        {
            get { return this.cseq; }
            set { this.cseq = value; }
        }

        private SipEvent eventName = SipEvent.None;
        public SipEvent EventName
        {
            get { return this.eventName; }
            set { this.eventName = value; }
        }

        private SipMessage originPacket = null;
        public SipMessage OriginPacket
        {
            get { return this.originPacket; }
            set { this.originPacket = value; }
        }

        public IList<Contact> Participants
        {
            get { return this.participants; }
        }

        public Conversation()
        {
        }

        public Conversation(BaseSipConnection connection, SipMessage packet)
        {
            this.connection = connection;
            this.callID = int.Parse(packet.CallID.Value);
            this.cseq = packet.CSeq.Value;
            this.from = new SipUri(packet.From.Value);
            string eventStr = (packet.Event == null ? SipEvent.None.ToString() : packet.Event.Value);
            if (Enum.IsDefined(typeof(SipEvent), eventStr))
            {
                this.eventName = (SipEvent)Enum.Parse(typeof(SipEvent), eventStr, true);
            }
            this.originPacket = packet;
        }

        protected virtual void RcvRequest(SipRequest packet)
        {
            string sipMethod = packet.Method;
            EventHandler<ConversationArgs> msgHandler = this.MsgRcv;
            switch (sipMethod)
            {
                case SipMethodName.Invite:
                    this.ResponseInvite(packet);
                    break;
                case SipMethodName.Bye:
                    this.ResponseBye(packet);
                    break;
                case SipMethodName.Message:
                    this.ResponseReceiveMsg(packet);
                    if (msgHandler != null)
                    {
                        IMType imType = IMType.Internal;
                        //haozes edit 11/15
                        if (packet.ContentType == null || packet.ContentType.Value == "text/plain")
                        {
                            imType = IMType.SMS;//短信
                        }
                        else
                        {
                            imType = IMType.Text;//会话
                        }
                        msgHandler(this, new ConversationArgs(imType, packet));
                    }
                    break;
                default:
                    if (msgHandler != null)
                    {
                        if (this.eventName == SipEvent.AddBuddy)
                        {//也许有其他需要originPacket的地方
                            msgHandler(this, new ConversationArgs(IMType.Internal, packet, this.originPacket));
                        }
                        else
                        {
                            msgHandler(this, new ConversationArgs(IMType.Internal, packet));
                        }
                    }
                    break;
            }
        }

        protected virtual void RcvResponse(SipResponse packet)
        {
            EventHandler<ConversationArgs> msgHandler = this.MsgRcv;
            msgHandler(this, new ConversationArgs(IMType.Internal, packet));
        }

        public void RcvPacket(SipMessage packet)
        {
            SipRequest req = packet as SipRequest;
            if (req != null)
            {
                if (req[SipHeadFieldName.From] != null)
                {
                    this.from = new SipUri(req[SipHeadFieldName.From].Value);
                }
                this.RcvRequest(req);
            }
            else
            {
                this.RcvResponse(packet as SipResponse);
            }
        }

        //离线
        public void SendMsg(string content, bool isLeaveMsg)
        {
            SipMessage p = PacketFactory.LeaveMsgPacket(this.from, content);
            this.connection.Send(p);
        }

        //回复
        public void SendMsg(string msgContent)
        {
            SipMessage p = PacketFactory.ReplyMsgPacket(this, msgContent);
            this.connection.Send(p);
        }

        //短信
        public void SendSMS(string mobile, string content)
        {
        }

        //飞信短信
        public void SendSMS(string content)
        {
            SipMessage p = PacketFactory.SMSPacket(this.from, content);
            this.connection.Send(p);
        }

        /// <summary>
        /// 对方发送会话邀请 发送应答信号
        /// </summary>
        /// <param name="packet"></param>
        private void ResponseInvite(SipMessage packet)
        {
            SipMessage p = PacketFactory.RSPInvitePacket(packet);
            this.connection.Send(p);
        }

        /// <summary>
        /// 收到信息 发送收到消息成功的信号
        /// </summary>
        /// <param name="packet"></param>
        private void ResponseReceiveMsg(SipMessage packet)
        {
            SipMessage p = PacketFactory.RSPReceiveMsgPacket(packet);
            this.connection.Send(p);
        }

        /// <summary>
        /// 回复结束会话请求
        /// </summary>
        /// <param name="packet"></param>
        private void ResponseBye(SipMessage packet)
        {
            SipMessage p = PacketFactory.RSPBye(packet);
            this.connection.Send(p);
        }
    }
}
