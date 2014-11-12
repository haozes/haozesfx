using System;
using System.Collections.Generic;
using System.Text;

namespace Haozes.FxClient.Core
{
    public class SipUri
    {
        private Uri uri = null;

        public Uri Uri
        {
            get { return this.uri; }
        }

        private long num = long.MinValue;

        /// <summary>
        /// 飞信号或手机号
        /// </summary>
        public long Sid
        {
            get { return this.num; }
        }

        private void Init(Uri uri)
        {
            this.uri = uri;
            if (this.uri.Scheme.ToLower() == "sip")
            {
                string[] arr = uri.LocalPath.Split('@');
                this.num = long.Parse(arr[0]);
            }
            else if (uri.Scheme.ToLower() == "tel")
            {
                this.num = long.Parse(uri.LocalPath);
            }
            else
            {
                throw new ArgumentException("不是合法的SipUri格式");
            }
        }

        public SipUri()
        {
        }

        public SipUri(string rawUri)
        {
            bool ok = Uri.TryCreate(rawUri.Trim(), UriKind.Absolute, out this.uri);
            if (ok)
            {
                this.Init(this.uri);
            }
        }

        public SipUri(Uri uri)
        {
            this.Init(uri);
        }

        public string Raw
        {
            get { return this.uri.ToString(); }
        }

        public override string ToString()
        {
            return this.uri.ToString();
        }

        public override int GetHashCode()
        {
            return this.Raw.GetHashCode();
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            SipUri p = (SipUri)obj;
            return this.Raw == p.Raw;
        }
    }
}
