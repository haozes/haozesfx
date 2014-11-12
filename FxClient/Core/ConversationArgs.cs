using System;
using System.Collections.Generic;
using System.Text;
using Haozes.FxClient.Sip;

namespace Haozes.FxClient.Core
{
    public class ConversationArgs : EventArgs
    {
        private IMType imType;
        private string text;
        private SipMessage sipMessage;
        private SipMessage originMessage;

        public ConversationArgs(SipMessage sipMessage)
            : this(IMType.Internal, sipMessage, null)
        { 
        }

        public ConversationArgs(IMType imType, SipMessage sipMessage)
            : this(imType, sipMessage, null)
        {
        }

        public ConversationArgs(IMType imType, SipMessage sipMessage, SipMessage originMessage)
        {
            this.imType = imType;
            this.sipMessage = sipMessage;
            this.text = sipMessage.Body;
            this.originMessage = originMessage;
        }

        public IMType MsgType
        {
            get { return this.imType; }
        }

        /// <summary>
        /// 会话结果,默认为SipMessage Body部分
        /// </summary>
        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }

        public SipMessage RawPacket
        {
            get { return this.sipMessage; }
        }

        public SipMessage OriginPacket
        {
            get { return this.originMessage; }
        }
    }
}
