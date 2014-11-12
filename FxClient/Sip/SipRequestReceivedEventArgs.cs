using System;
using System.Collections.Generic;
using System.Text;

namespace Haozes.FxClient.Sip
{
    public class SipRequestReceivedEventArgs : EventArgs
    {
        private SipRequest request;

        public SipRequest Request
        {
            get { return this.request; }
            set { this.request = value; }
        }

        public SipRequestReceivedEventArgs()
        {
        }

        public SipRequestReceivedEventArgs(SipRequest request)
        {
            this.request = request;
        }
    }
}
