using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Haozes.FxClient.Core;
using Haozes.Robot;
using Haozes.RobotIPlugin;
using Haozes.FxClient.CommUtil;
using Haozes.Robot.Utils;
namespace Haozes.Robot
{
    public partial class RobotFrm : Form
    {
        private static RobotFrm UniqueFrm;
        private System.Collections.Generic.Dictionary<int, bool> checkState;
        private bool selFlag = false;
        private IList<Contact> contactList;
        public RobotFrm()
        {
            InitializeComponent();
            UniqueFrm = this;
            contactList = new List<Contact>();
            LoadContacts();
            RobotCore.ContactsChanged += new EventHandler(RobotCore_ContactsChanged);
        }

        void RobotCore_ContactsChanged(object sender, EventArgs e)
        {
            LoadContacts();
            ContactListBindData();
            InitTargetNum();
            this.Invalidate();
        }

        public static void ShowFrm()
        {
            if (UniqueFrm == null)
            {
                new RobotFrm();
            }

            UniqueFrm.Show();
        }

        private void InitData()
        {
            InitTaskType();
            InitTargetNum();
            InitTaskList();

            ContactListBindData();
            PermitListBindData();
            PluginListBindData();
            IntervalBindData();
            TriggerBindData();
            txtVersion.Text = Application.ProductVersion;
        }

        private void InitTaskType()
        {
            IList<Iplugin> pluginList = PluginMgr.PlugList;
            cmbTask.DataSource = pluginList;
            cmbTask.DisplayMember = "Name";
            cmbTask.ValueMember = "Name";
        }

        private void InitTargetNum()
        {
            BindingManagerBase bindMB = cmbTarget.BindingContext[contactList];
            bindMB.SuspendBinding();
            bindMB.ResumeBinding();
            cmbTarget.DataSource = contactList;
            cmbTarget.DisplayMember = "DisplayName";
        }

        private void IntervalBindData()
        {
            for (int i = 1; i <= 60; i++)
            {
                dudInterval.Items.Add(i);
            }
            dudInterval.SelectedIndex = 1;
        }

        private void TriggerBindData()
        {
            cmbTrigger.Items.Add("天");
            cmbTrigger.Items.Add("小时");
            cmbTrigger.Items.Add("分钟");
            cmbTrigger.SelectedIndex = 1;
        }

        private void LoadContacts()
        {
            contactList.Clear();
            foreach (Contact c in RobotCore.Host.Contacts)
            {
                if (c.ContactTypeName != ContactType.BlockedBuddy)
                    contactList.Add(c);
            }
        }

        private void ContactListBindData()
        {
            BindingManagerBase bindMB = listContactList.BindingContext[contactList];
            bindMB.SuspendBinding();
            bindMB.ResumeBinding();
            listContactList.DataSource = contactList;
            listContactList.DisplayMember = "DisplayName";
        }

        private void PermitListBindData()
        {
            lstPermitContactList.DataSource = RobotCore.PermissionMgr.PermitContactList;
            lstPermitContactList.DisplayMember = "DisplayName";
        }

        private void PluginListBindData()
        {
            listBoxPlugins.DataSource = PluginMgr.PlugList;
            listBoxPlugins.DisplayMember = "Name";
        }

        private void ShowTabPage(TabPage tab)
        {
            tabOption.SelectedTab = tab;
        }

        #region leftbar linkclick method
        private void addTaskLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowTabPage(TaskPage);
        }

