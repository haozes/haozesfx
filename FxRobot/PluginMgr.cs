using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using Haozes.RobotIPlugin;

namespace Haozes.Robot
{
    public class PluginMgr
    {
        private static IList<Iplugin> pluginList = new List<Iplugin>();
        private const string PLUGINDIR = "Plugin";

        public static IList<Iplugin> PlugList
        {
            get
            {
                if (pluginList == null || pluginList.Count == 0)
                    pluginList = GetPluginList();
                return pluginList;
            }
        }

        public static bool IsContainsCmd(string cmd)
        {
            List<string> pluginNameList = new List<string>();
            foreach (Iplugin plugin in PlugList)
            {
                pluginNameList.Add(plugin.Name);
            }
            return pluginNameList.Contains(cmd);
        }

        public static Iplugin GetPlugin(string cmd)
        {
            Iplugin plugin = null;
            foreach (Iplugin p in PlugList)
            {
                if (p.Name == cmd)
                {
                    plugin = p;
                    break;
                }
            }
            return plugin;
        }

        public static string GetPluginListDescription()
        {
            StringBuilder sb = new StringBuilder();
            int i = 1;
            foreach (Iplugin p in GetPluginList())
            {
                sb.Append(i + ": " + p.Name + ":" + Environment.NewLine);
                sb.Append(p.Description + Environment.NewLine);
                i++;
            }
            sb.AppendLine(string.Format("{0}:{1}",i++,"ly"));
            sb.AppendLine("∏¯÷˜»À¡Ù—‘");
            return sb.ToString();
        }

        private static IList<Iplugin> GetPluginList()
        {
            IList<Iplugin> list = new List<Iplugin>();
            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PLUGINDIR);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                return list;
            }
            string[] dllfiles = Directory.GetFiles(dir, "*.DLL");
            for (int i = 0; i < dllfiles.Length; i++)
            {
                Assembly assembly = Assembly.LoadFile(dllfiles[i]);
                foreach (Type t in assembly.GetExportedTypes())
                {
                    if (t.IsClass && typeof(Iplugin).IsAssignableFrom(t))
                    {
                        Iplugin plugin = (Iplugin)Activator.CreateInstance(t);
                        list.Add(plugin);
                    }
                }

            }//end of for
            return list;
        }

    }
}
