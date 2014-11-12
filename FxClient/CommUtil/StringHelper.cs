using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;

namespace Haozes.FxClient.CommUtil
{
    public static class StringHelper
    {
        // Fields
        public const string CrLf = "\r\n";
        public const string Ellipsis = "...";
        private static SortedList<char, char> fullHalfAngleTable;
        private const string HexTable = "0123456789ABCDEF";
        public const string Space = " ";

        // Methods
        static StringHelper()
        {
            string str = "ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ～！＠＃＄％︿＆＊（）＿－＋｜＼｛｝［］：＂；＇＜＞，．？／０１２３４５６７８９";
            string str2 = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ~!@#$%^&*()_-+|\\{}[]:\";'<>,.?/0123456789";
            fullHalfAngleTable = new SortedList<char, char>();
            for (int i = 0; i < str.Length; i++)
            {
                fullHalfAngleTable.Add(str[i], str2[i]);
            }
        }

        public static bool CheckEmailFormat(string email)
        {
            email = email.Trim();
            if (email.Length == 0)
            {
                return true;
            }
            int index = email.IndexOf("@");
            return ((index > 0) && (index < (email.Length - 1))) && (email.IndexOf('@', ++index) < 0);
        }

        public static int ComparsionByCharValue(string str1, string str2)
        {
            if (str1 == null)
            {
                if (str2 != null)
                {
                    return -1;
                }
                return 0;
            }
            if (str2 == null)
            {
                if (str1 != null)
                {
                    return 1;
                }
                return 0;
            }
            if (str1.Length == 0)
            {
                if (str2.Length != 0)
                {
                    return -1;
                }
                return 0;
            }
            for (int i = 0; i < str1.Length; i++)
            {
                if (i >= str2.Length)
                {
                    return 1;
                }
                int num2 = str1[i] - str2[i];
                if (num2 != 0)
                {
                    return num2;
                }
            }
            return str1.Length - str2.Length;
        }

        public static string EncodString(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < source.Length; i++)
            {
                switch (source[i])
                {
                    case '<':
                        {
                            builder.Append("&lt;");
                            continue;
                        }
                    case '>':
                        {
                            builder.Append("&gt;");
                            continue;
                        }
                    case '&':
                        {
                            builder.Append("&amp;");
                            continue;
                        }
                    case '"':
                        {
                            builder.Append("&quot;");
                            continue;
                        }
                }
                builder.Append(source[i]);
            }
            return builder.ToString();
        }

