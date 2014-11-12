using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Haozes.FxClient.Sip
{
   public class LineReader
    {
        private static char[] newLine = new char[] { '\r', '\n' };
        private MemoryStream stream;

        public void Bind(MemoryStream stream)
        {
            this.stream = stream;
        }

        public string ReadLine()
        {
            long position = this.stream.Position;
            while (this.stream.Position < this.stream.Length)
            {
                if (((this.stream.ReadByte() == 13) && (this.stream.Position < this.stream.Length)) && (this.stream.ReadByte() == 10))
                {
                    break;
                }
            }
            long num2 = this.stream.Position;
            if (num2 <= position)
            {
                return null;
            }
            string str = Encoding.UTF8.GetString(this.stream.GetBuffer(), (int)position, (int)(num2 - position));
            if (str == null)
            {
                return null;
            }
            return str.TrimEnd(newLine);
        }
    }
}
