using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Haozes.FxClient.CommUtil;
using Haozes.RobotIPlugin;
using Haozes.Robot.Utils;
using Haozes.FxClient.Core;
using Haozes.FxClient;

namespace Haozes.Robot
{
    public class RobotTaskAnalyzer : IRobotAnalyzer
    {
        private Client fetion;
        private string uri = string.Empty;
        private string taskInfo = string.Empty;
        private bool needParseResult = false;

        public RobotTaskAnalyzer(string _uri)
        {
            fetion = RobotCore.Fetion;
            uri = _uri;
        }

        #region IRobotAnalyzer 成员

        public void ParseMsg(string msg)
        {
            try
            {
                if (!CommonUtil.IsValidCMD(ref msg))
                    return;
                Cmd cmd = CommonUtil.GetCmd(msg);
                bool isInternalCmd = CommonUtil.IsInternalCmd(cmd.Name);
                this.taskInfo = msg;
                if (isInternalCmd)
                {//内部命令
                    InternalCmdExecutor intercmder = new InternalCmdExecutor(this, new InterCmd(cmd.Name, cmd.Para));
                    intercmder.Execute();
                }
                else
                {
                    if (PluginMgr.IsContainsCmd(cmd.Name))
                    {
                        Iplugin p = PluginMgr.GetPlugin(cmd.Name);
                        this.needParseResult = p.NeedParseResult;
                        Executor executor = new TaskExecutor(this, cmd.Para, p);
                        executor.Execute();
                    }
                }
            }
            catch (Exception ex)
            {
                RobotCore.Log.Error(ex.ToString());
            }

        }


        public string Uri
        {
            get { return this.uri; }
        }
        #endregion

        public void Send(string msg)
        {
            if (this.needParseResult && CommonUtil.IsValidCMD(ref msg))
            {
                this.ParseMsg(msg);
            }
            else
            {
                string[] uriArr = this.uri.Split('|');
                foreach (string uri in uriArr)
                {
                    this.Send(new SipUri(uri), msg);
                }
            }
        }

        public void Send(SipUri toUri, string msg)
        {
            if (fetion != null)
            {
                fetion.SendSMS(toUri, msg, true);
                RobotCore.Log.Info(string.Format("{0} 完成计划任务:{1}", DateTime.Now, taskInfo));
            }
        }

        public void Formward(string msg)
        {
            RobotCore.SendEMail(string.Empty, msg);
        }

    }
}
