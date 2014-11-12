using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.XPath;
using Haozes.FxClient.CommUtil;

namespace Haozes.FxClient.Core
{
    public class PortInfo
    {
        private string ssic = string.Empty;
        private SipUri hostUri = null;
        private string domain = string.Empty;
        private string credential = string.Empty;
        private string userid = string.Empty;

        public PortInfo()
        {
        }

        public void Load(string[] list)
        {
            this.ssic = list[0].Replace("ssic=", string.Empty);
            TextReader reader = new StringReader(list[1]);
            XPathDocument doc = new XPathDocument(reader);
            XPathNavigator nav = ((IXPathNavigable)doc).CreateNavigator();
            XPathNodeIterator iter = nav.Select("//@uri");
            iter.MoveNext();
            this.hostUri = new SipUri(iter.Current.Value);

            iter = nav.Select("//@user-id");
            iter.MoveNext();
            this.userid = iter.Current.Value.Trim();

            iter = nav.Select("//results/user/credentials/credential[1]");
            iter.MoveNext();
            this.domain = iter.Current.GetAttribute("domain", string.Empty);

            iter = nav.Select("//results/user/credentials/credential[1]");
            iter.MoveNext();
            this.credential = iter.Current.GetAttribute("c", string.Empty);
        }

        public string Ssic 
        { 
            get { return this.ssic; } 
        }

        public SipUri HostUri 
        { 
            get { return this.hostUri; }
        }

        public string UserId
        {
            get { return this.userid; }
        }

        public string Domain
        {
            get { return this.domain; }
        }

        public string Credential 
        {
            get { return this.credential; }
        }
    }
}
