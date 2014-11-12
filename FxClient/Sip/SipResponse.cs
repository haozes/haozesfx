using System;
using System.Collections.Generic;
using System.Text;

namespace Haozes.FxClient.Sip
{
    public class SipResponse : SipMessage
    {
        private int statusCode;
        private string reasonPhrase = string.Empty;

        public SipResponse()
        {
        }

        public SipResponse(int statusCode, string reasonPhrase)
        {
            this.statusCode = statusCode;
            this.reasonPhrase = reasonPhrase;
        }

        protected override string GetFirstLine()
        {
            return string.Format(Protocol.Version+" {0} {1}", this.statusCode, this.reasonPhrase);
        }

        public int StatusCode
        {
            get { return this.statusCode; }
            set { this.statusCode = value; }
        }

        public string ReasonPhrase
        {
            get { return this.reasonPhrase; }
            set { this.reasonPhrase = value; }
        }
    }
}
