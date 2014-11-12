using System;
using System.Collections.Generic;
using System.Text;
using Haozes.RobotIPlugin;
using Haozes.Robot.Utils;

namespace Haozes.Robot
{
	public  class TaskExecutor:Executor
	{
        public TaskExecutor(IRobotAnalyzer _convAnalyzer, string _cmdContent, Iplugin _plugin)
            : base(_convAnalyzer, _cmdContent, _plugin)
        { }

        protected override void ExecuteCallBack(IAsyncResult ar)
        {
            string excResult = EndExecute(ar);
            if (excResult.IndexOf(StringTable.TASK_EXCUTE_ERR) >= 0)
            {//ִ���������쳣
                return;
            }
            if (string.IsNullOrEmpty(excResult) || excResult == StringTable.NO_NEW_MAIL)
            {//Ĭ�Ͽս��
                RobotCore.Log.Info(string.Format("{0} ��ɼƻ�����:{1}:{2}", DateTime.Now, this.plugin.Name,cmdContent));
                return;
            }
            if (excResult.IndexOf(StringTable.CONTENT_SEPARATOR) < 0)
            {
                analyzer.Send(excResult);
            }
            else
            {
                string[] arr = excResult.Split(StringTable.CONTENT_SEPARATOR);
                foreach (string item in arr)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        analyzer.Send(item);
                    }
                }
            }//end of if

        }
	}
}
