using System;
using System.Collections.Generic;
using System.Text;

namespace Haozes.FxClient.Core
{
    public enum ContactType
    {
        Buddy,
        MobileBuddy,
        BlockedBuddy
    }

    public class Contact
    {
        private SipUri uri = null;
        private string userid = string.Empty;
        private string localName = string.Empty;
        private string nickName = string.Empty;
        private string impresa = string.Empty;
        private long mobileNo = long.MinValue;
        private ContactType contactType;

        public Contact()
        { 
        }

        public Contact(SipUri uri)
            : this(uri, string.Empty, ContactType.Buddy)
        { 
        }

        public Contact(SipUri uri,string userid, string localName, string nickName, ContactType contactType)
        {
            this.uri = uri;
            this.localName = localName;
            this.nickName = nickName;
            this.contactType = contactType;
            this.userid = userid;
        }

        public Contact(SipUri uri,string localName, string nickName, ContactType contactType)
        {
            this.uri = uri;
            this.localName = localName;
            this.nickName = nickName;
            this.contactType = contactType;
            this.userid = string.Empty;
        }

        public Contact(SipUri uri, string localName, ContactType contactType)
            : this(uri, string.Empty, localName, string.Empty, contactType)
        {
        }

        public long MobileNo
        {
            get { return this.mobileNo; }
            set { this.mobileNo = value; }
        }

        public SipUri Uri
        {
            get { return this.uri; }
            set { this.uri = value; }
        }

        public string LocalName
        {
            get { return this.localName; }
            set { this.localName = value; }
        }

        public string NickName
        {
            get { return this.nickName; }
            set { this.nickName = value; }
        }

        public string Impresa
        {
            get { return this.impresa; }
            set { this.impresa = value; }
        }

        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.localName))
                    return this.localName;
                else if (!string.IsNullOrEmpty(this.nickName))
                    return this.nickName;
                else
                    return this.uri.Sid.ToString();
            }
        }

        public ContactType ContactTypeName
        {
            get { return this.contactType; }
        }

        public string UserId
        {
            get { return this.userid; }
            set { this.userid = value; }
        }
    }
}
