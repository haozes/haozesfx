using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Imps.Client.Data;
using Haozes.FxClient.CommUtil;
using Haozes.FxClient.Core;
using Haozes.Robot.Utils;

namespace Haozes.Robot
{
    public class RobotTaskMgr
    {
        //private Timer taskTimer;
        private System.Timers.Timer taskTimer;
        private User currentUser;
        private SQLiteHelper dbHelper;
        private IList<RobotTask> taskList = new List<RobotTask>();
        public RobotTaskMgr(User currentUser)
        {
            this.currentUser = currentUser;
            dbHelper = SQLiteHelper.Instance;
            InitializeDataBase();
            InitializeTimer();
        }
        public IList<RobotTask> TaskList
        {
            get { return taskList; }
        }
        private void InitializeDataBase()
        {

            dbHelper.InitializeDatabase(currentUser.Uri.Sid.ToString());
            List<string> tableList = this.dbHelper.Objects;
            StringBuilder builder;
            try
            {
                if (!tableList.Contains("Task"))
                {
                    builder = new StringBuilder();
                    builder.AppendLine("CREATE TABLE Task(");
                    builder.AppendLine("TaskID INTEGER PRIMARY KEY,");
                    builder.AppendLine("TaskInfo VARCHAR(256),");
                    builder.AppendLine("TargetNum VARCHAR(256),");
                    builder.AppendLine("TargetName VARCHAR(256),");
                    builder.AppendLine("TaskTime DATETIME,");
                    builder.AppendLine("NextTaskTime DATETIME,");
                    builder.AppendLine("Remark VARCHAR(256),");
                    builder.AppendLine("Trigger VARCHAR(256),");
                    builder.AppendLine("Interval int");
                    builder.AppendLine(")");
                    builder.AppendLine("");
                    this.dbHelper.ExecuteNonQuery(builder.ToString(), null);
                }
                Refresh();
            }
            catch (Exception ex)
            {
                RobotCore.Log.Error(ex.ToString());
            }
        }
        private void InitializeTimer()
        {
            taskTimer = new System.Timers.Timer();
            taskTimer.Enabled = true;
            taskTimer.Interval = 1000; //fixed bug:重复发短信
            //taskTimer.Tick += new EventHandler(TaskTimer_Tick);
            taskTimer.Elapsed += new System.Timers.ElapsedEventHandler(taskTimer_Elapsed);
            taskTimer.Start();
        }
        public void Stop()
        {
            taskTimer.Stop();
        }
        private  void Refresh()
        {
            SQLiteDataReader reader = RobotCore.DBHelper.ExecuteReader("SELECT * FROM TASK", null);
            taskList= CommonUtil.ToTaskList(reader);
        }

        private void taskTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            foreach (RobotTask task in this.taskList)
            {
                if(CanExecute(task))
                {
                    Execute(task);
                }
            }
        }

        public void Execute(RobotTask task)
        {
            IRobotAnalyzer taskAnalyzer = new RobotTaskAnalyzer(task.TargetNum);
            taskAnalyzer.ParseMsg(task.TaskInfo);
            if (task.Trigger == TaskTrigger.Once)
            {
                //如果是执行一次的任务,执行完毕删除
                Delete(task.TaskID);
            }
            else
            {
                IncreaseTaskNextTime(task);
            }
        }

        public void Execute(int taskID)
        {
            RobotTask currentTask = null;
            foreach (RobotTask task in this.taskList)
            {
                if (taskID == task.TaskID)
                {
                    currentTask = task;
                    break;
                }
            }
            if (currentTask != null)
            {
                Execute(currentTask);
            }
        }

        public  void Update(RobotTask task)
        {
            RobotCore.DBHelper.ExecuteNonQuery("UPDATE  Task set NextTaskTime=@NextTime WHERE TaskID=@TaskID", new object[] { task.TaskID,task.NextTaskTime });
            this.Refresh();
        }

