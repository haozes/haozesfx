using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Haozes.FxClient.CommUtil;
using Haozes.FxClient.Sip;
using System.Diagnostics;

namespace Haozes.FxClient.Core
{
    public class ChatConnection : BaseSipConnection
    {
        public override void Connect(string ip, string port)
        {
            this.remoteIp = ip;
            //don't catch exception here
            this.socket = TcpHelper.CreateSocket(ip, port);
        }

        public override void Send(SipMessage packet)
        {
            try
            {
                TcpHelper.AsyncSend(this.socket, packet, this.SendCallback);
            }
            catch (SocketException ex)
            {
                LogUtil.Log.Debug("ChatConnection Send异常:" + ex.ToString());
                //not throw fata error,keep main thread not exit
                ErrManager.RaiseError(new FxErrArgs(ErrLevel.Critical, ex));

            }
        }

        public override void StartKeepLive()
        {

        }
    }
}
