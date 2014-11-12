using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Security.Cryptography;
using System.IO;
using Haozes.FxClient.CommUtil;

namespace Haozes.FxClient.Security
{
    public static class SecurityHelper
    {
        // Fields
        private static string _secToken = WindowsIdentity.GetCurrent().User.Value;
        public static readonly byte[] RgbIV = new byte[] { 0x1f, 0x4e, 0x81, 12, 0xa8, 0x20, 0xfe, 0x42 };

        // Methods
        public static string DecryptPassword(string text)
        {
            if (text.Length <= 0)
            {
                return string.Empty;
            }
            return InnerDecrypt(Convert.FromBase64String(text), _secToken);
        }

        public static string EncryptPass(string sid, string domain, string oldPass, string newPass)
        {
            string s = sid + ":" + domain + ":" + oldPass;
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            byte[] buffer2 = new MD5CryptoServiceProvider().ComputeHash(bytes);
            string str2 = StringHelper.Hex2Str(buffer2) + ":" + newPass;
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
            provider.Key = buffer2;
            MemoryStream stream = new MemoryStream();
            using (CryptoStream stream2 = new CryptoStream(stream, new TripleDESCryptoServiceProvider().CreateEncryptor(buffer2, RgbIV), CryptoStreamMode.Write))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(str2);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                stream2.Close();
            }
            byte[] buffer4 = stream.ToArray();
            stream.Close();
            return StringHelper.Hex2Str(buffer4);
        }

        public static string EncryptPassword(string plain)
        {
            if (plain.Length <= 0)
            {
                return string.Empty;
            }
            return Convert.ToBase64String(InnerEncrypt(plain, _secToken));
        }

        public static byte[] GetSha1(byte[] a)
        {
            using (SHA1 sha = SHA1.Create())
            {
                return sha.ComputeHash(a);
            }
        }

        public static String EncryptV4(String plainpwd)
        {    
            return doHash(Encoding.UTF8.GetBytes("fetion.com.cn:"), Encoding.UTF8.GetBytes(plainpwd));
        }

        public static String EncryptV4(int userid, String plainpwd)
        {
            String passHex = EncryptV4(plainpwd);
            return doHash(BitConverter.GetBytes(userid), StringHelper.HexStringToByteArray(passHex));
        }

        private static String doHash(byte[] b1, byte[] b2)
        {
            byte[] dst = new byte[b1.Length + b2.Length];
            System.Array.Copy(b1, 0, dst, 0, b1.Length);
            System.Array.Copy(b2, 0, dst, b1.Length, b2.Length);
            byte[] res = GetSha1(dst);

            string tmp = StringHelper.Hex2Str(res);
            return StringHelper.byteToHexStr(res);
        }

        private static string InnerDecrypt(byte[] buffer, string keyToken)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(keyToken);
            byte[] rgbKey = new MD5CryptoServiceProvider().ComputeHash(bytes);
            MemoryStream stream = new MemoryStream(buffer, false);
            using (CryptoStream stream2 = new CryptoStream(stream, new TripleDESCryptoServiceProvider().CreateDecryptor(rgbKey, RgbIV), CryptoStreamMode.Read))
            {
                StreamReader reader = new StreamReader(stream2);
                return reader.ReadToEnd();
            }
        }

        private static byte[] InnerEncrypt(string text, string keyToken)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(keyToken);
            byte[] rgbKey = new MD5CryptoServiceProvider().ComputeHash(bytes);
            MemoryStream stream = new MemoryStream();
            using (CryptoStream stream2 = new CryptoStream(stream, new TripleDESCryptoServiceProvider().CreateEncryptor(rgbKey, RgbIV), CryptoStreamMode.Write))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                stream2.Close();
            }
            return stream.ToArray();
        }


        ////about ras///
        public static RSAParameters ParsePublicKey(string publicKey)
        {
            string modulusText = publicKey.Substring(0, 0x100);
            string exponentText = publicKey.Substring(0x100);
            byte[] modulus = StringHelper.HexStringToByteArray(modulusText);
            byte[] exponent = StringHelper.HexStringToByteArray(exponentText);
            //Create   a   new   instance   of   RSACryptoServiceProvider.
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            //Create   a   new   instance   of   RSAParameters.
            RSAParameters RSAKeyInfo = new RSAParameters();
            //Set   RSAKeyInfo   to   the   public   key   values.  
            RSAKeyInfo.Modulus = modulus;
            RSAKeyInfo.Exponent = exponent;

            //Import   key   parameters   into   RSA.
            RSA.ImportParameters(RSAKeyInfo);

            return RSAKeyInfo;
        }

       public static  byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.ImportParameters(RSAKeyInfo);
            return RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
        }

       public static  byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.ImportParameters(RSAKeyInfo);
            return RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
        }

        ///   <summary>
        ///   RSA.Encrypt
        ///   </summary>
        ///   <param   name= "DataToEncrypt "> </param>
        ///   <param   name= "publicKey "> try   128bit   bytes </param>
        ///   <param   name= "exponent "> any   bytes </param>
        ///   <param   name= "DoOAEPPadding "> </param>
        ///   <returns> </returns>
        public static  byte[] RSAEncrypt(byte[] DataToEncrypt, byte[] publicKey, byte[] exponent, bool DoOAEPPadding)
        {
            RSAParameters RSAKeyInfo = new RSAParameters();
            RSAKeyInfo.Modulus = publicKey;
            RSAKeyInfo.Exponent = exponent;

            return RSAEncrypt(DataToEncrypt, RSAKeyInfo, DoOAEPPadding);
        }

       public  static  byte[] RSADecrypt(byte[] DataToDecrypt, byte[] publicKey, byte[] exponent, bool DoOAEPPadding)
        {
            RSAParameters RSAKeyInfo = new RSAParameters();
            RSAKeyInfo.Modulus = publicKey;
            RSAKeyInfo.Exponent = exponent;

            return RSADecrypt(DataToDecrypt, RSAKeyInfo, DoOAEPPadding);
        }

        public static  byte[] RSAEncrypt(byte[] DataToEncrypt, bool DoOAEPPadding)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            return RSAEncrypt(DataToEncrypt, RSA.ExportParameters(true), DoOAEPPadding);

        }

       public static  byte[] RSADecrypt(byte[] DataToDecrypt, bool DoOAEPPadding)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            return RSADecrypt(DataToDecrypt, RSA.ExportParameters(true), DoOAEPPadding);
        }
    }
}
