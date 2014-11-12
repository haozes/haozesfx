using System;
using System.Collections.Generic;
using System.Text;
using Haozes.RobotIPlugin;

namespace PluginBus
{
    public class Bus:Iplugin
    {
        public bool IsHostOnly
        {
            get { return false; }
        }

        public bool NeedParseResult
        {
            get {return false; }
        }

        public string Name
        {
            get {return "bus"; }
        }

        public string Description
        {
            get { return "线路查询 bus:158 \r\n起点-终点查询 bus:黄科路口 银科路口\r\n站点查询 bus:火车站"; }
        }

        public IList<string> SupportedCommand()
        {
            throw new NotImplementedException();
        }

        public bool CanExecute(string cmdContent)
        {
            return true;
        }

        public string Execute(string cmdContent)
        {
            HfBus hfBus = new HfBus();
           return hfBus.Search(cmdContent);
        }

        public string Execute(params string[] cmdParas)
        {
            throw new NotImplementedException();
        }

        public string Remark
        {
            get { return "合肥公交查询"; }
        }
    }
}
