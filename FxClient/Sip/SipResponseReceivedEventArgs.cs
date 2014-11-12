using System;
using System.Collections.Generic;
using System.Text;

namespace Haozes.FxClient.Sip
{
    public class SipResponseReceivedEventArgs : EventArgs
    {
        private SipResponse response;

        public SipResponse Response
        {
            get { return this.response; }
            set { this.response = value; }
        }

        public SipResponseReceivedEventArgs()
        {
        }

        public SipResponseReceivedEventArgs(SipResponse response)
        {
            this.response = response;
        }
    }
}
