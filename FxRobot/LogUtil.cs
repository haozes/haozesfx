using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace Haozes.Robot
{
    public class LogUtil
    {
        private static readonly log4net.ILog log = (log4net.ILog)LogManager.GetLogger("FxLog");
        static LogUtil()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static log4net.ILog Log
        {
            get
            {
                return log;
            }
        }
    }
}
