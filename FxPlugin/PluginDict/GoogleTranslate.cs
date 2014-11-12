using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Net;
using System.Text.RegularExpressions;
namespace Dict
{

    public class GoogleTranslate
    {
        public enum Language { zh = 0, en = 1,unknow=2 };
        private const string GoogleTranslateUrl = "http://www.google.com/uds/Gtranslate?callback=google.language.callbacks.id101&context=22&q={0}&langpair={1}%7C{2}&key=notsupplied&v=1.0";
        private const string GoogleLanguageDetectUrl = "http://www.google.com/uds/GlangDetect?callback=google.language.callbacks.id100&context=22&q={0}&key=notsupplied&v=1.0";

        public string Translate(string cmdContent)
        {
            Language lan = DetectLanguage(cmdContent);
            if (lan != Language.unknow)
            {
                Language TranslateToLan;
                if (lan == Language.zh)
                    TranslateToLan = Language.en;
                else
                    TranslateToLan = Language.zh;
                string url = string.Format(GoogleTranslateUrl, System.Web.HttpUtility.UrlEncode(cmdContent, Encoding.GetEncoding("utf-8")), lan.ToString(), TranslateToLan.ToString());
                using (WebClient wc = new WebClient())
                {
                    wc.Encoding = Encoding.GetEncoding("utf-8");
                    string htmlResult = wc.DownloadString(url);
                    if (string.IsNullOrEmpty(htmlResult))
                        return "翻译失败！";
                    else
                    {
                        return string.Format("{0}：{1}", cmdContent, RegexWord(htmlResult));
                    }
                }
            }
            else
            {
                return  "无法找到匹配的翻译语言";
            }
        }
        public Language DetectLanguage(string cmdContent)
        {
            /* http://www.google.com/uds/samples/language/detect.html
             *Google这玩意也太忽悠了，太不准了吧。
             */
            string url = string.Format(GoogleLanguageDetectUrl, System.Web.HttpUtility.UrlEncode(cmdContent, Encoding.GetEncoding("utf-8")));
            string lan = string.Empty;
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.GetEncoding("utf-8");
                string htmlResult = wc.DownloadString(url);
                if (!string.IsNullOrEmpty(htmlResult))
                {
                    lan = RegexLan(htmlResult);
                }
            }
            //检测不到的，俺全当英文了
             return   Enum.IsDefined(typeof(Language), lan) ? (Language)Enum.Parse(typeof(Language), lan, true) : Language.en;
        }
        private string RegexWord(string goolgeCallBackStr)
        {
            string resultString = "";
            try
            {
                resultString = Regex.Match(goolgeCallBackStr, @"translatedText\"":\""(.+?)\""").Groups[1].Value;
                resultString = System.Web.HttpUtility.UrlDecode(resultString);

            }
            catch (ArgumentException ex)
            {
                resultString = "translate faild!";
            }
            return resultString;
        }
        private string RegexLan(string goolgeCallBackStr)
        {
            string resultString = "";
            try
            {
                resultString = Regex.Match(goolgeCallBackStr, @"language\"":\""(.+?)\""").Groups[1].Value;
                resultString = System.Web.HttpUtility.UrlDecode(resultString).ToLower().Trim(); ;

            }
            catch (ArgumentException ex)
            {
                resultString = "detect faild!";
            }
            if (resultString== "uk")
                resultString = "en";
            else if(resultString.StartsWith("zh"))
                resultString="zh";
            return resultString;
        }
    }
}
