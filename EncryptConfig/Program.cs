using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Xml;
using System.IO;

namespace EncryptConfig
{
    class Program
    {
        static void Main(string[] args)
        {
            string password = GetAppSetting("Password");
            UpdateAppSetting("Password", Encrypt(password));
            Console.WriteLine("密码节点已加密!");
            string gmailPassword = GetAppSetting("GmailPassword");
            UpdateAppSetting("GmailPassword", Encrypt(gmailPassword));
            Console.WriteLine("Gmail密码节点已加密!");
            Console.ReadLine();
        }
        public static string Encrypt(string toEncrypt)
        {
            string result = Encrypt(toEncrypt, "haozesfx");
            return string.Concat(result, "encrypt");
        }

        public static void UpdateAppSetting(string key, string value)
        {
            string configPath =Path.Combine(AppDomain.CurrentDomain.BaseDirectory , "HaozesFx.exe.config");
            XmlDocument doc = new XmlDocument();
            doc.Load(configPath);
            XmlNode node = doc.SelectSingleNode(string.Format(@"//add[@key='{0}']",key));
            XmlElement ele = (XmlElement)node;
            ele.SetAttribute("value", value);
            doc.Save(configPath);
        }
        public static string GetAppSetting(string key)
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HaozesFx.exe.config");
            XmlDocument doc = new XmlDocument();
            doc.Load(configPath);
            XmlNode node = doc.SelectSingleNode(string.Format(@"//add[@key='{0}']", key));
            XmlElement ele = (XmlElement)node;
            return ele.GetAttribute("value");
        }
        /// <summary>
        /// DES加密。
        /// </summary>
        public static string Encrypt(string pToEncrypt, string sKey)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }

    }
}
