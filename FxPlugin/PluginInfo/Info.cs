using System;
using System.Collections.Generic;
using System.Text;
using Haozes.RobotIPlugin;

namespace PluginInfo
{
    public class Info : Iplugin
    {
        public bool IsHostOnly
        {
            get { return false; }
        }

        public string Name
        {
            get { return "info"; }
        }

        public string Description
        {
            get { return "发送短信"; }
        }

        public IList<string> SupportedCommand()
        {
            return null;
        }

        public bool CanExecute(string cmdContent)
        {
            return true;
        }

        public string Execute(string cmdContent)
        {
            return cmdContent;
        }

        public string Execute(params string[] cmdParas)
        {
            return Execute(cmdParas[0]);
        }

        public string Remark
        {
            get { return "发送短信"; }
        }


        public bool NeedParseResult
        {
            get { return false; }
        }
    }
}
