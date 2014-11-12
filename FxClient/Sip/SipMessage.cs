using System;
using System.Collections.Generic;
using System.Text;

namespace Haozes.FxClient.Sip
{
    public abstract class SipMessage
    {
        private string body = string.Empty;
        private byte[] bodyBuffer;
        private IList<SipHeadField> headFielders = new List<SipHeadField>();

        public SipMessage()
        {
        }

        public void AddFielder(SipHeadField fielder)
        {
            this.headFielders.Add(fielder);
        }

        protected abstract string GetFirstLine();

        public virtual byte[] ToBinary()
        {
            string s = this.ToString();
            return Encoding.UTF8.GetBytes(s);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(this.GetFirstLine());
            foreach (var field in this.headFielders)
            {
                if (field.Name != "L" && !string.IsNullOrEmpty(field.Value))
                {
                    builder.AppendLine(field.ToString());
                }
            }
            string body = this.Body;
            if (this.body.Trim().Length > 0)
            {
                SipHeadField lengthField = new SipHeadField(SipHeadFieldName.ContentLength, Encoding.UTF8.GetByteCount(body).ToString());
                builder.AppendLine(lengthField.ToString());
                builder.AppendLine(Environment.NewLine + body);
            }
            else
            {
                builder.AppendLine(Environment.NewLine + Environment.NewLine);
            }
            return builder.ToString();
        }

        public string Body
        {
            get
            {
                if (string.IsNullOrEmpty(this.body))
                {
                    if (this.bodyBuffer == null)
                    {
                        return string.Empty;
                    }
                    this.body = Encoding.UTF8.GetString(this.bodyBuffer);
                }
                return this.body;
            }
            set
            {
                this.body = (value == null) ? string.Empty : value;
            }
        }

        public byte[] BodyBuffer
        {
            get
            {
                return this.bodyBuffer;
            }
            internal set
            {
                this.bodyBuffer = value;
                this.body = null;
            }
        }

        public IList<SipHeadField> HeadFields
        {
            get { return this.headFielders; }
        }

        public SipHeadField this[string name]
        {
            get
            {
                foreach (var item in this.headFielders)
                {
                    if (name == item.Name)
                        return item;
                }
                return null;
            }
        }

        public SipHeadField Authorization
        {
            get { return this["A"]; }
        }

        public SipHeadField CallID
        {
            get { return this["I"]; }
        }

        public SipHeadField Contact
        {
            get { return this["M"]; }
        }

        public SipHeadField ContentEncoding
        {
            get { return this["E"]; }
        }

        public SipHeadField ContentLength
        {
            get { return this["L"]; }
        }

        public SipHeadField ContentType
        {
            get { return this["C"]; }
        }

        public SipHeadField CSeq
        {
            get { return this["Q"]; }
        }

        public SipHeadField Date
        {
            get { return this["D"]; }
        }

        public SipHeadField EndPoints
        {
            get { return this["EP"]; }
        }

        public SipHeadField Event
        {
            get { return this["N"]; }
        }

        public SipHeadField Expires
        {
            get { return this["X"]; }
        }

        public SipHeadField From
        {
            get { return this["F"]; }
        }

        public SipHeadField MessageID
        {
            get { return this["XI"]; }
        }

        public SipHeadField ReferredBy
        {
            get { return this["RB"]; }
        }

        public SipHeadField ReferTo
        {
            get { return this["RT"]; }
        }

        public SipHeadField Require
        {
            get { return this["RQ"]; }
        }

        public SipHeadField RosterManager
        {
            get { return this["RM"]; }
        }

        public SipHeadField Source
        {
            get { return this["SO"]; }
        }

        public SipHeadField Supported
        {
            get { return this["K"]; }
        }

        public SipHeadField To
        {
            get { return this["T"]; }
        }

        public SipHeadField Unsupported
        {
            get { return this["UK"]; }
        }

        public SipHeadField WWWAuthenticate
        {
            get { return this["W"]; }
        }
    }
}
