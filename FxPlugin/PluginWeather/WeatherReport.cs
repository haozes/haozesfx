using System;
using System.Collections.Generic;
using System.Text;
using Haozes.RobotIPlugin;
namespace Weather
{
   public class WeatherReport:Iplugin
    {
        public string Name
        {
            get { return "tq"; }
        }

        public string Description
        {
            get {return "tq:城市名称 查询城市天气情况"; }
        }

        public IList<string> SupportedCommand()
        {
            IList<string> list = new List<string>();
            return list;
        }

        public bool CanExecute(string cmdContent)
        {
            return true;
        }

        public string Execute(string cmdContent)
        {
            //IWeather weather = new WebXmlWeather();
            IWeather weather = new GoogleWeather();
            return weather.Search(cmdContent);
        }

        public string Remark
        {
            get { return "天气预报查询插件"; }
        }

        public string Execute(params string[] cmdParas)
        {
            return this.Execute(cmdParas[0]);
        }

        public bool IsHostOnly
        {
            get { return false; }
        }


        public bool NeedParseResult
        {
            get { return false; }
        }
    }
}
