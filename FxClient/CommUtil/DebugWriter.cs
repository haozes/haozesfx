using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Haozes.FxClient.Sip;

namespace Haozes.FxClient.CommUtil
{
    public class DebugWriter
    {
        public static void WriteSendPacket(SipMessage p)
        {
            string info = Environment.NewLine + ">>>>>>>>>>>>>>>>> send pack start >>>>>>>>>>>>>>" + Environment.NewLine;
            info += p;
            info += Environment.NewLine + ">>>>>>>>>>>>>>>> send pack  end >>>>>>>>>>>>>>>>>>" + Environment.NewLine;
            LogUtil.Log.Debug(info);
        }

        public static void WriteRCVPacket(SipMessage p)
        {
            string info = Environment.NewLine + ">>>>>>>>>>>>>>>>> rcv pack start >>>>>>>>>>>>>>" + Environment.NewLine;
            info += p;
            info += Environment.NewLine + ">>>>>>>>>>>>>>>> rcv pack end >>>>>>>>>>>>>>>>>>" + Environment.NewLine;
            LogUtil.Log.Debug(info);
        }
    }
}
