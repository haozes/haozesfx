using System;
using System.Collections.Generic;
using System.Text;
using Haozes.FxClient.Sip;

namespace Haozes.FxClient.LoginTemplate
{
    public class FetionTemplate
    {
        public readonly string NavServerUrl = "http://nav.fetion.com.cn/nav/getsystemconfig.aspx";
        public static readonly string ClientVersion="4.0.3390";

        public static string Tc()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("GET /nav/tc.aspx HTTP/1.1");
            sb.AppendLine("Host: nav.fetion.com.cn");
            sb.AppendLine("Connection: Close");
            sb.AppendLine("\r\n");
            return sb.ToString();
        }

        public static string GetSystemConfig(string mobile)
        {
            string xml = GetSystemConfigPostData(mobile);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("POST /nav/getsystemconfig.aspx HTTP/1.1");
            sb.AppendLine("User-Agent: IIC2.0/PC "+ClientVersion);
            sb.AppendLine("Host: nav.fetion.com.cn");
            sb.AppendLine("Content-Length: " + xml.Length + "");
            sb.AppendLine("Connection: Close");
            sb.AppendLine(string.Empty);
            sb.AppendLine(xml);
            return sb.ToString();
        }

        public static string GetSystemConfigPostData(string mobile)
        {
            string xml = "<config><user mobile-no=\"" + mobile + "\" /><client type=\"PC\" version=\""+LoginTemplate.FetionTemplate.ClientVersion+"\" platform=\"W5.1\" /><servers version=\"0\" /><service-no version=\"0\" /><parameters version=\"0\" /><hints version=\"0\" /><http-applications version=\"0\" /><client-config version=\"0\" /></config>";
            return xml;
        }

        public static string RegesterSIPCStep1(string sid)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("R fetion.com.cn " + Protocol.Version);
            sb.AppendLine("F: " + sid + "");
            sb.AppendLine("I: 1");
            sb.AppendLine("Q: 1 R");
            sb.AppendLine("CN: d62aa14003cb0de2f252afa755df43cf");
            sb.AppendLine("CL: type=\"pc\",version=\"4.0.3390\"");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        public static string RegesterSIPCStep2(string mobile,string userid, string sid, string response)
        {
            string arg = "<args><device accept-language=\"default\" machine-code=\"1F2E883F250398DEE59C33DD607A6B4C\" /><caps value=\"3FF\" /><events value=\"7F\" /><user-info mobile-no=\""+mobile+"\" user-id=\""+userid+"\"><personal version=\"0\" attributes=\"v4default;alv2-version;alv2-warn\" /><custom-config version=\"0\" /><contact-list version=\"0\" buddy-attributes=\"v4default\" /></user-info><credentials domains=\"fetion.com.cn;m161.com.cn;www.ikuwa.cn;games.fetion.com.cn;turn.fetion.com.cn\" /><presence><basic value=\"400\" desc=\"\" /><extendeds /></presence></args>";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("R fetion.com.cn " + Protocol.Version);
            sb.AppendLine("F: " + sid + "");
            sb.AppendLine("I: 1");
            sb.AppendLine("Q: 2 R");
            sb.AppendLine(string.Format("A: Digest algorithm=\"SHA1-sess-v4\",response=\"{0}\"", response));
            sb.AppendLine("L: " + arg.Length + "");
            sb.AppendLine(string.Empty);
            sb.AppendLine(arg);

            return sb.ToString();
        }
        /******************************************************************************/
    }
}
