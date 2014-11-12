using System;
using System.Collections.Generic;
using System.Text;
using Haozes.Robot.Utils;
using System.Configuration;
using System.ComponentModel;
using Haozes.FxClient;
using System.Text.RegularExpressions;
using Haozes.FxClient.Core;

namespace Haozes.Robot
{
    public enum InternaCmdName
    {
        /// <summary>
        /// 添加好友
        /// </summary>
        [DescriptionAttribute("添加好友")]
        AddBuddy,
        /// <summary>
        /// 或取联系人信息
        /// </summary>
        [DescriptionAttribute("或取联系人信息")]
        GetContacts,
        /// <summary>
        /// 短信
        /// </summary>
        [DescriptionAttribute("短信")]
        Sms,
        /// <summary>
        /// 留言到手机(根据配置可能是转发到EMail)
        /// </summary>
        [DescriptionAttribute("留言到手机(根据配置可能是转发到EMail)")]
        Ly,
        /// <summary>
        /// 发布Buzz
        /// </summary>
        [DescriptionAttribute("发布Buzz(Gmail帐户需要关联Buzz)")]
        Buzz
    }

    public class InternalCmdExecutor
    {
        private IRobotAnalyzer analyzer;
        public IRobotAnalyzer Analyzer
        {
            get { return this.analyzer; }
            set { this.analyzer = value; }
        }

        private InterCmd cmd;
        public InterCmd Cmd
        {
            get { return this.cmd; }
            set { this.cmd = value; }
        }



        public InternalCmdExecutor()
        {
        }

        public InternalCmdExecutor(IRobotAnalyzer analyzer, InterCmd cmd)
        {
            this.analyzer = analyzer;
            this.cmd = cmd;
        }

        public void Execute()
        {
            switch (cmd.InterName)
            {
                case InternaCmdName.AddBuddy:
                    this.AddBuddy();
                    break;
                case InternaCmdName.GetContacts:
                    this.GetContacts();
                    break;
                case InternaCmdName.Sms:
                    this.SendSMS();
                    break;
                case InternaCmdName.Ly:
                    this.Ly();
                    break;
                case InternaCmdName.Buzz:
                    this.Buzz();
                    break;
                default:
                    break;
            }
        }



        private void AddBuddy()
        {
            if (analyzer == null)
                return;
            string regex = @"(sip:\d+)|(tel:\d+)";
            bool foundMatch = Regex.IsMatch(this.cmd.Para, regex, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (foundMatch)
            {
                string sipurl = Regex.Match(this.cmd.Para, regex).Value;
                Client fetion = RobotCore.Fetion;
                SipUri uri = new SipUri(sipurl);
                if (fetion != null)
                {
                    fetion.AddBuddy(uri, string.Empty);
                    string c = string.Format("向{0}的好友请求已发送", sipurl);
                    analyzer.Send(c);
                }
            }
        }

        private void GetContacts()
        {
            if (analyzer == null)
                return;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("姓名   Uri   Tel");
            Client fetion = RobotCore.Fetion;
            if (fetion != null)
            {
                foreach (Contact c in fetion.CurrentUser.Contacts)
                {
                    sb.AppendLine(string.Format("{0} {1} {2}", c.DisplayName, c.Uri.ToString(), c.MobileNo));
                }
            }
            analyzer.Send(sb.ToString());
        }

        private void SendSMS()
        {
            if (analyzer == null)
                return;
            Regex regexObj = new Regex(@"(\d{9,12})(.+)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (regexObj.IsMatch(this.cmd.Para))
            {
                string id = regexObj.Match(this.cmd.Para).Groups[1].Value;
                string content = regexObj.Match(this.cmd.Para).Groups[2].Value;
                long sid;
                bool validSid = long.TryParse(id, out sid);
                Client fetion = RobotCore.Fetion;
                if (fetion != null && validSid)
                {
                    Contact c = CommonUtil.FindContactBySid(sid);
                    if (c != null)
                    {
                        analyzer.Send(c.Uri, content);
                        string tmp = string.Format("向{0}的短信已发送", id);
                        analyzer.Send(tmp);
                    }
                }
            }
        }

        private void Ly()
        {
            if (analyzer == null)
                return;
            if (NeedForward())
            {
                analyzer.Formward(this.cmd.Para);
            }
            analyzer.Send("主人会很快听到你的消息!");
        }

        private void Buzz()
        {
            if (analyzer == null)
                return;
            RobotCore.SendEMail("buzz@gmail.com",this.cmd.Para,string.Empty);
            analyzer.Send("Buzz已发送!");
        }

        private bool NeedForward()
        {
            string allowForward = ConfigurationManager.AppSettings["AllowForward"];
            bool forward = (allowForward == "true" ? true : false);
            return forward;
        }
    }
}
