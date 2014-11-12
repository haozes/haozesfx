using System;
using System.Collections.Generic;
using System.Text;

namespace Haozes.RobotIPlugin
{
    public interface Iplugin
    {
        /// <summary>
        /// �Ƿ�����Ȩ�û�����ʹ��
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is host only; otherwise, <c>false</c>.
        /// </value>
        bool IsHostOnly { get; }

        bool NeedParseResult { get; }

        /// <summary>
        /// �������.eg:cmd
        /// </summary>
        string Name{get;}

        /// <summary>
        /// ����
        /// </summary>
        string Description{get;}

        /// <summary>
        /// ֧�������б�(��δʹ��)
        /// </summary>
        /// <returns></returns>
        IList<string> SupportedCommand();

        /// <summary>
        /// always true
        /// </summary>
        /// <param name="cmdContent"></param>
        /// <returns></returns>
        bool CanExecute(string cmdContent);

        /// <summary>
        /// ִ�������
        /// </summary>
        /// <param name="cmdContent">�������</param>
        /// <returns></returns>
        string Execute(string cmdContent);

        string Execute(params string[] cmdParas);

        /// <summary>
        /// ��ע
        /// </summary>
        string Remark { get;}
    }
}