        public static Encoding GetFileEncoding(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (stream.CanRead)
                {
                    byte[] buffer = new byte[4];
                    stream.Read(buffer, 0, 4);
                    if ((buffer[0] == 0xff) && (buffer[1] == 0xfe))
                    {
                        return Encoding.Unicode;
                    }
                    if ((buffer[0] == 0xfe) && (buffer[1] == 0xff))
                    {
                        return Encoding.BigEndianUnicode;
                    }
                    if (((buffer[0] == 0xef) && (buffer[1] == 0xbb)) && (buffer[2] == 0xbf))
                    {
                        return Encoding.UTF8;
                    }
                }
            }
            return Encoding.Default;
        }

        public static string GetFileSize(long bytes)
        {
            return GetFileSize(bytes, true);
        }

        public static string GetFileSize(long bytes, bool needDecimal)
        {
            int num = 0x400;
            if (bytes < num)
            {
                return string.Format("{0} 字节", bytes.ToString());
            }
            if ((bytes >= num) && (bytes < (num * num)))
            {
                return string.Format("{0} KB", Convert.ToString((long)(bytes / ((long)num))));
            }
            if ((bytes >= (num * num)) && (bytes < ((num * num) * num)))
            {
                float num2 = (((float)bytes) / ((float)num)) / ((float)num);
                return string.Format("{0} MB", num2.ToString(needDecimal ? "F1" : "F0"));
            }
            float num3 = ((((float)bytes) / ((float)num)) / ((float)num)) / ((float)num);
            return string.Format("{0} G", num3.ToString(needDecimal ? "F2" : "F0"));
        }

        public static string GetPaddingString(string text, int maxlen, string padding)
        {
            if ((text == null) || (text.Length <= maxlen))
            {
                return text;
            }
            int length = maxlen - padding.Length;
            if (length <= 0)
            {
                return text.Substring(maxlen);
            }
            return text.Substring(0, length) + padding;
        }

        public static string GetPaddingStringEndEllipsis(string text, int maxlen)
        {
            return GetPaddingString(text, maxlen, "...");
        }

        public static string Hex2Str(byte[] bytes)
        {
            return Hex2Str(bytes, true, -1, -1, -1);
        }

        public static byte[] HexToBinary(string hex)
        {
            if ((hex == null) || (hex.Length < 1))
            {
                return new byte[0];
            }
            int num = hex.Length / 2;
            byte[] buffer = new byte[num];
            num *= 2;
            for (int i = 0; i < num; i++)
            {
                int num3 = int.Parse(hex.Substring(i, 2), NumberStyles.HexNumber);
                buffer[i / 2] = (byte)num3;
                i++;
            }
            return buffer;
        }

        public static string Hex2Str(byte[] bytes, bool upperCase, int spacePerBytes, int hyphenPerBytes, int crPerBytes)
        {
            string str = upperCase ? "0123456789ABCDEF" : "0123456789ABCDEF".ToLower();
            int length = bytes.Length;
            StringBuilder builder = new StringBuilder(length * 2);
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            for (int i = 0; i < length; i++)
            {
                builder.Append(str[bytes[i] >> 4]);
                builder.Append(str[bytes[i] & 15]);
                if (i == (length - 1))
                {
                    break;
                }
                bool flag = true;
                if ((crPerBytes > 0) && (++num4 == crPerBytes))
                {
                    builder.Append("\r\n");
                    num4 = 0;
                    flag = false;
                }
                if ((hyphenPerBytes > 0) && (++num3 == hyphenPerBytes))
                {
                    if (flag)
                    {
                        builder.Append('-');
                        flag = false;
                    }
                    num3 = 0;
                }
                if ((spacePerBytes > 0) && (++num2 == spacePerBytes))
                {
                    if (flag)
                    {
                        builder.Append(' ');
                    }
                    num2 = 0;
                }
            }
            return builder.ToString();
        }

        public static bool IsAscii(char ch)
        {
            return ch <= '\x007f';
        }

        public static bool IsAscii(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (!IsAscii(s[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsDigit(string s)
        {
            if (s.Length <= 0)
            {
                return false;
            }
            for (int i = 0; i < s.Length; i++)
            {
                if ((s[i] < '0') || (s[i] > '9'))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsEmail(string mail)
        {
            string pattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            return Regex.Match(mail, pattern, RegexOptions.IgnoreCase).Success;
        }

        public static string ReplaceCrLfToSpace(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace("\r\n", " ");
                text = text.Replace("\r\n"[1], " "[0]);
            }
            return text;
        }

        public static DateTime Str2DateTime(string s)
        {
            return Convert.ToDateTime(s);
        }

        public static byte[] Str2Hex(string input)
        {
            return Encoding.UTF8.GetBytes(input);
        }

        public static char ToHalfAngle(char ch)
        {
            char ch2;
            if (ch <= Convert.ToChar((byte)0xff))
            {
                return ch;
            }
            if (!fullHalfAngleTable.TryGetValue(ch, out ch2))
            {
                return ch;
            }
            return ch2;
        }

        public static string ToHalfAngle(string text, CharConvertDelegate converter)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            char[] chArray = new char[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                chArray[i] = ToHalfAngle(text[i]);
                if (converter != null)
                {
                    chArray[i] = converter(chArray[i]);
                }
            }
            return new string(chArray);
        }

        /// <summary> 
        /// 字节数组转16进制字符串 
        /// </summary> 
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /**
* 把一个16进制字符串转换为字节数组，字符串没有空格，所以每两个字符
* 一个字节
* 
* @param s
* @return
*/

        public static byte[] HexStringToByteArray(String hexString)
        {
            int NumberChars = hexString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return bytes;
        }


        // Nested Types
        public delegate char CharConvertDelegate(char ch);

        //CS address="221.176.31.10:8080;221.176.31.10:443",credential="2101977755.1708399442"
        public static IList<KeyValuePair<string, string>> GetKeyValuePairs(string str)
        {
            IList<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();

            string[] arr = str.Split(',');
            foreach (var item in arr)
            {
                if (item.IndexOf('=') > 0)
                {
                    string key = item.Split('=')[0].Trim().Replace("\"", "");
                    string value = item.Split('=')[1].Trim().Replace("\"", "");
                    list.Add(new KeyValuePair<string, string>(key, value));
                }
            }
            return list;
        }

        public static string GetKeyFromPairs(string key, IList<KeyValuePair<string, string>> list)
        {
            var re = (from c in list
                      where c.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase)
                      select c.Value).FirstOrDefault();

            return re;
        }
    }


}
