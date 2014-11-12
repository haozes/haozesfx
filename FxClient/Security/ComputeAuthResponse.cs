using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Security.Cryptography;
using Haozes.FxClient.CommUtil;

namespace Haozes.FxClient.Security
{
    public class ComputeAuthResponse
    {
        // Fields
        private string _cnonce;
        private string _domain;
        private string _nonce;
        private string _password;
        private static Random _random;
        private string _sid;
        private bool _usingSHA1;

        // Methods
        public ComputeAuthResponse(string sid, string password, string domain, string nonce, bool usingSHA1)
        {
            this._sid = sid;
            this._password = password;
            this._domain = domain;
            this._nonce = nonce;
            this._usingSHA1 = usingSHA1;
            int seed = DateTime.Now.DayOfYear * 0xf4240;
            seed += DateTime.Now.Hour * 0x2710;
            seed += DateTime.Now.Minute * 100;
            seed += DateTime.Now.Second;
            _random = new Random(seed);
        }

        public static string BinaryToHex(byte[] binary)
        {
            StringBuilder builder = new StringBuilder();
            foreach (byte num in binary)
            {
                if (num > 15)
                {
                    builder.AppendFormat("{0:X}", num);
                }
                else
                {
                    builder.AppendFormat("0{0:X}", num);
                }
            }
            return builder.ToString();
        }

        private string ComputeH1(byte[] key)
        {
            string s = string.Format(":{0}:{1}", this._nonce, this._cnonce);
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            byte[] array = new byte[key.Length + bytes.Length];
            key.CopyTo(array, 0);
            Array.Copy(bytes, 0, array, key.Length, bytes.Length);
            return MD5ToHex(array);
        }

        private string ComputeH2()
        {
            string s = string.Format("REGISTER:{0}", this._sid);
            return MD5ToHex(Encoding.UTF8.GetBytes(s));
        }

        private byte[] ComputeKey()
        {
            if (this._usingSHA1)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(this._sid + ":" + this._domain + ":");
                byte[] src = HexToBinary(this._password.Substring(8));
                byte[] dst = new byte[bytes.Length + src.Length];
                Buffer.BlockCopy(bytes, 0, dst, 0, bytes.Length);
                Buffer.BlockCopy(src, 0, dst, bytes.Length, src.Length);
                return SecurityHelper.GetSha1(dst);
            }
            string s = string.Format("{0}:{1}:{2}", this._sid, this._domain, this._password);
            return MD5(Encoding.UTF8.GetBytes(s));
        }

        public string ComputeResponse()
        {
            byte[] key = this.ComputeKey();
            string str = this.ComputeH1(key);
            string str2 = this.ComputeH2();
            return this.ComputeResponse(str, str2);
        }

        private string ComputeResponse(string h1, string h2)
        {
            string s = string.Format("{0}:{1}:{2}", h1, this._nonce, h2);
            return MD5ToHex(Encoding.UTF8.GetBytes(s));
        }

        private static string GenNonce()
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            lock (_random)
            {
                num = _random.Next();
                num2 = _random.Next();
                num3 = _random.Next();
                num4 = _random.Next();
            }
            if ((num >> 0x18) < 0x10)
            {
                num += 0x10000000;
            }
            if ((num2 >> 0x18) < 0x10)
            {
                num2 += 0x10000000;
            }
            if ((num3 >> 0x18) < 0x10)
            {
                num3 += 0x10000000;
            }
            if ((num4 >> 0x18) < 0x10)
            {
                num4 += 0x10000000;
            }
            return string.Format("{0:X}{1:X}{2:X}{3:X}", new object[] { num, num2, num3, num4 });
        }

        public string GetCNonce()
        {
            this._cnonce = GenNonce();
            return this._cnonce;
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

        private static byte[] MD5(byte[] data)
        {
            return System.Security.Cryptography.MD5.Create().ComputeHash(data);
        }

        private static string MD5ToHex(byte[] data)
        {
            data = MD5(data);
            return BinaryToHex(data);
        }


        public static string ComputeNewResponse(string userid,string plainpwd,string key,string nonce)
        {
            string aesKey = "568AC8CA87A03B388903BFD6C7836B6A00FB32755CD68EEEE9CEDFD234DC8451";
            byte[] aesKeyArr = StringHelper.HexStringToByteArray(aesKey);

            byte[] passwordArr = StringHelper.HexStringToByteArray(SecurityHelper.EncryptV4(int.Parse(userid),plainpwd));
            byte[] nonceArr = Encoding.UTF8.GetBytes(nonce);
            // byte[] data = password+Encoding.UTF8.GetBytes(nonce)+hex2byteArray(AESKey)
            byte[] dst = new byte[passwordArr.Length + nonceArr.Length + aesKeyArr.Length];
            System.Array.Copy(nonceArr, 0, dst, 0, nonceArr.Length);
            System.Array.Copy(passwordArr, 0, dst, nonceArr.Length, passwordArr.Length);
            System.Array.Copy(aesKeyArr, 0, dst, nonceArr.Length + passwordArr.Length, aesKeyArr.Length);

            RSAParameters rsaKey = SecurityHelper.ParsePublicKey(key);
            byte[] encryptArr = SecurityHelper.RSAEncrypt(dst, rsaKey, false);
            string response = StringHelper.Hex2Str(encryptArr);
            return response;
        }
    }
}
