using System;
using System.Collections.Generic;
using System.Text;
using Haozes.FxClient.Core;
using Haozes.RobotIPlugin;

namespace Haozes.Robot
{
    public class RobotCore
    {
        private static Haozes.FxClient.Client fetion;
        private static RobotTaskMgr taskMgr;
        private static RobotPermissionMgr permissionMgr;
        public static event EventHandler ContactsChanged;

        public static void Init(Haozes.FxClient.Client client)
        {
            fetion = client;
            taskMgr = new RobotTaskMgr(fetion.CurrentUser);
            permissionMgr = new RobotPermissionMgr(fetion.CurrentUser);
            fetion.CurrentUser.ContactsManager.ContactsChanged += new EventHandler<ContactsChangedArg>(ContactsManager_ContactsChanged);
        }

        private static void ContactsManager_ContactsChanged(object sender, ContactsChangedArg e)
        {
            if (ContactsChanged != null)
            {
                ContactsChanged(null, e);
            }
        }

        public static RobotTaskMgr TaskMgr
        {
            get { return taskMgr; }
        }

        public static RobotPermissionMgr PermissionMgr
        {
            get { return permissionMgr; }
        }

        public static User Host
        {
            get { return fetion.CurrentUser; }
        }

        public static Haozes.FxClient.Client Fetion
        {
            get 
            {
                if (fetion!=null && fetion.IsALive)
                    return fetion;
                else
                    return null;
            }
        }

        public static SQLiteHelper DBHelper
        {
            get { return SQLiteHelper.Instance; }
        }

        public static log4net.ILog Log
        {
            get { return LogUtil.Log; }
        }

        public static void Stop()
        {
            if (taskMgr != null)
                taskMgr.Stop();
        }

        public static void SendEMail(string title, string msg)
        {
            SendEMail(string.Empty, title, msg);
        }

        public static void SendEMail(string to,string title, string msg)
        {
            if (string.IsNullOrEmpty(title))
                title = "HaozesFx Message"; ;
            string[] arr = new string[] { title, msg,to };
            System.Threading.ThreadPool.QueueUserWorkItem(AsyncSendEmail, arr);
        }

        private static void AsyncSendEmail(object obj)
        {
            string[] arr = (string[])obj;
            if (arr == null || arr.Length < 2)
            {
                RobotCore.Log.Warn("AsyncSendEmail by Invid arg");
            }
            else
            {
                string title = arr[0];
                string msg = arr[1];
                string to = arr[2];
                Iplugin p = PluginMgr.GetPlugin("mail");
                if (string.IsNullOrEmpty(to))
                    to = p.Remark;
                if (p != null)
                {
                    p.Execute(to, title, msg);
                }
                else
                {
                    Log.Warn("Can not find mail plugin dll,send to:"+to+",title:"+title + Environment.NewLine + msg+" failed!");
                }
            }
        }

    }
}
