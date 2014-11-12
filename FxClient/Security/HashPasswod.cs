using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Haozes.FxClient.Security
{
   public class HashPasswod
    {
        public static string DoHashPassword(byte[] password)
        {
            byte[] bytes = BitConverter.GetBytes(Environment.TickCount);
            return DoHashPassword(password, bytes);
        }

        public static string DoHashPassword(byte[] password, byte[] b0)
        {
            using (SHA1 sha = SHA1.Create())
            {
                byte[] src = sha.ComputeHash(password);
                for (int i = 0; i < password.Length; i++)
                {
                    password[i] = 0;
                }
                byte[] dst = new byte[b0.Length + src.Length];
                System.Buffer.BlockCopy(b0, 0, dst, 0, b0.Length);
                System.Buffer.BlockCopy(src, 0, dst, b0.Length, src.Length);
                byte[] buffer3 = sha.ComputeHash(dst);
                byte[] buffer4 = new byte[b0.Length + buffer3.Length];
                System.Buffer.BlockCopy(b0, 0, buffer4, 0, b0.Length);
                System.Buffer.BlockCopy(buffer3, 0, buffer4, b0.Length, buffer3.Length);
                return ComputeAuthResponse.BinaryToHex(buffer4);
            }
        }

        public static string DoHashPassword(string pwd)
        {
            char[] chArray = pwd.ToCharArray();

            byte[] bytes = Encoding.UTF8.GetBytes(chArray);
            return HashPasswod.DoHashPassword(bytes);
        }
    }
}
