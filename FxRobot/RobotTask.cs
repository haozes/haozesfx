using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Haozes.Robot
{
    public class RobotTask
    {
        public RobotTask()
        { }

        public RobotTask(int _id, DateTime _time, string _info, string _num, string _targetName, string _remark, TaskTrigger _trigger, int _interval,DateTime _nextTime)
        {
            taskID = _id;
            taskTime = _time;
            taskInfo = _info;
            targetNum = _num;
            targetName = _targetName;
            remark = _remark;
            trigger = _trigger;
            interval = _interval;
            nextTaskTime = _nextTime;
        }
        public RobotTask(int _id, DateTime _time, string _info, string _num, string _targetName, string _remark, string _trigger, int _interval,DateTime _nextTime)
            : this(_id, _time, _info, _num, _targetName, _remark, (TaskTrigger)Enum.Parse(typeof(TaskTrigger), _trigger), _interval,_nextTime)
        {

        }

        public RobotTask(int _id, DateTime _time, string _info, string _num, string _targetName, string _remark)
            :this(_id, _time, _info, _num, _targetName, _remark, TaskTrigger.Once, -1,_time)
        {
        }
        public RobotTask(DateTime _time, string _info, string _num, string _targetName, string _remark)
            : this(0, _time, _info, _num, _targetName, _remark, TaskTrigger.Once, -1,_time)
        {
        }
        private int taskID = 0;

        public int TaskID
        {
            get { return taskID; }
            set { taskID = value; }
        }
        private DateTime taskTime = DateTime.MinValue;

        public DateTime TaskTime
        {
            get { return taskTime; }
            set { taskTime = value; }
        }
        private DateTime nextTaskTime = DateTime.MinValue;
        public DateTime NextTaskTime 
        {
            get {return nextTaskTime ;}
            set { nextTaskTime=value;}
        }

        private string taskInfo = string.Empty;
        public string TaskInfo
        {
            get { return taskInfo; }
            set { taskInfo = value; }
        }
        private string targetNum = string.Empty;

        public string TargetNum
        {
            get { return targetNum; }
            set { targetNum = value; }
        }

        private string targetName;

        public string TargetName
        {
            get { return targetName; }
            set { targetName = value; }
        }

        private string remark = string.Empty;

        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        private TaskTrigger trigger = TaskTrigger.Once;
        public TaskTrigger Trigger
        {
            get { return trigger; }
            set { trigger = value; }
        }
        private int interval = -1;
        public int Interval
        {
            get { return interval; }
            set { interval = value; }
        }

    }
    public enum TaskTrigger
    {
        [DescriptionAttribute("天")]
        Daily,
        [DescriptionAttribute("小时")]
        Hourly,
        [DescriptionAttribute("分钟")]
        Minutely,
        [DescriptionAttribute("无")]
        Once
    }
}
