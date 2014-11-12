using System;
using System.Collections.Generic;
using System.Text;
using Haozes.RobotIPlugin;
namespace Console
{
   public class ConsolePlugin:Iplugin
    {
        public string Name
        {
            get { return "cmd"; }
        }

        public string Description
        {
            get { return "cmd:console command"; }
        }

        public IList<string> SupportedCommand()
        {
            IList<string> list=new List<string>();
            return list;
        }

        public bool CanExecute(string cmdContent)
        {
            return true;
        }

        public string Execute(string cmdContent)
        {
            string exeName = cmdContent;
            string param = string.Empty;
            if (cmdContent.IndexOf(' ') != -1)
            {
                exeName = cmdContent.Substring(0, cmdContent.IndexOf(' '));
                param = cmdContent.Substring(cmdContent.IndexOf(' ') + 1);
            }
            string[] arr=new string[]{exeName,param};
            return this.Execute(arr);
            //RunProcess rp = new RunProcess();
            //rp.Run(exeName, param);
            //return (rp.HasError ? rp.Error : rp.Output);
        }

        public string Remark
        {
            get { return "执行开始-运行的cmd命令"; }
        }

        public string Execute(params string[] cmdParas)
        {
            string exeName = string.Empty;
            string param = string.Empty;
            if (cmdParas.Length > 1)
            {
                exeName = cmdParas[0];
                param = cmdParas[1];
            }
            RunProcess rp = new RunProcess();
            rp.Run(exeName, param);
            return (rp.HasError ? rp.Error : rp.Output);
        }

        public bool IsHostOnly
        {
            get { return true; }
        }


        public bool NeedParseResult
        {
            get { return false; }
        }
    }
}
