using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Haozes.FxClient;
using Haozes.FxClient.Core;
using Haozes.RobotIPlugin;
using Haozes.Robot.Utils;
using System.Configuration;

namespace Haozes.Robot
{
    public class RobotConvAnalyzer : IRobotAnalyzer
    {
        private Client fetion;
        private bool forward = false;
        private Contact contact;
        public Conversation conversation;

        public RobotConvAnalyzer() { }

        public RobotConvAnalyzer(Conversation _convWindow)
        {
            fetion = RobotCore.Fetion;
            conversation = _convWindow;
            contact = CommonUtil.FindContactByUri(conversation.From);
            if (contact == null)
                contact = new Contact(conversation.From);
            string allowForward = ConfigurationManager.AppSettings["AllowForward"];
            forward = (allowForward == "true" ? true : false);
        }

        public void ParseMsg(string msg)
        {
            try
            {
                RobotCore.Log.Info(string.Format("{0}�����Fetion������˵:{1}", contact.DisplayName, msg));
                bool isPermitUser = RobotCore.PermissionMgr.IsPermitUser(conversation.From.ToString());
                bool isValidCmd = CommonUtil.IsValidCMD(ref msg);
                if (!isValidCmd)
                {
                    SendCmdInvalid();
                    return;
                }
                Cmd cmd = CommonUtil.GetCmd(msg);
                bool isInternalCmd = CommonUtil.IsInternalCmd(cmd.Name);
                if (isInternalCmd)
                {//�ڲ�����
                    if (!isPermitUser && !CommonUtil.IsOpenInternalCmd(cmd.Name))
                    {
                        SendSuggest();
                    }
                    else
                    {
                        InternalCmdExecutor intercmder = new InternalCmdExecutor(this, new InterCmd(cmd.Name, cmd.Para));
                        intercmder.Execute();
                    }
                }
                else
                {//�������
                    
                    bool hasPlugin = PluginMgr.IsContainsCmd(cmd.Name);
                    if (hasPlugin)
                    {
                        Iplugin p = PluginMgr.GetPlugin(cmd.Name);
                        if (!isPermitUser && p.IsHostOnly)
                        {//δ��Ȩ�û�ִ���ض�����Ȩ����е�����
                            this.Send(string.Format("�������ֻ�����˲���ִ��Ŷ(w)"));
                            return;
                        }
                        Executor executor = new ConvExecutor(this, cmd.Para, p);
                        executor.Execute();
                    }
                    else
                    {
                        SendSuggest();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog("RobotConvAnalyzer ParseMsg occur error:" + ex.ToString()); ;
            }

        }

        private void SendCmdInvalid()
        {
            string failedParseStr = string.Format("����{0}�ķ��Ż�����,��ֻ��ִ��\"abc:abc\"���������:b", fetion.CurrentUser.NickName);
            Send(failedParseStr);
        }

        private void SendSuggest()
        {
            string suggestStr = "Ŀǰ֧�ֵ����" + Environment.NewLine;
            suggestStr += PluginMgr.GetPluginListDescription();
            Send(suggestStr);
        }

        public void Send(string msg)
        {
            if (fetion == null)
            {
                RobotCore.Log.WarnFormat("fetion client is disposed,message:{0} has not send", msg);
            }
            fetion.Send(this.conversation, msg);
        }

        public void Send(SipUri uri, string msg)
        {
            if (fetion != null)
            {
                fetion.SendMsg(uri, msg);
            }
        }

        public void WriteLog(string logInfo)
        {
            RobotCore.Log.Info(logInfo);
        }

        public string Uri
        {
            get { return contact.Uri.Raw; }
        }

        public void Formward(string mailContent)
        {
            mailContent = string.Format("SipUrl:{0} Display Name:{1}" + Environment.NewLine + "says:{2}", contact.Uri.ToString(),contact.DisplayName, mailContent);
            RobotCore.SendEMail(string.Empty, mailContent);
        }
    }
}
