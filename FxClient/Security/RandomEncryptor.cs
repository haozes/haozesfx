using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Haozes.FxClient.Security
{
    public class RandomEncryptor
    {
        // Fields
        private readonly TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

        // Methods
        public RandomEncryptor()
        {
            this.tdes.GenerateIV();
            this.tdes.GenerateKey();
        }

        public string Decrypt(string data)
        {
            ICryptoTransform transform = this.tdes.CreateDecryptor();
            byte[] inputBuffer = Convert.FromBase64String(data);
            byte[] bytes = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
            return Encoding.UTF8.GetString(bytes);
        }

        public string Encrypt(string data)
        {
            ICryptoTransform transform = this.tdes.CreateEncryptor();
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(transform.TransformFinalBlock(bytes, 0, bytes.Length));
        }
    }
}
