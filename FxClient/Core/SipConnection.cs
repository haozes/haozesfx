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
    public class SipConnection : BaseSipConnection
    {


        protected virtual void KeepConnectionBusy(object state)
        {
            SipMessage hpPacket = PacketFactory.KeepConnectionBusy();
            this.Send(hpPacket);
        }

        protected virtual void KeepLive(object state)
        {
            SipMessage hpPacket = PacketFactory.GetKeepAlivePacket();
            this.Send(hpPacket);
        }

        public override void StartKeepLive()
        {
            int period = 3000 * 60;
            int dueTime = 10000;

            //心跳包一分钟调一次
            this.timerKeepLive = new Timer(new TimerCallback(this.KeepLive), this.socket, dueTime, period);

            //int kpbusyPeriod=1000*30;
            // this.timerKeepConnectionBusy = new Timer(new TimerCallback(this.KeepConnectionBusy),this.socket,0,kpbusyPeriod);
        }

    }
}
