using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace Haozes.FxClient.CommUtil
{
    public class LogUtil
    {
        private static log4net.ILog log;

        static LogUtil()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static void Init(log4net.ILog log4)
        {
            log = log4;
        }

        public static log4net.ILog Log
        {
            get
            {
                if (log == null)
                {
                    throw new NullReferenceException("未初始化Log4net实例");
                }
                return log;
            }
        }
    }
}
