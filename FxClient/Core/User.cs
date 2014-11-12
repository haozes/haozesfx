using System;
using System.Collections.Generic;
using System.Text;
using Haozes.FxClient.CommUtil;
using System.Xml.XPath;
using System.IO;
using System.Xml;

namespace Haozes.FxClient.Core
{
    public class User
    {
        //好友列表更新

        private SipConnection sipConnection = new SipConnection();
        private PortInfo portInfo = new PortInfo();
        private SipSysConfig sysConfig = new SipSysConfig();
        private SipUri uri = null;
        private string nickname = string.Empty;
        private string impresa = string.Empty;
        private long mobileNo = long.MinValue;
        private string userid = string.Empty;
        private string pwd = string.Empty;
        private string hashPwd = string.Empty;
        private string name = string.Empty;
        private int gender = -1;
        private string personalEmail = string.Empty;
        private ContactMgr contactMgr = null;

        public IList<Contact> Contacts
        {
            get { return this.contactMgr.Contacts; }
        }

        public string NickName
        {
            get { return this.nickname; }
        }

        public long MobileNo
        {
            get { return this.mobileNo; }
        }

        public string UserId
        {
            get { return this.userid; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public string Password
        {
            get { return this.pwd; }
        }

        public string HashedPassword
        {
            get { return this.hashPwd; }
        }

        public int Gender
        {
            get { return this.gender; }
        }

        public string PersonalEmail
        {
            get { return this.personalEmail; }
        }

        public User()
        {
        }

        public User(long mobileNo, string password)
        {
            this.mobileNo = mobileNo;
            this.pwd = password;
            this.hashPwd = Security.HashPasswod.DoHashPassword(this.pwd);
            this.contactMgr = new ContactMgr();
        }

        public void CreatePortInfo(string[] list)
        {
            this.portInfo = LoginHelper.LoadPortInfo(list);
            this.uri = this.portInfo.HostUri;
            this.userid = this.portInfo.UserId;
        }

        public void CreateSipSysConfig(string xml)
        {
            this.sysConfig = LoginHelper.LoadSipSysConfig(xml);
        }

        public void CreatePersonalInfo(string xml)
        {
            TextReader reader = new StringReader(xml);
            XPathDocument doc = new XPathDocument(reader);
            XPathNavigator nav = ((IXPathNavigable)doc).CreateNavigator();
            XPathNodeIterator iter = nav.Select("/results/user-info/personal");
            iter.MoveNext();
            this.nickname = iter.Current.GetAttribute("nickname", string.Empty);
            this.impresa = iter.Current.GetAttribute("impresa", string.Empty);
            this.name = iter.Current.GetAttribute("name", string.Empty);
            this.mobileNo = long.Parse(iter.Current.GetAttribute("mobile-no", string.Empty));
            this.gender = int.Parse(iter.Current.GetAttribute("gender", string.Empty));
        }

        public Contact FindContact(SipUri uri)
        {
            return this.contactMgr.Find(uri);
        }

        public PortInfo Port
        {
            get { return this.portInfo; }
        }

        public SipSysConfig SysConfig
        {
            get { return this.sysConfig; }
        }

        public SipUri Uri
        {
            get { return this.uri; }
            set { this.uri = value; }
        }

        public SipConnection Conncetion
        {
            get { return this.sipConnection; }
            set { this.sipConnection = value; }
        }

        public ContactMgr ContactsManager
        {
            get { return this.contactMgr; }
        }
    }
}