        public  void Delete(int taskId)
        {
            RobotCore.DBHelper.ExecuteNonQuery("DELETE FROM Task WHERE TaskID=@TaskID", new object[] { taskId });
            this.Refresh();
        }

        public void Add(RobotTask task)
        {
            RobotCore.DBHelper.ExecuteNonQuery("INSERT INTO [Task] ([TaskInfo], [TargetNum],[TargetName],[TaskTime],[NextTaskTime],[Remark],[Trigger],[Interval]) VALUES (@TaskInfo, @TargetNum, @TargetName,@TaskTime,@NextTaskTime,@Remark,@Trigger,@Interval)",
                                  new object[] { task.TaskInfo, task.TargetNum, task.TargetName, task.TaskTime, task.NextTaskTime, task.Remark, task.Trigger.ToString(), task.Interval });
            this.Refresh();
        }

        private bool CanExecute(RobotTask task)
        {
            bool result = false;
            DateTime now=DateTime.Now;
            DateTime currentTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            DateTime nextTime = new DateTime(task.NextTaskTime.Year, task.NextTaskTime.Month, task.NextTaskTime.Day, task.NextTaskTime.Hour, task.NextTaskTime.Minute, task.NextTaskTime.Second);
            if (nextTime < currentTime)
            {
                task.NextTaskTime = GetNextTime(task);
            }
            if (task.NextTaskTime == currentTime)
            {
                result = true;
            }
            return result;
        }

        private  DateTime GetNextTime(RobotTask task)
        {
            int interval = task.Interval;
            DateTime old = task.NextTaskTime;
            DateTime temp = DateTime.Now;
            DateTime now = new DateTime(temp.Year, temp.Month, temp.Day, temp.Hour, temp.Minute, temp.Second);
            DateTime correctTime = DateTime.MaxValue;
            switch (task.Trigger)
            {
                case TaskTrigger.Daily:
                    int diff1 = (int)(now - task.NextTaskTime).TotalDays;
                    int span1 = diff1 / interval;
                    task.NextTaskTime = task.NextTaskTime.AddDays(span1 * interval);
                    if (task.NextTaskTime < now)
                        correctTime = task.NextTaskTime.AddDays(interval);
                    else
                        correctTime = now;
                    break;

                case TaskTrigger.Hourly:
                    int diff2 = (int)(now - task.NextTaskTime).TotalHours;
                    int span2 = diff2 / interval;
                    task.NextTaskTime = task.NextTaskTime.AddHours(span2 * interval);
                    if (task.NextTaskTime < now)
                        correctTime = task.NextTaskTime.AddHours(interval);
                    else
                        correctTime = now;
                    break;
                case TaskTrigger.Minutely:
                    int diff3 = (int)(now - task.NextTaskTime).TotalMinutes;
                    int span3 = diff3 / interval;
                    task.NextTaskTime = task.NextTaskTime.AddMinutes(span3 * interval);
                    if (task.NextTaskTime < now)
                        correctTime = task.NextTaskTime.AddMinutes(interval);
                    else
                        correctTime = now;
                    break;
                case TaskTrigger.Once:
                    correctTime = task.TaskTime;
                    break;
                default:
                    break;
            }
            return correctTime;
        }

        private void IncreaseTaskNextTime(RobotTask task)
        {
            switch (task.Trigger)
            {
                case TaskTrigger.Daily:
                    task.NextTaskTime = task.NextTaskTime.AddDays(task.Interval);
                    break;
                case TaskTrigger.Hourly:
                    task.NextTaskTime = task.NextTaskTime.AddHours(task.Interval);
                    break;
                case TaskTrigger.Minutely:
                    task.NextTaskTime = task.NextTaskTime.AddMinutes(task.Interval);
                    break;
                case TaskTrigger.Once:
                    Delete(task.TaskID);
                    break;
                default:
                    break;
            }
            Update(task);
        }
      

    }
}
