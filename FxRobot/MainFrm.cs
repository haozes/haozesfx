using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;
using System.Diagnostics;
using Haozes.FxClient;
using Haozes.FxClient.Core;


namespace Haozes.Robot
{
    public partial class MainFrm : Form
    {
        private Client fetion;
        private int globalHotKey;
        private SynchronizationContext sc;
        public static MainFrm StaticInstance = null;
        public MainFrm()
        {
            sc = SynchronizationContext.Current;
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            this.Hide();
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            RegisterHotKey();
            LoginFx();
            StaticInstance = this;
        }
        #region 注册热键

        private void RegisterHotKey()
        {
            Keys key;
            try
            {
                key = (Keys)Enum.Parse(typeof(Keys), ConfigurationManager.AppSettings["HotKey"]);
            }
            catch
            {
                RobotCore.Log.Error("不正确的热键定义");
                return;
            }
            Hotkey hotkey = new Hotkey(this.Handle);
            globalHotKey = hotkey.RegisterHotkey(key, Hotkey.KeyFlags.MOD_CONTROL | Hotkey.KeyFlags.MOD_ALT);
            if (globalHotKey != 0)
            {
                hotkey.OnHotkey += new HotkeyEventHandler(hotkey_OnHotkey);
            }
            else
            {
                RobotCore.Log.Warn(string.Format("注册热键{0}失败,可能该热键已被占用", key.ToString()));
            }
        }

        #endregion
        private void InitFx()
        {
            fetion = new Client();
            fetion.Log = LogUtil.Log;
            fetion.LoginSucceed += new EventHandler(fetion_LoginSucceed);
            fetion.LoginFailed += new EventHandler<LoginEventArgs>(fetion_LoginFailed);
            fetion.VerifyCodeRequired = (reason, picBuffer) =>
                {
                    return this.InputVerifyCode(reason, picBuffer);
                };
            fetion.Load += new EventHandler(fetion_Load);
            fetion.Errored += new EventHandler<Haozes.FxClient.Core.FxErrArgs>(fetion_Errored);
            fetion.MsgReceived += new EventHandler<Haozes.FxClient.Core.ConversationArgs>(fetion_OnMsgReceived);
            fetion.Deregistered += new EventHandler(fetion_OnDeregistered);
            fetion.AddBuddyRequest += new EventHandler<ConversationArgs>(fetion_AddBuddyApplication);
            fetion.AddBuddyResult += new EventHandler<ConversationArgs>(fetion_AddBuddyResult);
            fetion.DeleteBuddyResult += new EventHandler<ConversationArgs>(fetion_DeleteBuddyResult);

        }

