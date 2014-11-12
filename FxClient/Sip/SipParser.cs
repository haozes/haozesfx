using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Haozes.FxClient.Sip
{
    public class SipParser
    {
        private LineReader reader = new LineReader();
        private SipRequestReceivedEventArgs requestEventArgs = new SipRequestReceivedEventArgs();
        private SipResponseReceivedEventArgs responseEventArgs = new SipResponseReceivedEventArgs();
        private MemoryStream stream = new MemoryStream(0x800);
        public const int MaxMessageLength = 0x20000;

        public event EventHandler MessageParsingFailed;
        public event EventHandler<SipRequestReceivedEventArgs> RequestReceived;
        public event EventHandler<SipResponseReceivedEventArgs> ResponseReceived;

        public SipParser()
        {
            this.reader.Bind(this.stream);
        }

        private void MoveData(long pos)
        {
            if (pos == 0L)
            {
                this.stream.Position = this.stream.Length;
            }
            else if ((pos < 0L) || (pos >= this.stream.Length))
            {
                this.stream.Position = 0L;
                this.stream.SetLength(0L);
            }
            else
            {
                MemoryStream stream = new MemoryStream(0x800);
                stream.Write(this.stream.GetBuffer(), (int)pos, (int)(this.stream.Length - pos));
                this.stream = stream;
                this.reader.Bind(this.stream);
            }
        }

        private bool CheckLine(string line)
        {
            if (line == null)
            {
                return false;
            }
            byte[] buffer = this.stream.GetBuffer();
            return ((buffer[(int)((IntPtr)(this.stream.Position - 1L))] == 10) && (buffer[(int)((IntPtr)(this.stream.Position - 2L))] == 13));
        }

        private void TryToParse()
        {
            string line = this.reader.ReadLine();
            if (this.CheckLine(line))
            {
                bool flag;
                if (line.IndexOf(Protocol.Version) == 0)
                {
                    flag = this.ParseResponse(line);
                }
                else
                {
                    flag = this.ParseRequest(line);
                }
                long pos = 0L;
                while (flag)
                {
                    pos = this.stream.Position;
                    line = this.reader.ReadLine();
                    if (!this.CheckLine(line))
                    {
                        break;
                    }
                    if (line.IndexOf(Protocol.Version) == 0)
                    {
                        flag = this.ParseResponse(line);
                    }
                    else
                    {
                        flag = this.ParseRequest(line);
                    }
                }
                this.MoveData(pos);
            }
        }

        public void Parse(byte[] buffer, int pos, int len)
        {
            this.stream.Write(buffer, pos, len);
            if (this.stream.Length > 8L)
            {
                this.stream.Seek(0L, SeekOrigin.Begin);
                this.TryToParse();
            }
            if ((this.stream.Length >= MaxMessageLength) && (this.MessageParsingFailed != null))
            {
                this.MessageParsingFailed(this, EventArgs.Empty);
            }
        }

        private SipHeadField ParseHeadField(string name, string value)
        {
            name = name.ToUpper();
            if (SipKey.HeadFieldList.ContainsKey(name))
            {
                return new SipHeadField(name, value);
            }
            return null;
        }

        private bool ParseBody(SipMessage msg)
        {
            SipHeadField lengthField = msg.ContentLength;
            if ((lengthField != null) && (Int32.Parse(lengthField.Value) >= 1))
            {
                int len = Int32.Parse(lengthField.Value);
                if ((this.stream.Length - this.stream.Position) < len)
                {
                    return false;
                }
                msg.BodyBuffer = new byte[len];
                this.stream.Read(msg.BodyBuffer, 0, len);
            }
            return true;
        }

        private bool ParseHeaders(SipMessage msg)
        {
            for (string str = this.reader.ReadLine(); this.CheckLine(str); str = this.reader.ReadLine())
            {
                if (str.Length < 1)
                {//空白行解析开始Body部分
                    return this.ParseBody(msg);
                }
                int index = str.IndexOf(": ");
                if (index < 1)
                {
                    return false;
                }
                string name = str.Substring(0, index);
                SipHeadField header = this.ParseHeadField(name, str.Substring(index + 2));
                if (header != null)
                {
                    msg.HeadFields.Add(header);
                }
            }
            return false;
        }

        private bool CheckResponse(SipResponse rsp)
        {
            if (((rsp.CallID != null) && (rsp.CSeq != null)) && ((rsp.ContentLength == null) || ((int.Parse(rsp.ContentLength.Value) >= 0) && (int.Parse(rsp.ContentLength.Value) < MaxMessageLength))))
            {
                return true;
            }
            if (this.MessageParsingFailed != null)
            {
                this.MessageParsingFailed(this, EventArgs.Empty);
            }
            return false;
        }

        private bool ParseResponse(string line)
        {
            int blankIndex1 = line.IndexOf(" ");
            int blankIndex2 = line.IndexOf(" ", (int)(blankIndex1 + 1));
            if ((blankIndex1 < 1) || (blankIndex2 < 1))
            {
                if (this.MessageParsingFailed != null)
                {
                    this.MessageParsingFailed(this, EventArgs.Empty);
                }
                return false;
            }
            try
            {
                SipResponse msg = new SipResponse(int.Parse(line.Substring(blankIndex1, blankIndex2 - blankIndex1)), line.Substring(blankIndex2 + 1));
                if (this.ParseHeaders(msg))
                {
                    if (!this.CheckResponse(msg))
                    {
                        return false;
                    }
                    this.responseEventArgs.Response = msg;
                    if (this.ResponseReceived != null)
                    {
                        this.ResponseReceived(this, this.responseEventArgs);
                    }
                    return true;
                }
                return false;
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.ToString());
                return false;
            }
        }

        private bool CheckRequest(SipRequest req)
        {
            SipHeadField callId = req.CallID;
            SipHeadField cSeq = req.CSeq;

            string method = SipHelper.GetCSeqMethod(cSeq);
            if (((callId != null) && (cSeq != null)) && (method == req.Method))
            {
                SipHeadField contentLength = req.ContentLength;
                if (contentLength == null)
                {
                    return true;
                }
                int len = int.Parse(contentLength.Value);
                if ((len >= 0) && (len < MaxMessageLength))
                {
                    return true;
                }
            }
            if (this.MessageParsingFailed != null)
            {
                this.MessageParsingFailed(this, EventArgs.Empty);
            }
            return false;
        }

        private bool ParseRequest(string line)
        {
            if (line.Length >= 1)
            {
                string[] strArray = line.Split(new char[] { ' ' });
                if (((strArray.Length != 3) || (strArray[2] != Protocol.Version)) || !SipKey.MethodNameList.ContainsKey(strArray[0]))
                {
                    if (this.MessageParsingFailed != null)
                    {
                        this.MessageParsingFailed(this, EventArgs.Empty);
                    }
                    return false;
                }
                SipRequest msg = new SipRequest(strArray[0], strArray[1]);
                if (!this.ParseHeaders(msg))
                {
                    return false;
                }
                if (!this.CheckRequest(msg))
                {
                    return false;
                }
                this.requestEventArgs.Request = msg;
                if (this.RequestReceived != null)
                {
                    this.RequestReceived(this, this.requestEventArgs);
                }
            }
            return true;
        }

        public void Clear()
        {
            this.stream = new MemoryStream(0x800);
            this.reader.Bind(this.stream);
        }

        public string GetUnderlyContent()
        {
            if ((this.stream != null) && (this.stream.Length > 0L))
            {
                return Encoding.UTF8.GetString(this.stream.GetBuffer(), 0, (int)this.stream.Length);
            }
            return string.Empty;
        }
    }
}
