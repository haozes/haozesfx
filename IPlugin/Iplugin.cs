using System;
using System.Collections.Generic;
using System.Text;

namespace Haozes.RobotIPlugin
{
    public interface Iplugin
    {
        /// <summary>
        /// 是否是授权用户才能使用
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is host only; otherwise, <c>false</c>.
        /// </value>
        bool IsHostOnly { get; }

        bool NeedParseResult { get; }

        /// <summary>
        /// 插件名称.eg:cmd
        /// </summary>
        string Name{get;}

        /// <summary>
        /// 描述
        /// </summary>
        string Description{get;}

        /// <summary>
        /// 支持命令列表(暂未使用)
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
        /// 执行命令方法
        /// </summary>
        /// <param name="cmdContent">命令参数</param>
        /// <returns></returns>
        string Execute(string cmdContent);

        string Execute(params string[] cmdParas);

        /// <summary>
        /// 备注
        /// </summary>
        string Remark { get;}
    }
}
