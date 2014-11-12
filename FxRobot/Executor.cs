using System;
using System.Collections.Generic;
using System.Text;
using Haozes.RobotIPlugin;
using Haozes.Robot.Utils;

namespace Haozes.Robot
{
    public abstract class Executor
	{
        protected delegate string asynchDelegate(string cmdContent);
        protected asynchDelegate asychExecute;
        protected string cmdContent;
        protected Iplugin plugin;
        protected IRobotAnalyzer analyzer;

        public  Executor(IRobotAnalyzer _convAnalyzer, string _cmdContent, Iplugin _plugin)
        {
            this.analyzer = _convAnalyzer;
            this.cmdContent = _cmdContent;
            this.plugin = _plugin;
            asychExecute = new asynchDelegate(plugin.Execute);
        }

        public virtual  void Execute()
        {
            string para = cmdContent;
            IAsyncResult ar = BeginExecute(para, ExecuteCallBack, null);
        }

        protected virtual void ExecuteCallBack(IAsyncResult ar)
        {
            string excResult = EndExecute(ar);
        }

        #region async execute the plugin method
        protected virtual IAsyncResult BeginExecute(string cmd, AsyncCallback callBack, Object stateObject)
        {
            try
            {
                return asychExecute.BeginInvoke(cmd, callBack, stateObject);
            }
            catch (Exception e)
            {
                RobotCore.Log.Error(e);
                throw e;
            }
        }

        protected virtual string EndExecute(IAsyncResult ar)
        {
            if (ar == null)
            {
                RobotCore.Log.Error(string.Format("{0}:参数不能为空",StringTable.TASK_EXCUTE_ERR));
                return StringTable.TASK_EXCUTE_ERR;
            }
            try
            {
                return asychExecute.EndInvoke(ar);
            }
            catch (Exception e)
            {
                RobotCore.Log.Error(e);
                return StringTable.TASK_EXCUTE_ERR;
            }
        }
        #endregion
	}
}
