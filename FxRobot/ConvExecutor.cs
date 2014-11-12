using System;
using System.Collections.Generic;
using System.Text;
using Haozes.RobotIPlugin;
using Haozes.Robot.Utils;

namespace Haozes.Robot
{
    public class ConvExecutor:Executor
    {
        public ConvExecutor(IRobotAnalyzer _convAnalyzer, string _cmdContent, Iplugin _plugin)
            : base(_convAnalyzer, _cmdContent, _plugin)
        { }
        protected override void ExecuteCallBack(IAsyncResult ar)
        {
            string excResult = EndExecute(ar);

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
