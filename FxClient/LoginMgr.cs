using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Haozes.FxClient.LoginTemplate;
using Haozes.FxClient.CommUtil;
using Haozes.FxClient.Security;
using Haozes.FxClient.Core;
using Haozes.FxClient.Sip;
using System.Threading;
using System.IO;

namespace Haozes.FxClient
{
    public class LoginMgr
    {
        private User user;
        private MessageParser msgParser;
        private ConversationMgr convMgr;

        private Conversation convRegSipcStep1;
        private Conversation convRegSipcStep2;

        //sipc第二步验证中的response
        private string sipcResponse = string.Empty;

        public event EventHandler<LoginEventArgs> LoginSucceed;
        public event EventHandler<LoginEventArgs> LoginFailed;

        public Func<string, byte[], string> VerifyCodeRequired { get; set; }

        public ConversationMgr ConversationManager
        {
            get { return this.convMgr; }
        }

        public LoginMgr(User user)
        {
            this.user = user;
        }

        public void Login()
        {
            EventHandler<LoginEventArgs> handlerFailed = this.LoginFailed;
            string password = this.user.Password;
            try
            {
                this.GetSysConfig(this.user.MobileNo.ToString());
                this.SignInSSIServer(this.user.MobileNo.ToString(), password, string.Empty, string.Empty, string.Empty);
                this.SingInSipcServer();
            }
            catch (Exception ex)
            {
                if (handlerFailed != null)
                {
                    handlerFailed(this, new LoginEventArgs(ex.ToString()));
                }
            }
        }

        private void GetSysConfig(string mobile)
        {
            string postData = FetionTemplate.GetSystemConfigPostData(mobile);
            string configStr = HttpHelper.Post("http://nav.fetion.com.cn/nav/getsystemconfig.aspx", postData);
            if (!string.IsNullOrEmpty(configStr))
            {
                this.user.CreateSipSysConfig(configStr);
            }
        }

        private void SignInSSIServer(string mobile, string pwd, string pid, string pic, string algorithm)
        {
            string url;
            string digest = SecurityHelper.EncryptV4(pwd);
            if (String.IsNullOrEmpty(pid))
            {
                url = string.Format("{0}?mobileno={1}&domains=fetion.com.cn%3bm161.com.cn%3bwww.ikuwa.cn%3bgames.fetion.com.cn%3bturn.fetion.com.cn%3bpos.fetion.com.cn&v4digest-type=1&v4digest={2}", this.user.SysConfig.SsiAppSignIn, mobile, digest);
            }
            else
            {
                url = string.Format("{0}?mobileno={1}&domains=fetion.com.cn%3bm161.com.cn%3bwww.ikuwa.cn%3bgames.fetion.com.cn%3bturn.fetion.com.cn%3bpos.fetion.com.cn&pid={3}&pic={4}&algorithm={5}&v4digest-type=1&v4digest={2}",
                    this.user.SysConfig.SsiAppSignIn, mobile, digest, pid, pic, algorithm);
            }
            try
            {
                string[] resultList = HttpHelper.Get(url, string.Empty);
                LogUtil.Log.Debug("step1:获取SSIC成功!");
                this.user.CreatePortInfo(resultList);
            }
            catch (WebException ex)
            {//不知道为啥.NET要在http状态非200的时候抛异常?!
                var rsp = (HttpWebResponse)ex.Response;
                string responseXml;
                string status;
                string desc=string.Empty;
                using (Stream dataStream = rsp.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(dataStream, Encoding.GetEncoding("utf-8")))
                    {
                        responseXml = reader.ReadToEnd();
                        XDocument doc = XDocument.Parse(responseXml);
                        status= doc.Element("results").Attribute("status-code").Value;
                        desc = doc.Element("results").Attribute("desc") == null ? string.Empty : doc.Element("results").Attribute("desc").Value;
  
                    }
                }
                if ((int)rsp.StatusCode == 200)
                {
                    return;
                }
                else if ((int)rsp.StatusCode == 421)
                {
                    XDocument doc = XDocument.Parse(responseXml);
                    string tip = doc.Element("results").Element("verification").Attribute("text").Value;
                    algorithm = doc.Element("results").Element("verification").Attribute("algorithm").Value;
                    byte[] picBuffer;
                    HttpHelper.GetPicCodeV4(algorithm, out picBuffer, out pid);
                    string picRsp = this.VerifyCodeRequired(tip, picBuffer);
                    this.SignInSSIServer(mobile, pwd, pid, picRsp, algorithm);
                }
                else
                {
                    this.LoginFailed(this, new LoginEventArgs(string.Format("状态码:{0},{1}",rsp.StatusCode,desc)));
                }
            }
        }

        private void SingInSipcServer()
        {
            this.SingInSipcStep1();
        }

        private void SingInSipcStep1()
        {
            string sendData = FetionTemplate.RegesterSIPCStep1(this.user.Uri.Sid.ToString());
            string[] addrStrArr = this.user.SysConfig.SipcProxy.Split(':');
            CreateConnection(addrStrArr[0], addrStrArr[1]);
            //cn值是随机的,可以自己创建.
            SipMessage step1data = PacketFactory.RegSipcStep1("d62aa14003cb0de2f252afa755df43cf");
            this.convRegSipcStep1 = this.convMgr.Create(step1data, true);
            this.convRegSipcStep1.MsgRcv += new EventHandler<ConversationArgs>(this.ConvRegSipcStep1_OnMsgRcv);
        }

