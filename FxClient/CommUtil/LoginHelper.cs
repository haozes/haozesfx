using System;
using System.Collections.Generic;
using System.Text;
using Haozes.FxClient.Core;

namespace Haozes.FxClient.CommUtil
{
    public static class LoginHelper
    {
        public static PortInfo LoadPortInfo(string[] list)
        {
            PortInfo port = new PortInfo();
            port.Load(list);
            return port;
        }

        public static SipSysConfig LoadSipSysConfig(string xml)
        {
            SipSysConfig config = new SipSysConfig();
            config.Load(xml);
            return config;
        }
    }
}
