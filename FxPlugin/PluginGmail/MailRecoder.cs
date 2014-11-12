using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Gmail
{
    public class MailRecoder
    {
        private string filePath = string.Empty;
        private StreamReader reader;
        private StreamWriter writer;
        public MailRecoder()
        {
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mail.dat");
            if (!File.Exists(filePath))
                File.Create(filePath);
        }
        public void Write(string id)
        {
            writer = new StreamWriter(File.Open(filePath, FileMode.Append));
            writer.Write(";" + id);
            writer.Close();
        }

        public bool IsExsit(string id)
        {
            bool result = false;
            reader = new StreamReader(File.OpenRead(filePath));
            string all = reader.ReadToEnd();
            reader.Close();
            if (string.IsNullOrEmpty(all))
                return false;
            string[] arr = all.Split(';');
            foreach (string item in arr)
            {
                if (item == id)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
