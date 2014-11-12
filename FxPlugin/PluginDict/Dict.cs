using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Haozes.RobotIPlugin;
using System.Net;
using System.Text.RegularExpressions;

namespace Dict
{
    public class Translate:Iplugin
    {

        public string Name
        {
            get { return "fy"; }
        }

        public string Description
        {
            get { return "fy:content"; }
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
            GoogleTranslate gt = new GoogleTranslate();
            return   gt.Translate(cmdContent);
        }

        public string Remark
        {
            get { return "Translate english to chinese or chinese to english"; }
        }

        public string Execute(params string[] cmdParas)
        {
            return this.Execute(cmdParas[0]);
        }

        public bool IsHostOnly
        {
            get { return false; }
        }


        public bool NeedParseResult
        {
            get { return false; }
        }
    }
}
