using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Haozes.Robot
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.ThreadException += new ThreadExceptionEventHandler(ThreadExceptionFunction);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionFunction);
            Application.SetCompatibleTextRenderingDefault(false);
            //  if (RunningInstance() == null)
            //fix bug:The Undo operation encountered a context that is different from what was applied in the corresponding Set operation.
            //see more: http://www.eggheadcafe.com/software/aspnet/30443269/how-to-use-invokerequired-when-in-the-middle-of-a-dll.aspx
            WindowsFormsSynchronizationContext.AutoInstall = false;
            Application.Run(new MainFrm());
        }
        /// <summary>
        /// 使程序只运行一次
        /// </summary>
        /// <returns></returns>
        public static Process RunningInstance()
        {
            Process currentProcess = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);//与当前进程名相同的所有进程
            foreach (Process process in processes)
            {
                if (process.Id != currentProcess.Id)
                {
                    //也许有相同的进程名，这时应该比较是否是同一个文件
                    if (process.MainModule.FileName == currentProcess.MainModule.FileName)
                    {
                        return process;//之前的实例
                    }
                }
            }
            return null;
        }

        private static void ThreadExceptionFunction(Object sender, ThreadExceptionEventArgs e)
        {
#if !DEBUG
            ShowUnhandledExceptionDlg(e.Exception);
#endif
        }
        private static void UnhandledExceptionFunction(Object sender, UnhandledExceptionEventArgs args)
        {
#if !DEBUG
            ShowUnhandledExceptionDlg((Exception)args.ExceptionObject);
#endif
        }
        private static void ShowUnhandledExceptionDlg(Exception e)
        {

            Exception unhandledException = e;
            LogUtil.Log.Error(unhandledException);
            LogUtil.Log.Info("UnHandledException raised,Application Restart!!");
            #if !DEBUG
            Application.Restart();
            #endif
            /*
            DialogResult ar = ShowThreadExceptionDialog(e);
            if (DialogResult.Yes == ar)
            {
                Application.Exit();
            }*/
        }

        private static DialogResult ShowThreadExceptionDialog(Exception ex)
        {
            LogUtil.Log.Error(ex);

            return MessageBox.Show(ex.Message + "\r\n 囧,出错喽! 请重新启动应用程序",
                "Unexpected Error",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Error);

        }
    }
}
