using System;
using System.Collections.Generic;
using System.Text;
using Haozes.FxClient.CommUtil;
using System.Xml.XPath;
using System.IO;

namespace Haozes.FxClient.Core
{
    public class SipSysConfig
    {
        private string sipcProxy = string.Empty;
        private string sipcSslProxy = string.Empty;
        private string ssiAppSignIn = string.Empty;
        private string ssiAppSignOut = string.Empty;
        private string portal = string.Empty;
        private string serverVersion = string.Empty;
        #region unusedKey
        //sub-service
        //apply-sub-service
        //crbt-portal
        //email-adapter
        //get-general-info
        //get-pic-code
        //get-svc-order-status
        //get-system-status
        //get-sipUri
        //group-category
        //group-website
        //matching-portal
        #endregion

        public SipSysConfig()
        { 
        }

        public void Load(string data)
        {
            TextReader reader = new StringReader(data);
            XPathDocument doc = new XPathDocument(reader);
            XPathNavigator nav = ((IXPathNavigable)doc).CreateNavigator();
            XPathNodeIterator iter = nav.Select("/config/servers/sipc-proxy");
            iter.MoveNext();
            this.sipcProxy = iter.Current.Value;

            iter = nav.Select("/config/servers/sipc-ssl-proxy");
            iter.MoveNext();
            this.sipcSslProxy = iter.Current.Value;

            //iter = nav.Select("/config/servers/ssi-app-sign-in");
            iter = nav.Select("/config/servers/ssi-app-sign-in-v2");
            iter.MoveNext();
            this.ssiAppSignIn = iter.Current.Value;

            iter = nav.Select("/config/servers/ssi-app-sign-out");
            iter.MoveNext();
            this.ssiAppSignOut = iter.Current.Value;

            iter = nav.Select("/config/servers/portal");
            iter.MoveNext();
            this.portal = iter.Current.Value;

            iter = nav.Select("/config/servers/server-version");
            iter.MoveNext();
            this.serverVersion = iter.Current.Value;
        }

        public string SipcProxy
        {
            get { return this.sipcProxy; }
        }

        public string SipcSslProxy 
        { 
            get { return this.sipcSslProxy; } 
        }

        public string SsiAppSignIn 
        {
            get { return this.ssiAppSignIn; } 
        }

        public string SsiAppSignOut
        {
            get { return this.ssiAppSignOut; }
        }

        public string Portal
        { 
            get { return this.portal; }
        }

        public string ServerVersion
        {
            get { return this.serverVersion; }
        }
    }
}
