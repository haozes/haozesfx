using System;
using System.Collections.Generic;
using System.Text;

namespace Haozes.FxClient.Sip
{
    public class SipRequest : SipMessage
    {
        private string method = string.Empty;
        private string uri = string.Empty;

        public SipRequest()
        {
        }

        public SipRequest(string method, string uri)
        {
            this.method = method;
            this.uri = uri;
        }

        protected override string GetFirstLine()
        {
            return string.Format("{0} {1} "+Protocol.Version, this.method, this.uri);
        }

        public string Method
        {
            get
            {
                return this.method;
            }

            set
            {
                this.method = value;
            }
        }

        public string Uri
        {
            get
            {
                return this.uri;
            }

            set
            {
                this.uri = (value == null) ? string.Empty : value.Trim();
            }
        }
    }
}
