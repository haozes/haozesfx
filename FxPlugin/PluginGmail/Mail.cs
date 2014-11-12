using System;
using System.Collections.Generic;
using System.Text;
using Haozes.RobotIPlugin;
using System.Xml;
using System.Configuration;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Gmail
{
    public class Mail : Iplugin
    {
        private MailRecoder recoder;
        private string gmailUser = string.Empty;
        private string pwd = string.Empty;

        public Mail()
        {
            recoder = new MailRecoder();
            gmailUser = ConfigurationManager.AppSettings["GmailUserName"];
            pwd = ConfigurationManager.AppSettings["GmailPassword"];
            string flag = "encrypt";
            if (pwd.EndsWith(flag))
            {
                pwd = pwd.Substring(0, pwd.Length - flag.Length);
                pwd = Decrypt(pwd, "haozesfx");
            }
        }

        public string Name
        {
            get { return "mail"; }
        }

        public string Description
        {
            get { return "mail:get " + Environment.NewLine + "mail:to target@mail.com;subjext;content"; }
        }

        public IList<string> SupportedCommand()
        {
            return null;
        }

        public bool CanExecute(string cmdContent)
        {
            return true;
        }

        public string Execute(params string[] cmdParas)
        {
            string execResult = string.Empty;
            if (cmdParas.Length < 2)
            {
                execResult = "参数错误:至少一个元素";
            }
            else
            {
                this.Send(cmdParas[0], cmdParas[1], cmdParas[2]);
                execResult = "邮件已发送!";
            }
            return execResult;
        }

        public string Execute(string cmdContent)
        {
            string result = string.Empty;
            string startFlag = "to ";
            if (cmdContent == "get")
            {
                result = CheckFeeds();
                if (string.IsNullOrEmpty(result))
                    result = "没有新邮件";
            }
            else if (cmdContent.StartsWith(startFlag, StringComparison.OrdinalIgnoreCase))
            {
                result = string.Format("正确的命令格式:{0}", Description);
                cmdContent = cmdContent.Substring(startFlag.Length);
                string[] arr = cmdContent.Split(';');
                if (arr.Length > 1)
                {
                    if (Regex.IsMatch(arr[0], @"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b", RegexOptions.IgnoreCase))
                    {
                        string mailContent = string.Empty;
                        for (int i = 2; i < arr.Length; i++)
                        {
                            mailContent += arr[i];
                        }
                        if (arr.Length > 2)
                        {
                            Send(arr[0], arr[1], mailContent);
                        }
                        else
                        {
                            Send(arr[0], arr[1], mailContent);
                        }
                        result = "邮件已发送!";
                    }
                }

            }
            return result;
        }

        private void Send(string to, string subject, string content)
        {
            RC.Gmail.GmailMessage.SendFromGmail(gmailUser, pwd, to, subject, content);
        }

        private string CheckFeeds()
        {
            StringBuilder sb = new StringBuilder();
            string entryAuthorName = string.Empty;
            string entryAuthorEmail = string.Empty;
            string entryTitle = string.Empty;
            string entrySummary = string.Empty;
            DateTime entryIssuedDate = DateTime.MinValue;
            string entryId = string.Empty;
            RC.Gmail.GmailAtomFeed gmailFeed = new RC.Gmail.GmailAtomFeed(gmailUser, pwd);
            gmailFeed.GetFeed();

            // Access the feeds XmlDocument 
            XmlDocument myXml = gmailFeed.FeedXml;
            // Access the raw feed as a string 
            string feedString = gmailFeed.RawFeed;
            // Access the feed through the object 
            string feedTitle = gmailFeed.Title;
            string feedTagline = gmailFeed.Message;
            DateTime feedModified = gmailFeed.Modified;

            for (int i = 0; i < gmailFeed.FeedEntries.Count; i++)
            {
                entryAuthorName = gmailFeed.FeedEntries[i].FromName;
                entryAuthorEmail = gmailFeed.FeedEntries[i].FromEmail;
                entryTitle = gmailFeed.FeedEntries[i].Subject;
                entrySummary = gmailFeed.FeedEntries[i].Summary;
                entryIssuedDate = gmailFeed.FeedEntries[i].Received;
                entryId = gmailFeed.FeedEntries[i].Id;
                if (!recoder.IsExsit(entryId))
                {
                    recoder.Write(entryId);
                    sb.Append(string.Format("{0}:{1};{2} {3}", entryTitle, entrySummary,entryAuthorEmail, "|"));
                }
            }
            return sb.ToString();
        }

        private static string Decrypt(string pToDecrypt, string sKey)
        {
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        public string Remark
        {
            get { return this.gmailUser; }
        }


        public bool IsHostOnly
        {
            get { return true; }
        }


        public bool NeedParseResult
        {
            get { return true; }
        }
    }
}