        /// <summary>
        /// 提示输入验证码对话框
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="picBuffer">验证码的buffer</param>
        /// <returns></returns>
        private string InputVerifyCode(string reason, byte[] picBuffer)
        {
            using (VerifyCodeFrm frm = new VerifyCodeFrm(reason, picBuffer))
            {
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    return frm.VerfyCode;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private void fetion_DeleteBuddyResult(object sender, ConversationArgs e)
        {
            if (e.Text == "200")
            {
                notify.ShowBalloonTip(100, "", "删除好友成功", ToolTipIcon.Info);
            }
        }

        private void fetion_AddBuddyResult(object sender, ConversationArgs e)
        {
            string result = e.Text;
            if (!string.IsNullOrEmpty(result))
            {
                notify.ShowBalloonTip(100, "", string.Format("添加好友:{0}成功!", result), ToolTipIcon.Info);
            }
            else
            {
                notify.ShowBalloonTip(100, "", "添加好友失败!", ToolTipIcon.Info);
            }
        }

        private void fetion_AddBuddyApplication(object sender, ConversationArgs e)
        {
            XDocument doc = XDocument.Parse(e.Text);
            string uri = doc.Element("events").Element("event").Element("application").Attribute("uri").Value;
            string desc = doc.Element("events").Element("event").Element("application").Attribute("desc").Value;
            string userid = doc.Element("events").Element("event").Element("application").Attribute("user-id").Value;

            fetion.SendToSelf(uri + "添加你为好友");
            fetion.AgreeAddBuddy(new SipUri(uri), userid);

            RobotCore.SendEMail("HaozesFx Robot Message", uri.ToString() + " 添加你的机器人为好友,机器人已自动添加他为好友");
        }


        private void LoginFx()
        {
            string mobile = ConfigurationManager.AppSettings["Telephone"];
            string pwd = ConfigurationManager.AppSettings["Password"];
            string flag = "encrypt";
            if (pwd.EndsWith(flag))
            {
                pwd = pwd.Substring(0, pwd.Length - flag.Length);
                pwd = Utils.CommonUtil.Decrypt(pwd, "haozesfx");
            }
            notify.ShowBalloonTip(800, "", "登陆中...", ToolTipIcon.Info);
            InitFx();
            fetion.Login(mobile, pwd);
        }

        private void LogoutFx()
        {
            fetion.Exit();
            fetion.LoginSucceed -= new EventHandler(fetion_LoginSucceed);
            fetion.LoginFailed -= new EventHandler<LoginEventArgs>(fetion_LoginFailed);
            fetion.Errored -= new EventHandler<Haozes.FxClient.Core.FxErrArgs>(fetion_Errored);
            fetion.MsgReceived -= new EventHandler<Haozes.FxClient.Core.ConversationArgs>(fetion_OnMsgReceived);
            fetion.Deregistered -= new EventHandler(fetion_OnDeregistered);
            fetion.AddBuddyRequest -= new EventHandler<ConversationArgs>(fetion_AddBuddyApplication);
            fetion.AddBuddyResult -= new EventHandler<ConversationArgs>(fetion_AddBuddyResult);
            fetion.DeleteBuddyResult -= new EventHandler<ConversationArgs>(fetion_DeleteBuddyResult);
        }

        private void fetion_OnDeregistered(object sender, EventArgs e)
        {
            RobotCore.Log.InfoFormat("用户已从其他客户端登陆,系统退出");
            LogoutFx();
            notify.ShowBalloonTip(500, "", "用户已从其他客户端登陆", ToolTipIcon.Info);
            if (sc != null)
                sc.Post(AlterLoginBtn, true);
        }

        private void fetion_LoginFailed(object sender, LoginEventArgs e)
        {
            notify.ShowBalloonTip(800, "", "登陆中失败,请查看日志:" + e.ToString(), ToolTipIcon.Error);
            ReLogin();
        }

        private void fetion_LoginSucceed(object sender, EventArgs e)
        {
            notify.ShowBalloonTip(100, "", "登陆成功", ToolTipIcon.Info);
            RobotCore.Log.Info("login successed!");
        }

        private void fetion_Load(object sender, EventArgs e)
        {
            notify.ShowBalloonTip(100, "", "获取联系人列表成功", ToolTipIcon.Info);
            menuItemLogin.Visible = false;
            menuItemRobotConfig.Enabled = true;
            notify.Text = "HaozesFx-" + fetion.CurrentUser.NickName;
            RobotCore.Init(fetion);
            fetion.SetPresence(PresenceStatus.Active);
        }

        private void fetion_OnMsgReceived(object sender, Haozes.FxClient.Core.ConversationArgs e)
        {
            if (sender != null)
            {
                Conversation conv = (Conversation)sender;
                if (e.MsgType == IMType.Text || e.MsgType == IMType.SMS)
                {
                    //hook robot here
                    RobotConvAnalyzer analyzer = new RobotConvAnalyzer(conv);
                    analyzer.ParseMsg(e.Text);
                }

            }
        }

        private void fetion_Errored(object sender, Haozes.FxClient.Core.FxErrArgs e)
        {
            RobotCore.Log.Error("fetion client occured error:", e.InnerException);
            if (e.Level == ErrLevel.Fatal && fetion.IsALive)
            {
                ReLogin();
            }

        }

        private void ReLogin()
        {
            LogoutFx();
            RobotCore.Stop();
            string info = "系统遇到错误,20秒后将重新登陆";
            RobotCore.Log.Info(info);
            notify.ShowBalloonTip(100, "", info, ToolTipIcon.Info);
            Thread.Sleep(20000);
            LoginFx();
        }

        private void hotkey_OnHotkey(int HotKeyID)
        {
            if (HotKeyID == globalHotKey)
            {
                bool isShow = this.notify.Visible;
                this.notify.Visible = !isShow;
            }
        }

        private void AlterLoginBtn(object state)
        {
            bool flag = (bool)state;
            menuItemLogin.Visible = flag;
            menuItemRobotConfig.Enabled = !flag;
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void robotConfigMenuItem_Click(object sender, EventArgs e)
        {
            RobotFrm.ShowFrm();
        }

        private void notify_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            RobotFrm.ShowFrm();
        }

        private void menuItemLogin_Click(object sender, EventArgs e)
        {
            LoginFx();
        }

        public void AddBuddy(SipUri buddyUri)
        {
            fetion.AddBuddy(buddyUri, string.Format("{0}的机器人,请求加你为好友", fetion.CurrentUser.NickName));
        }

        public void DeleteBuddy(SipUri buddyUri)
        {
            fetion.DeleteBuddy(buddyUri);
        }
    }
}
