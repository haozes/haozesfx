using System;
using System.Collections.Generic;
using System.Text;
using Haozes.FxClient.Core;

namespace Haozes.Robot
{
	public interface IRobotAnalyzer
	{
        /// <summary>
        /// conversation 对方的Uri.raw
        /// </summary>
        string Uri { get; }
        /// <summary>
        /// 解析命令
        /// </summary>
        /// <param name="msg"></param>
        void ParseMsg(string msg);
        /// <summary>
        /// 发送短信/或即时消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="toUri"></param>
        void Send(string msg);

        void Send(SipUri uri, string msg);

        void Formward(string msg);
	}
}