        void ConvRegSipcStep1_OnMsgRcv(object sender, ConversationArgs e)
        {
            string nonce;
            string rsaKey;
            this.GetNonceAndRSAKey(e.RawPacket.WWWAuthenticate.Value, out nonce, out rsaKey);
            this.convRegSipcStep1.MsgRcv -= new EventHandler<ConversationArgs>(this.ConvRegSipcStep1_OnMsgRcv);
            this.convMgr.Remove(this.convRegSipcStep1.CallID);
            this.SingInSipcStep2(nonce, rsaKey);
        }

        private void CreateConnection(string ip, string port)
        {
            this.user.Conncetion.Connect(ip, port);
            PacketFactory.Ower = this.user;
            this.convMgr = new ConversationMgr(this.user.Conncetion);
            this.msgParser = new MessageParser(this.convMgr);
            this.user.Conncetion.MessageReceived += new EventHandler<ConversationArgs>(this.msgParser.ReceiveSipMessage);
            this.user.Conncetion.StartListen();
        }

        private void SingInSipcStep2(string nonce, string rsaKey)
        {
            if (string.IsNullOrEmpty(nonce))
                return;

            this.sipcResponse = ComputeAuthResponse.ComputeNewResponse(this.user.UserId, this.user.Password, rsaKey, nonce);
            SipMessage step2data = PacketFactory.RegSipcStep2(this.sipcResponse);
            this.convRegSipcStep2 = this.convMgr.Create(step2data, true);
            this.convRegSipcStep2.MsgRcv += new EventHandler<ConversationArgs>(this.convRegSipcStep2_MsgRcv);
        }

        void convRegSipcStep2_MsgRcv(object sender, ConversationArgs e)
        {
            SipResponse rsp = e.RawPacket as SipResponse;
            if (rsp != null && rsp.StatusCode == 200)
            {
                LogUtil.Log.Debug("step2:Sign in SIPC Server Successful!");
                this.user.CreatePersonalInfo(rsp.Body);
                this.user.ContactsManager.InitContactList(rsp.Body);
                EventHandler<LoginEventArgs> handlerSuccess = this.LoginSucceed;
                if (handlerSuccess != null)
                {
                    handlerSuccess(this, null);
                    this.convRegSipcStep2.MsgRcv -= new EventHandler<ConversationArgs>(this.convRegSipcStep2_MsgRcv);
                    this.convMgr.Remove(this.convRegSipcStep2.CallID);
                }
            }
            else if (rsp != null && rsp.StatusCode == 421)
            {//需要验证码
                string wwwAuth = rsp.WWWAuthenticate.Value.Trim();
                IDictionary<string, string> hash = GetKeyValueList(wwwAuth);

                string algorithm = hash["Verify algorithm"];
                string type = hash["type"];
                var doc = XDocument.Parse(rsp.Body);
                string reason = doc.Element("results").Element("reason").Attribute("text").Value;
                byte[] picBuffer;
                string chid;
                HttpHelper.GetPicCodeV4(algorithm, out picBuffer, out chid);
                string picRsp = this.VerifyCodeRequired(reason, picBuffer);

                SipMessage step2data = PacketFactory.RegSipcStep2(this.sipcResponse, algorithm, type, picRsp, chid);
                this.user.Conncetion.Send(step2data);
            }
            else if (rsp != null && rsp.StatusCode == 401)
            {
                this.LoginFailed(this, new LoginEventArgs("密码错误"));
            }
            else if (rsp != null && rsp.StatusCode == 420)
            {
                this.LoginFailed(this, new LoginEventArgs("输入验证码错误"));
            }
            else if (rsp != null && rsp.StatusCode == 424)
            {
                this.LoginFailed(this, new LoginEventArgs("登陆次数超过限制"));
            }
            else if (rsp != null && rsp.StatusCode == 431)
            {
                this.LoginFailed(this, new LoginEventArgs("用户正在更换飞信号，暂时无法登陆"));
            }
            else if (rsp != null && rsp.StatusCode == 432)
            {
                this.LoginFailed(this, new LoginEventArgs("当前飞信号已停止使用"));
            }
            else if (rsp != null && rsp.StatusCode == 435)
            {
                this.LoginFailed(this, new LoginEventArgs("您未绑定安全邮箱或手机号"));
            }
            else
            {
                this.LoginFailed(this, new LoginEventArgs("step2:Sign in SIPC Server failed!"));
            }
        }
         
        private void GetNonceAndRSAKey(string authHead, out string nonce, out string rsaKey)
        {
            IDictionary<string, string> hash = this.GetKeyValueList(authHead);
            nonce = hash["nonce"];
            rsaKey = hash["key"];
        }

        /// <summary>
        /// 分割这样的字符串为键值对 Verify algorithm="picc-WeakPassword",type="GeneralPic"
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private IDictionary<string, string> GetKeyValueList(string str)
        {
            Dictionary<string, string> hash = new Dictionary<string, string>();
            foreach (string s in str.Split(','))
            {
                int i = s.IndexOf('=');
                if (i > 0)
                {
                    hash.Add(s.Substring(0, i).Trim(), s.Substring(i + 1).Trim().Replace("\"", ""));
                }
            }
            return hash;
        }

    }

    public class LoginEventArgs : EventArgs
    {
        private string description = string.Empty;

        public LoginEventArgs()
        {
        }

        public LoginEventArgs(string description)
        {
            this.description = description;
        }

        public override string ToString()
        {
            return this.description;
        }
    }
}
