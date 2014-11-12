using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Haozes.FxClient.CommUtil;
using Haozes.FxClient.Sip;
using System.Diagnostics;

namespace Haozes.FxClient.Core
{
    public abstract class BaseSipConnection
    {
        protected Socket socket = null;
        protected string remoteIp = string.Empty;
        protected string remotePort = string.Empty;

        protected volatile int callID = 1;
        protected volatile int cseq = 1;
        protected volatile bool isClosed = false;
        protected Timer timerKeepLive;
        protected Timer timerKeepConnectionBusy;

        protected byte[] recvBuffers = new byte[0x1000];
        protected SipParser parser = new SipParser();

        public event EventHandler<ConversationArgs> MessageReceived;

        public BaseSipConnection()
            : this(null)
        {
        }

        public BaseSipConnection(Socket socket)
            : this(socket, 1)
        {
        }

        public BaseSipConnection(Socket socket, int callID)
        {
            this.callID = callID;
            this.socket = socket;
            this.parser.RequestReceived += new EventHandler<SipRequestReceivedEventArgs>(this.Parser_RequestReceived);
            this.parser.ResponseReceived += new EventHandler<SipResponseReceivedEventArgs>(this.Parser_ResponseReceived);
            this.parser.MessageParsingFailed += new EventHandler(this.Parser_MessageParsingFailed);
        }

        public virtual Socket SipSocket
        {
            get { return this.socket; }
            set { this.socket = value; }
        }

        public virtual void Connect(string ip, string port)
        {
            try
            {
                this.remoteIp = ip;
                this.remotePort = port;
                this.socket = TcpHelper.CreateSocket(ip, port);
            }
            catch (Exception ex)
            {
                ErrManager.RaiseError(new FxErrArgs(ErrLevel.Fatal, ex));
            }
        }

        public virtual System.Net.EndPoint LocalEndPoint
        {
            get { return this.socket.LocalEndPoint; }
        }

        public virtual string RemoteIP
        {
            get { return this.remoteIp; }
        }

        public virtual string RemotePort
        {
            get { return this.remotePort; }
        }

        public virtual int NextCallID()
        {
            return ++this.callID;
        }

        public virtual int CallID
        {
            get { return this.callID; }
            set { this.callID = value; }
        }

        public virtual int Cseq
        {
            get { return this.cseq; }
            set { this.cseq = value; }
        }

        public virtual int NextCseq()
        {
            return ++this.cseq;
        }

        public virtual void StartListen()
        {
            this.ListenAsync(this.socket);
        }

        public abstract void StartKeepLive();

        protected virtual void ListenAsync(object o)
        {
            if (this.isClosed)
                return;
            try
            {
                Socket state = o as Socket;
                state.BeginReceive(this.recvBuffers, 0, this.recvBuffers.Length, SocketFlags.None, new AsyncCallback(this.ReceiveData), state);
            }
            catch (Exception ex)
            {
                LogUtil.Log.Error("ListenAsync异常:" + ex.ToString());
                return;
            }
        }

        protected virtual void ReceiveData(IAsyncResult iar)
        {
            int recv = 0;
            Socket remote = null;
            try
            {
                remote = (Socket)iar.AsyncState;
                recv = remote.EndReceive(iar);
                byte[] data = new byte[recv];
                Buffer.BlockCopy(this.recvBuffers, 0, data, 0, data.Length);
                if (data.Length < 2)
                    return;
               // DebugWriter.WriteLine(">>>>>>>receive data:" + Encoding.UTF8.GetString(data));
                this.parser.Parse(data, 0, data.Length);
            }
            catch (SocketException ex)
            {
                string err = "ReceiveData异常:" + ex.ToString();
                if (ex.ErrorCode != (int)SocketError.OperationAborted)
                {//上个线程退出
                    return;
                }
                LogUtil.Log.Warn(err);
                return;
            }
            this.ListenAsync(remote);
        }

        public virtual void Send(SipMessage packet)
        {
            DebugWriter.WriteSendPacket(packet);
            try
            {
                TcpHelper.AsyncSend(this.socket, packet, this.SendCallback);
            }
            catch (SocketException ex)
            {
                LogUtil.Log.Error(ex);
                ErrManager.RaiseError(new FxErrArgs(ErrLevel.Fatal, ex));
            }
        }

        public virtual void SendCallback(IAsyncResult ar)
        {
            Socket socket = null;
            try
            {
                socket = ar as Socket;
            }
            catch (Exception exception)
            {
                string errInfo = string.Format("AysncSend:异步发送数据发生错误！:remoteip:{0},port:{1}",this.remoteIp, this.socket.LocalEndPoint.ToString());
                LogUtil.Log.Error(exception.ToString());
                ErrManager.RaiseError(new FxErrArgs(ErrLevel.Fatal, exception));
            }
        }

        public virtual void Close()
        {
            this.isClosed = true;

            this.parser.RequestReceived -= new EventHandler<SipRequestReceivedEventArgs>(this.Parser_RequestReceived);
            this.parser.ResponseReceived -= new EventHandler<SipResponseReceivedEventArgs>(this.Parser_ResponseReceived);
            this.parser.MessageParsingFailed -= new EventHandler(this.Parser_MessageParsingFailed);
            if (this.timerKeepLive != null)
            {
                this.timerKeepLive.Dispose();
            }
            if (this.socket != null)
            {
                this.socket.Shutdown(SocketShutdown.Both);
                this.socket.Close();
            }
        }

        protected virtual void Parser_MessageParsingFailed(object sender, EventArgs e)
        {
            LogUtil.Log.Error("数据解析失败");
        }

        protected virtual void Parser_ResponseReceived(object sender, SipResponseReceivedEventArgs e)
        {
            if (this.MessageReceived != null)
            {
                this.MessageReceived(this, new ConversationArgs(e.Response));
            }
        }

        protected virtual void Parser_RequestReceived(object sender, SipRequestReceivedEventArgs e)
        {
            if (this.MessageReceived != null)
            {
                this.MessageReceived(this, new ConversationArgs(e.Request));
            }
        }
    }
}
