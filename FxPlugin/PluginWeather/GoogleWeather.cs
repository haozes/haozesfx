using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace Weather
{
    public class GoogleWeather : IWeather
    {
        private string GoogleSearch = "http://www.google.com/ig/api?hl=zh-cn&weather=";
        public string Search(string cityName)
        {
            string url = GoogleSearch + System.Web.HttpUtility.UrlEncode(cityName, Encoding.UTF8);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //下面两行,fix:Too many automatic redirections were attempted 的错误
            CookieContainer cc = new CookieContainer();
            request.CookieContainer = cc;
            request.Method = "GET";

            //返回HTML
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream dataStream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(dataStream, Encoding.Default))
                {

                    string htmlResult = reader.ReadToEnd();
                    if (string.IsNullOrEmpty(htmlResult))
                        return "无法获取Google搜索结果！";
                    else
                    {
                        return cityName + ":" + GetPlainTextFromXml(htmlResult);
                    }
                }
            }
        }

        private string GetPlainTextFromXml(string xmlData)
        {
            StringBuilder result = new StringBuilder();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlData);

            var node = doc.SelectSingleNode("//weather/current_conditions/humidity");
            string cur_humidity = ReadXmlAttributeString(node, "data", false);
            node = doc.SelectSingleNode("//weather/current_conditions/wind_condition");
            string cur_wind = ReadXmlAttributeString(node, "data", false);
            result.Append(string.Format("{0},{1}", cur_humidity, cur_wind));

            var nodes = doc.SelectNodes("//weather/forecast_conditions");
            int i = 0;
            foreach (XmlNode n in nodes)
            {
                string data = string.Empty;
                if (i != 0)
                {
                    node = n.SelectSingleNode("day_of_week");
                    data = ReadXmlAttributeString(node, "data", false);
                    result.Append(data + ":");
                }
                node = n.SelectSingleNode("low");
                data = ReadXmlAttributeString(node, "data", false);
                result.Append(data);

                node = n.SelectSingleNode("high");
                data = ReadXmlAttributeString(node, "data", false);
                result.Append("~" + data + "℃ ");

                node = n.SelectSingleNode("condition");
                data = ReadXmlAttributeString(node, "data", false);
                result.Append(data + Environment.NewLine);
                i++;
            }

            return result.ToString();

        }

        public static string ReadXmlAttributeString(XmlNode node, string attrName, bool required)
        {
            XmlAttribute attribute = node.Attributes[attrName];
            if (required)
            {
                if ((attribute == null) || (attribute.Value.Length <= 0))
                {
                    throw new ApplicationException(string.Format("Attribute - {0} is missed!", attrName));
                }
                return attribute.Value;
            }
            if (attribute != null)
            {
                return attribute.Value;
            }
            return string.Empty;
        }

    }
}
