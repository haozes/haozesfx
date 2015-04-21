using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Haozes.FxClient.LoginTemplate;

namespace Haozes.FxClient.CommUtil
{
    public static class HttpHelper
    {
        public static string Post(string url, string data)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            string responseHTML = string.Empty;

            byte[] replybyte = Encoding.UTF8.GetBytes(data);
            request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "IIC2.0/PC "+LoginTemplate.FetionTemplate.ClientVersion;
            request.Method = "POST";
            //post 开始
            request.ContentLength = data.Length;
            using (Stream newStream = request.GetRequestStream())
            {
                newStream.Write(replybyte, 0, replybyte.Length);
            }
            
            //返回HTML
            response = (HttpWebResponse)request.GetResponse();
            using (Stream dataStream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(dataStream, Encoding.GetEncoding("utf-8")))
                {
                    responseHTML = reader.ReadToEnd();
                }
            }
            return responseHTML;
        }

        /// <summary>
        /// 返回包含ssic和http response内容两个元素的数组
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        /// <returns>string[]</returns>
        public static string[] Get(string url, string data)
        {
            string[] list = new string[2];
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            string responseHTML = string.Empty;

            byte[] replybyte = Encoding.UTF8.GetBytes(data);
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            request = (HttpWebRequest)WebRequest.Create(url);

            request.UserAgent = "IIC2.0/PC 4.0.0000";
            request.Method = "GET";

            //返回HTML
            response = (HttpWebResponse)request.GetResponse();
            string cookieStr = response.Headers["Set-Cookie"];
            string ssic = cookieStr.Split(';')[0].Trim();
            list[0] = ssic;
            //CookieCollection cookies = req.Cookies;
            using (Stream dataStream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(dataStream, Encoding.GetEncoding("utf-8")))
                {
                    responseHTML = reader.ReadToEnd();
                }
            }
            list[1] = responseHTML;

            return list;
        }

        public static void GetPicCodeV4(string algorithm, out byte[] picBuffer, out  string chid)
        {
            string url = string.Format("http://nav.fetion.com.cn/nav/GetPicCodeV4.aspx?algorithm={0}", algorithm);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "IIC2.0/PC "+FetionTemplate.ClientVersion;
            request.Method = "GET";

            //返回HTML
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream dataStream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(dataStream, Encoding.GetEncoding("utf-8")))
                {

                    XDocument doc = XDocument.Parse(reader.ReadToEnd());
                    string encryptedCode = doc.Element("results").Element("pic-certificate").Attribute("pic").Value;
                    picBuffer = Convert.FromBase64String(encryptedCode);
                    chid = doc.Element("results").Element("pic-certificate").Attribute("id").Value;
                }
            }
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {   //   Always   accept   
            return true;
        }
    }
}
