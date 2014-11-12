using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Haozes.FxClient.Core;
using Imps.Client.Data;
using System.Reflection;
using System.ComponentModel;
using System.Security.Cryptography;

namespace Haozes.Robot.Utils
{
    public class CommonUtil
    {
        public static readonly string CMD_REGEX = @"^(\w+?):(.*)";

        public static IList<RobotTask> ToTaskList(SQLiteDataReader reader)
        {
            IList<RobotTask> list = new List<RobotTask>();
            while (reader.Read())
            {
                try
                {
                    int id = int.Parse(reader["TaskID"].ToString());
                    DateTime time = DateTime.Parse(reader["TaskTime"].ToString());
                    DateTime nextTime = DateTime.Parse(reader["NextTaskTime"].ToString());
                    string num = reader["TargetNum"].ToString();
                    string info = reader["TaskInfo"].ToString();
                    string remark = reader["Remark"].ToString();
                    string targetName = reader["TargetName"].ToString();
                    TaskTrigger trigger = (TaskTrigger)Enum.Parse(typeof(TaskTrigger), reader["Trigger"].ToString());
                    int interval = int.Parse(reader["Interval"].ToString());
                    RobotTask task = new RobotTask(id, time, info, num, targetName, remark, trigger, interval, nextTime);
                    list.Add(task);
                }
                catch { }
            }
            return list;
        }

        public static IList<Contact> ToContactList(SQLiteDataReader reader)
        {
            List<Contact> list = new List<Contact>();
            while (reader.Read())
            {
                try
                {
                    Contact c;
                    long sid = long.Parse(reader["Sid"].ToString());
                    string userName = reader["UserName"].ToString();
                    string uri = reader["Uri"].ToString();
                    //开通飞信的好友
                    c = FindContactBySid(sid);
                    //    c = RobotCore.Host.Contacts.FindContactByMsisdnEx(mobileNo);
                    if (c != null)
                        list.Add(c);
                }
                catch { }
            }
            list.Sort(delegate(Contact c1, Contact c2) { return c1.DisplayName.CompareTo(c2.DisplayName); });
            return list;
        }
        public static bool IsValidCMD(ref string msg)
        {
           // msg = Regex.Replace(msg, "<Font.+?>(.+?)<\\/Font>", "$1").Trim();
            msg = ClearHtml(msg);
            bool isValid = Regex.IsMatch(msg, CMD_REGEX, RegexOptions.IgnoreCase);
            return isValid;
        }

        public static bool IsInternalCmd(string msg)
        {
            bool result = false;
            IDictionary<string, string> list = InternalCmdList();
            foreach (KeyValuePair<string,string> item in list)
            {
                if (string.Compare(item.Key, msg, true) == 0)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public static bool IsOpenInternalCmd(string cmdName)
        {
            return string.Compare(InternaCmdName.Ly.ToString(), cmdName, true) == 0;
        }

        public static IDictionary<string, string> InternalCmdList()
        {
            Array arr = Enum.GetValues(typeof(InternaCmdName));
            IDictionary<string, string> list = new Dictionary<string, string>();
            foreach (object obj in arr)
            {
                InternaCmdName cmdName=(InternaCmdName)Enum.Parse(typeof(InternaCmdName),obj.ToString());
                list.Add(cmdName.ToString(),GetEnumDescription(cmdName));
            }
            return list;
        }
        public static Cmd GetCmd(string msg)
        {
            Match m = Regex.Match(msg, CommonUtil.CMD_REGEX, RegexOptions.IgnoreCase);
            Cmd cmd = new Cmd();
            cmd.Name = m.Groups[1].Value;
            cmd.Para = m.Groups[2].Value;
            return cmd;
        }

        public static string ClearHtml(string msg)
        {
             msg = Regex.Replace(msg, "<Font.+?>(.+?)<\\/Font>", "$1").Trim();
            return  msg;
        }
        /// 获取枚举类子项描述信息
        /// </summary>
        /// <param name="enumSubitem">枚举类子项</param>        
        public static string GetEnumDescription(Enum enumSubitem)
        {
            string strValue = enumSubitem.ToString();

            FieldInfo fieldinfo = enumSubitem.GetType().GetField(strValue);
            Object[] objs = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs == null || objs.Length == 0)
            {
                return strValue;
            }
            else
            {
                DescriptionAttribute da = (DescriptionAttribute)objs[0];
                return da.Description;
            }

        }

        public static Contact FindContactBySid(long sid)
        {
            Contact contact = null;
            foreach (Contact c in RobotCore.Host.Contacts)
            {
                if (c.Uri.Sid == sid || c.MobileNo==sid)
                {
                    contact = c;
                    break;
                }
            }
            return contact;
        }

        public static Contact FindContactByUri(SipUri uri)
        {
            Contact contact = null;
            foreach (Contact c in RobotCore.Host.Contacts)
            {
                if (c.Uri.Raw == uri.Raw)
                {
                    contact = c;
                    break;
                }
            }
            return contact;
        }

        public static string AppFolder
        {
            get
            {
                //string app = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                //app = Path.Combine(app, "Fetion");
                //app = Path.Combine(app, fetion.Uri.Sid.ToString());
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        /// <summary>
        /// 进行DES解密。
        /// </summary>
        /// <param name="pToDecrypt">要解密的以Base64</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <returns>已解密的字符串。</returns>
        public static string Decrypt(string pToDecrypt, string sKey)
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

    }
}
