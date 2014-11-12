using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using HtmlAgilityPack;

namespace PluginBus
{
    public class HfBus
    {
        private readonly string hostUrl = "http://www.hfbus.cn";
        private readonly string queryLine = "http://www.hfbus.cn/Service/LinesSearch.aspx?sLinename={0}";
        private readonly string queryStation = "http://www.hfbus.cn/Service/StationSearch.aspx?stop_name={0}";
        private readonly string queryStop = "http://www.hfbus.cn/Service/StopsSearch.aspx?sStopName1={0}&sStopName2={1}";
        private CookieCollection cc = new CookieCollection();

        public string Search(string para)
        {
            try
            {
                int line;
                bool isLine = int.TryParse(para, out line);
                GetCookie();
                if (isLine)
                {
                    return QueryLine(para);
                }
                else
                {
                    bool isStops = para.IndexOf(' ') > 0;
                    if (!isStops)
                    {
                        return QueryStation(para.Trim());
                    }
                    else
                    {
                        string[] arr = para.Split(' ');
                        return QuerySrcAndDst(arr[0].Trim(), arr[1].Trim());
                    }
                }
            }
            catch(Exception ex)
            {
                return "查询发生异常:"+ex.Message;
            }
           
        }


        public void GetCookie()
        {
            CookieContainer _cookieContainer = new CookieContainer();
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(hostUrl);
            webRequest.CookieContainer = _cookieContainer;
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            cc = webResponse.Cookies;
        }
        public string QueryLine(string lineNum)
        {
            string reqUrl = string.Format(queryLine, lineNum);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(reqUrl);
            req.Referer = "http://www.hfbus.cn/Index.aspx";
            req.Accept = @"image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-silverlight, application/x-silverlight-2-b1, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, */*";
            req.UserAgent = @"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; Trident/4.0; QQDownload 598; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ;  Embedded Web Browser from: http://bsalsa.com/; .NET CLR 1.1.4322; .NET CLR 2.0.50727; IE7Pro; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
            CookieContainer c = new CookieContainer();
            req.CookieContainer = c;
            req.CookieContainer.Add(this.cc);
            HttpWebResponse webResponse = (HttpWebResponse)req.GetResponse();
            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                StreamReader reader = new StreamReader(webResponse.GetResponseStream(), Encoding.GetEncoding("gb2312"));
                string rspHtml = reader.ReadToEnd();
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(rspHtml);
               string s ="上行:"+doc.GetElementbyId("DataGrid1").SelectNodes("//td[@colspan=5]")[2].InnerText
                   +Environment.NewLine+"下行:"
                   + doc.GetElementbyId("DataGrid1").SelectNodes("//td[@colspan=5]")[3].InnerText;
               return s.Trim();
            }
            else
            {
                return "查询失败!";
            }

        }

        public string QuerySrcAndDst(string src, string dsc)
        {
            string reqUrl = string.Format(queryStop, HttpUtility.UrlEncode(src, Encoding.GetEncoding("gb2312")),HttpUtility.UrlEncode(dsc, Encoding.GetEncoding("gb2312")));
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(reqUrl);
            req.Referer = "http://www.hfbus.cn/Index.aspx";
            req.Accept = @"image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-silverlight, application/x-silverlight-2-b1, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, */*";
            req.UserAgent = @"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; Trident/4.0; QQDownload 598; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ;  Embedded Web Browser from: http://bsalsa.com/; .NET CLR 1.1.4322; .NET CLR 2.0.50727; IE7Pro; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
            CookieContainer c = new CookieContainer();
            req.CookieContainer = c;
            req.CookieContainer.Add(this.cc);
            HttpWebResponse webResponse = (HttpWebResponse)req.GetResponse();
            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                StreamReader reader = new StreamReader(webResponse.GetResponseStream(), Encoding.GetEncoding("gb2312"));
                string rspHtml = reader.ReadToEnd();
                HtmlDocument doc = new HtmlDocument();
                doc.OptionUseIdAttribute = true;
                doc.LoadHtml(rspHtml);
                return doc.GetElementbyId("form1").SelectNodes("//table[@width=600]")[1].InnerText.Trim();
            }
            else
            {
                return "查询失败!";
            }
        }

        public string QueryStation(string station)
        {
            string reqUrl = string.Format(queryStation, HttpUtility.UrlEncode(station, Encoding.GetEncoding("gb2312")));
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(reqUrl);
            req.Referer = "http://www.hfbus.cn/Index.aspx";
            req.Accept = @"image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-silverlight, application/x-silverlight-2-b1, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, */*";
            req.UserAgent = @"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; Trident/4.0; QQDownload 598; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ;  Embedded Web Browser from: http://bsalsa.com/; .NET CLR 1.1.4322; .NET CLR 2.0.50727; IE7Pro; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
            CookieContainer c = new CookieContainer();
            req.CookieContainer = c;
            req.CookieContainer.Add(this.cc);
            HttpWebResponse webResponse = (HttpWebResponse)req.GetResponse();
            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                StreamReader reader = new StreamReader(webResponse.GetResponseStream(), Encoding.GetEncoding("gb2312"));
                string rspHtml = reader.ReadToEnd();
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(rspHtml);
       
                return doc.GetElementbyId("DataGrid1").InnerText.Trim();
            }
            else
            {
                return "查询失败!";
            }
        }

    }
}