        private void aboutRobotLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowTabPage(AboutFXRobotPage);
        }
        private void linkTaskList_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowTabPage(pageTaskList);
        }

        private void LinkPlugin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowTabPage(pluginPage);
        }
        private void linkBuddy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowTabPage(pageBuddy);
        }
        #endregion leftbar linkclick method
        private void btnAddTask_Click(object sender, EventArgs e)
        {
            try
            {

                int interval = 0;
                if (!int.TryParse(dudInterval.Text.Trim(), out interval) || interval > 60)
                {
                    MessageBox.Show("请输入小于等于60的整数", "输入参数有误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                RobotTask task = new RobotTask();
                string date = dateTimePicker.Value.ToShortDateString();
                string time = timePicker.Value.ToLongTimeString();
                string taskTimeStr = string.Concat(date, " ", time);

                DateTime taskTime = DateTime.Parse(taskTimeStr);
                task.TaskTime = taskTime;
                task.NextTaskTime = taskTime;
                string cmd = cmbTask.SelectedValue.ToString();
                string para = txtPara.Text.Trim();
                task.TaskInfo = string.Format("{0}:{1}", cmd, para);
                if (chkLoop.Checked)
                {
                    task.Interval = interval;
                    if (cmbTrigger.SelectedIndex == 0)
                        task.Trigger = TaskTrigger.Daily;
                    else if (cmbTrigger.SelectedIndex == 1)
                        task.Trigger = TaskTrigger.Hourly;
                    else
                        task.Trigger = TaskTrigger.Minutely;
                }
                else
                {
                    task.Interval = -1;
                    task.Trigger = TaskTrigger.Once;
                }
                if (chkBoxSelf.Checked)
                {
                    task.TargetName = "自己";
                    task.TargetNum = RobotCore.Host.Uri.Raw.ToString();
                }
                else
                {
                    if (chkAllContact.Checked)
                    {
                        foreach (var c in contactList)
                        {
                            task.TargetNum += c.Uri.Raw.ToString() + "|";
                            task.TargetName += c.DisplayName + "|";
                        }
                    }
                    else
                    {
                        task.TargetNum += ((Contact)cmbTarget.SelectedValue).Uri.Raw.ToString();
                        task.TargetName += ((Contact)cmbTarget.SelectedValue).DisplayName;
                    }
                }

                task.TargetNum = task.TargetNum.TrimEnd('|');
                task.TargetName = task.TargetName.TrimEnd('|');
                RobotCore.TaskMgr.Add(task);
                MessageBox.Show("添加成功！");
                RefreshTaskList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                RobotCore.Log.Error(ex.ToString());
            }

        }
        private SQLiteHelper dbHelper
        {
            get { return SQLiteHelper.Instance; }
        }

        private void RobotFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UniqueFrm = null;
        }

        private void RobotFrm_Shown(object sender, EventArgs e)
        {
            
        }

        private void InitTaskList()
        {

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = RobotCore.TaskMgr.TaskList;

            // The check box column will be virtual.
            dataGridView1.VirtualMode = true;
            dataGridView1.Columns.Insert(0, new DataGridViewCheckBoxColumn());

            // Don't allow the column to be resizable.
            dataGridView1.Columns[0].Resizable = DataGridViewTriState.False;

            // Make the check box column frozen so it is always visible.
            dataGridView1.Columns[0].Frozen = true;

            // Put an extra border to make the frozen column more visible
            dataGridView1.Columns[0].DividerWidth = 1;

            // Make all columns except the first read only.
            foreach (DataGridViewColumn c in dataGridView1.Columns)
                if (c.Index != 0) c.ReadOnly = true;
            checkState = new Dictionary<int, bool>();
        }
        #region dataGridView1 inside method
        private void UpdateStatusBar()
        {
            // Calculate the number of checked values in the dictionary and update the status bar.
            int number = 0;
            foreach (bool isChecked in checkState.Values)
            {
                if (isChecked) number++;
            }

            toolStripStatusLabel1.Text = "Number of task selected: " + number.ToString(System.Globalization.CultureInfo.CurrentUICulture);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Update the status bar when the cell value changes.
            if (e.ColumnIndex == 0)
            {
                // Force the update of the value for the checkbox column.
                // Without this, the value doens't get updated until you move off from the cell.
                dataGridView1.Rows[e.RowIndex].Cells[0].Value = (bool)dataGridView1.Rows[e.RowIndex].Cells[0].EditedFormattedValue;

            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
        }
        private void dataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                int taskID = (int)dataGridView1.Rows[e.RowIndex].Cells["taskID"].Value;
                if (checkState.ContainsKey(taskID))
                {
                    e.Value = checkState[taskID];
                }
                else
                    e.Value = false;
            }
            if (e.ColumnIndex == 5)
            {
                string interval = dataGridView1.Rows[e.RowIndex].Cells["interval"].Value.ToString();
                string trigger = dataGridView1.Rows[e.RowIndex].Cells["trigger"].Value.ToString();
                TaskTrigger t = (TaskTrigger)Enum.Parse(typeof(TaskTrigger), trigger);
                if (t == TaskTrigger.Once)
                    e.Value = "无";
                else
                    e.Value = string.Format("每{0}{1}", interval, CommonUtil.GetEnumDescription(t));
            }
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Update the status bar when the cell value changes.
            if (e.ColumnIndex == 0 && e.RowIndex != -1)
            {
                // Get the taskID from the taskID column.
                int taskID = (int)dataGridView1.Rows[e.RowIndex].Cells["taskID"].Value;
                checkState[taskID] = (bool)dataGridView1.Rows[e.RowIndex].Cells[0].Value;

                this.UpdateStatusBar();
            }
        }



        private void dataGridView1_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            // Handle the notification that the value for a cell in the virtual column
            // needs to be pushed back to the dictionary.

            if (e.ColumnIndex == 0)
            {
                // Get the taskID from the taskID column.
                int taskID = (int)dataGridView1.Rows[e.RowIndex].Cells["taskID"].Value;

                // Add or update the checked value to the dictionary depending on if the 
                // key (taskID) already exists.
                if (!checkState.ContainsKey(taskID))
                {
                    checkState.Add(taskID, (bool)e.Value);
                }
                else
                    checkState[taskID] = (bool)e.Value;
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Auto size columns after the grid data binding is complete.
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }
        #endregion dataGridView1 inside method
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            int i;
            selFlag = !selFlag;
            for (i = 0; i < this.dataGridView1.RowCount; i++)
            {
                this.dataGridView1.Rows[i].Cells[0].Value = selFlag;

            }

        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            bool hasDel = false;
            //刷新
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            //取得选中的行
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((bool)dataGridView1.Rows[i].Cells[0].EditedFormattedValue == true)
                {
                    try
                    {
                        hasDel = true;
                        int taskID = Int32.Parse(dataGridView1.Rows[i].Cells["taskID"].Value.ToString());
                        RobotCore.TaskMgr.Delete(taskID);

                    }
                    catch (Exception ex)
                    {
                        RobotCore.Log.Error(ex.ToString());
                        MessageBox.Show(ex.ToString());
                    }
                }
            }

            if (hasDel)
            {
                dataGridView1.DataSource = RobotCore.TaskMgr.TaskList;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshTaskList();
        }

        private void RefreshTaskList()
        {
            dataGridView1.DataSource = RobotCore.TaskMgr.TaskList;
        }

        private void btnAddPermission_Click(object sender, EventArgs e)
        {
            object selectedContact = listContactList.SelectedItem;
            if (selectedContact != null && !lstPermitContactList.Items.Contains(selectedContact))
            {
                AddPermitContact((Contact)selectedContact);
            }
            PermitListBindData();
            lstPermitContactList.Refresh();
        }

        private void btnDelPermit_Click(object sender, EventArgs e)
        {
            Contact c = (Contact)lstPermitContactList.SelectedItem;
            try
            {
                RobotCore.PermissionMgr.Delete(c);
            }
            catch (Exception ex)
            {
                RobotCore.Log.Error(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
            PermitListBindData();
            lstPermitContactList.Refresh();
        }

        private void AddPermitContact(Contact c)
        {
            try
            {
                RobotCore.PermissionMgr.Add(c);
            }
            catch (Exception ex)
            {
                RobotCore.Log.Error(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }

        private void listBoxPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            Iplugin plugin = (Iplugin)listBoxPlugins.SelectedValue;
            txtboxPluginDetail.Clear();
            StringBuilder sb = new StringBuilder();
            sb.Append("名称:" + plugin.Name + Environment.NewLine);
            sb.Append(plugin.Description + Environment.NewLine);
            sb.Append("备注：" + plugin.Remark + Environment.NewLine);
            txtboxPluginDetail.Text = sb.ToString();
        }

        private void linkOption_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowTabPage(tabPageOption);
        }

        private void chkBoxSelf_CheckedChanged(object sender, EventArgs e)
        {
            bool self = chkBoxSelf.Checked;
            if (self)
            {
                cmbTarget.Enabled = false;
            }
            else
            {
                cmbTarget.Enabled = true;
            }
        }

        private void chkLoop_CheckedChanged(object sender, EventArgs e)
        {
            bool isLoop = chkLoop.Checked;
            if (isLoop)
            {
                dudInterval.Enabled = true;
                cmbTrigger.Enabled = true;
            }
            else
            {
                dudInterval.Enabled = false;
                cmbTrigger.Enabled = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://solo.cnblogs.com");
        }

        private void btnAddBuddy_Click(object sender, EventArgs e)
        {
            string buddyUri = txtBuddy.Text.Trim();
            long num;
            if (buddyUri.Length < 8 || long.TryParse(buddyUri, out num) == false)
            {
                MessageBox.Show("输入的号码不正确");
                txtBuddy.Focus();
                return;
            }
            if (chkMobile.Checked)
            {
                buddyUri = string.Format("tel:{0}", buddyUri);
            }
            else
            {
                buddyUri = string.Format("sip:{0}", buddyUri);
            }
            MainFrm.StaticInstance.AddBuddy(new SipUri(buddyUri));
        }

        private void menuItemDelBuddy_Click(object sender, EventArgs e)
        {
            object item = listContactList.SelectedItem;
            if (item != null)
            {
                Contact c = (Contact)item;
                MainFrm.StaticInstance.DeleteBuddy(c.Uri);

            }
        }

        private void RobotFrm_Load(object sender, EventArgs e)
        {
            InitData();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            //刷新
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            //取得选中的行
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((bool)dataGridView1.Rows[i].Cells[0].EditedFormattedValue == true)
                {
                    try
                    {
                      //  hasDel = true;
                        int taskID = Int32.Parse(dataGridView1.Rows[i].Cells["taskID"].Value.ToString());
                        RobotCore.TaskMgr.Execute(taskID);

                    }
                    catch (Exception ex)
                    {
                        RobotCore.Log.Error(ex.ToString());
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        private void listContactList_MouseMove(object sender, MouseEventArgs e)
        {
            string strTip = string.Empty;
            int index = listContactList.IndexFromPoint(e.Location);
            if ((index >= 0) && (index < listContactList.Items.Count))
                strTip = ((Contact)listContactList.Items[index]).Uri.ToString();
            toolTip1.SetToolTip(listContactList, strTip);
        }

    }
}