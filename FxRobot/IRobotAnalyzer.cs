using System;
using System.Collections.Generic;
using System.Text;
using Haozes.FxClient.Core;

namespace Haozes.Robot
{
	public interface IRobotAnalyzer
	{
        /// <summary>
        /// conversation �Է���Uri.raw
        /// </summary>
        string Uri { get; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="msg"></param>
        void ParseMsg(string msg);
        /// <summary>
        /// ���Ͷ���/��ʱ��Ϣ
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="toUri"></param>
        void Send(string msg);

        void Send(SipUri uri, string msg);

        void Formward(string msg);
	}
}
