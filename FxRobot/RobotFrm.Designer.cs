namespace Haozes.Robot
{
	partial class RobotFrm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RobotFrm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkBuddy = new System.Windows.Forms.LinkLabel();
            this.linkOption = new System.Windows.Forms.LinkLabel();
            this.linkTaskList = new System.Windows.Forms.LinkLabel();
            this.LinkPlugin = new System.Windows.Forms.LinkLabel();
            this.aboutRobotLink = new System.Windows.Forms.LinkLabel();
            this.addTaskLink = new System.Windows.Forms.LinkLabel();
            this.tabPageOption = new System.Windows.Forms.TabPage();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnDelPermit = new System.Windows.Forms.Button();
            this.btnAddPermission = new System.Windows.Forms.Button();
            this.lstPermitContactList = new System.Windows.Forms.ListBox();
            this.listContactList = new System.Windows.Forms.ListBox();
            this.contextMenuBuddy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemDelBuddy = new System.Windows.Forms.ToolStripMenuItem();
            this.pageTaskList = new System.Windows.Forms.TabPage();
            this.btnExecute = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.taskID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.targetName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taskInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taskTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trigger = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.interval = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.targetNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nextTaskTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.TaskPage = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbTrigger = new System.Windows.Forms.ComboBox();
            this.dudInterval = new System.Windows.Forms.DomainUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.chkLoop = new System.Windows.Forms.CheckBox();
            this.chkBoxSelf = new System.Windows.Forms.CheckBox();
            this.btnAddTask = new System.Windows.Forms.Button();
            this.cmbTarget = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPara = new System.Windows.Forms.TextBox();
            this.labPara = new System.Windows.Forms.Label();
            this.cmbTask = new System.Windows.Forms.ComboBox();
            this.timePicker = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.tskTxt = new System.Windows.Forms.Label();
            this.timeTxt = new System.Windows.Forms.Label();
            this.dateTxt = new System.Windows.Forms.Label();
            this.tabOption = new System.Windows.Forms.TabControl();
            this.pageBuddy = new System.Windows.Forms.TabPage();
            this.btnAddBuddy = new System.Windows.Forms.Button();
            this.chkMobile = new System.Windows.Forms.CheckBox();
            this.txtBuddy = new System.Windows.Forms.TextBox();
            this.pluginPage = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtboxPluginDetail = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listBoxPlugins = new System.Windows.Forms.ListBox();
            this.AboutFXRobotPage = new System.Windows.Forms.TabPage();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.txtVersion = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkAllContact = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.tabPageOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.contextMenuBuddy.SuspendLayout();
            this.pageTaskList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.TaskPage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabOption.SuspendLayout();
            this.pageBuddy.SuspendLayout();
            this.pluginPage.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.AboutFXRobotPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.linkBuddy);
            this.panel1.Controls.Add(this.linkOption);
            this.panel1.Controls.Add(this.linkTaskList);
            this.panel1.Controls.Add(this.LinkPlugin);
            this.panel1.Controls.Add(this.aboutRobotLink);
            this.panel1.Controls.Add(this.addTaskLink);
            this.panel1.Location = new System.Drawing.Point(7, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(148, 352);
            this.panel1.TabIndex = 0;
            // 
            // linkBuddy
            // 
            this.linkBuddy.AutoSize = true;
            this.linkBuddy.LinkColor = System.Drawing.Color.Black;
            this.linkBuddy.Location = new System.Drawing.Point(34, 198);
            this.linkBuddy.Name = "linkBuddy";
            this.linkBuddy.Size = new System.Drawing.Size(55, 13);
            this.linkBuddy.TabIndex = 6;
            this.linkBuddy.TabStop = true;
            this.linkBuddy.Text = "添加好友";
            this.linkBuddy.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkBuddy_LinkClicked);
            // 
            // linkOption
            // 
            this.linkOption.AutoSize = true;
            this.linkOption.LinkColor = System.Drawing.Color.Black;
            this.linkOption.Location = new System.Drawing.Point(37, 120);
            this.linkOption.Name = "linkOption";
            this.linkOption.Size = new System.Drawing.Size(79, 13);
            this.linkOption.TabIndex = 5;
            this.linkOption.TabStop = true;
            this.linkOption.Text = "授权用户管理";
            this.linkOption.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkOption_LinkClicked);
            // 
            // linkTaskList
            // 
            this.linkTaskList.AutoSize = true;
            this.linkTaskList.LinkColor = System.Drawing.Color.Black;
            this.linkTaskList.Location = new System.Drawing.Point(37, 78);
            this.linkTaskList.Name = "linkTaskList";
            this.linkTaskList.Size = new System.Drawing.Size(55, 13);
            this.linkTaskList.TabIndex = 3;
            this.linkTaskList.TabStop = true;
            this.linkTaskList.Text = "任务计划";
            this.linkTaskList.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkTaskList_LinkClicked);
            // 
            // LinkPlugin
            // 
            this.LinkPlugin.AutoSize = true;
            this.LinkPlugin.LinkColor = System.Drawing.Color.Black;
            this.LinkPlugin.Location = new System.Drawing.Point(37, 163);
            this.LinkPlugin.Name = "LinkPlugin";
            this.LinkPlugin.Size = new System.Drawing.Size(55, 13);
            this.LinkPlugin.TabIndex = 2;
            this.LinkPlugin.TabStop = true;
            this.LinkPlugin.Text = "插件管理";
            this.LinkPlugin.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkPlugin_LinkClicked);
            // 
            // aboutRobotLink
            // 
            this.aboutRobotLink.AutoSize = true;
            this.aboutRobotLink.LinkColor = System.Drawing.Color.Black;
            this.aboutRobotLink.Location = new System.Drawing.Point(37, 240);
            this.aboutRobotLink.Name = "aboutRobotLink";
            this.aboutRobotLink.Size = new System.Drawing.Size(78, 13);
            this.aboutRobotLink.TabIndex = 1;
            this.aboutRobotLink.TabStop = true;
            this.aboutRobotLink.Text = "关于HaozesFx";
            this.aboutRobotLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.aboutRobotLink_LinkClicked);
            // 
            // addTaskLink
            // 
            this.addTaskLink.AutoSize = true;
            this.addTaskLink.LinkColor = System.Drawing.Color.Black;
            this.addTaskLink.Location = new System.Drawing.Point(37, 36);
            this.addTaskLink.Name = "addTaskLink";
            this.addTaskLink.Size = new System.Drawing.Size(79, 13);
            this.addTaskLink.TabIndex = 0;
            this.addTaskLink.TabStop = true;
            this.addTaskLink.Text = "添加任务计划";
            this.addTaskLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.addTaskLink_LinkClicked);
            // 
            // tabPageOption
            // 
            this.tabPageOption.Controls.Add(this.pictureBox2);
            this.tabPageOption.Controls.Add(this.btnDelPermit);
            this.tabPageOption.Controls.Add(this.btnAddPermission);
            this.tabPageOption.Controls.Add(this.lstPermitContactList);
            this.tabPageOption.Controls.Add(this.listContactList);
            this.tabPageOption.Location = new System.Drawing.Point(4, 22);
            this.tabPageOption.Name = "tabPageOption";
            this.tabPageOption.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOption.Size = new System.Drawing.Size(440, 326);
            this.tabPageOption.TabIndex = 4;
            this.tabPageOption.Text = "授权用户管理";
            this.tabPageOption.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(376, 104);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(45, 50);
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // btnDelPermit
            // 
            this.btnDelPermit.Location = new System.Drawing.Point(309, 121);
            this.btnDelPermit.Name = "btnDelPermit";
            this.btnDelPermit.Size = new System.Drawing.Size(53, 23);
            this.btnDelPermit.TabIndex = 4;
            this.btnDelPermit.Text = "->";
            this.btnDelPermit.UseVisualStyleBackColor = true;
            this.btnDelPermit.Click += new System.EventHandler(this.btnDelPermit_Click);
            // 
            // btnAddPermission
            // 
            this.btnAddPermission.Location = new System.Drawing.Point(126, 121);
            this.btnAddPermission.Name = "btnAddPermission";
            this.btnAddPermission.Size = new System.Drawing.Size(44, 23);
            this.btnAddPermission.TabIndex = 3;
            this.btnAddPermission.Text = "->";
            this.btnAddPermission.UseVisualStyleBackColor = true;
            this.btnAddPermission.Click += new System.EventHandler(this.btnAddPermission_Click);
            // 
            // lstPermitContactList
            // 
            this.lstPermitContactList.FormattingEnabled = true;
            this.lstPermitContactList.Location = new System.Drawing.Point(177, 4);
            this.lstPermitContactList.Name = "lstPermitContactList";
            this.lstPermitContactList.Size = new System.Drawing.Size(120, 316);
            this.lstPermitContactList.TabIndex = 1;
            // 
            // listContactList
            // 
            this.listContactList.ContextMenuStrip = this.contextMenuBuddy;
            this.listContactList.FormattingEnabled = true;
            this.listContactList.Location = new System.Drawing.Point(0, 3);
            this.listContactList.Name = "listContactList";
            this.listContactList.Size = new System.Drawing.Size(120, 316);
            this.listContactList.TabIndex = 0;
            this.listContactList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listContactList_MouseMove);
            // 
            // contextMenuBuddy
            // 
            this.contextMenuBuddy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemDelBuddy});
            this.contextMenuBuddy.Name = "contextMenuBuddy";
            this.contextMenuBuddy.Size = new System.Drawing.Size(123, 26);
            // 
            // menuItemDelBuddy
            // 
            this.menuItemDelBuddy.Name = "menuItemDelBuddy";
            this.menuItemDelBuddy.Size = new System.Drawing.Size(122, 22);
            this.menuItemDelBuddy.Text = "删除好友";
            this.menuItemDelBuddy.Click += new System.EventHandler(this.menuItemDelBuddy_Click);
            // 
            // pageTaskList
            // 
            this.pageTaskList.Controls.Add(this.btnExecute);
            this.pageTaskList.Controls.Add(this.dataGridView1);
            this.pageTaskList.Controls.Add(this.statusStrip1);
            this.pageTaskList.Controls.Add(this.btnDel);
            this.pageTaskList.Controls.Add(this.btnRefresh);
            this.pageTaskList.Controls.Add(this.btnSelectAll);
            this.pageTaskList.Location = new System.Drawing.Point(4, 22);
            this.pageTaskList.Name = "pageTaskList";
            this.pageTaskList.Padding = new System.Windows.Forms.Padding(3);
            this.pageTaskList.Size = new System.Drawing.Size(440, 326);
            this.pageTaskList.TabIndex = 3;
            this.pageTaskList.Text = "任务计划";
            this.pageTaskList.UseVisualStyleBackColor = true;
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(298, 275);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 7;
            this.btnExecute.Text = "立即执行";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.taskID,
            this.targetName,
            this.taskInfo,
            this.taskTime,
            this.loop,
            this.trigger,
            this.interval,
            this.targetNum,
            this.remark,
            this.nextTaskTime});
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(437, 272);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dataGridView1_CellValueNeeded);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            this.dataGridView1.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dataGridView1_CellValuePushed);
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // taskID
            // 
            this.taskID.DataPropertyName = "TaskID";
            this.taskID.HeaderText = "TaskID";
            this.taskID.Name = "taskID";
            this.taskID.Visible = false;
            // 
            // targetName
            // 
            this.targetName.DataPropertyName = "TargetName";
            this.targetName.HeaderText = "收信人";
            this.targetName.Name = "targetName";
            // 
            // taskInfo
            // 
            this.taskInfo.DataPropertyName = "TaskInfo";
            this.taskInfo.HeaderText = "命令";
            this.taskInfo.Name = "taskInfo";
            // 
            // taskTime
            // 
            this.taskTime.DataPropertyName = "TaskTime";
            this.taskTime.HeaderText = "执行时间";
            this.taskTime.Name = "taskTime";
            // 
            // loop
            // 
            this.loop.HeaderText = "循环";
            this.loop.Name = "loop";
            // 
            // trigger
            // 
            this.trigger.DataPropertyName = "Trigger";
            this.trigger.HeaderText = "trigger";
            this.trigger.Name = "trigger";
            this.trigger.Visible = false;
            // 
            // interval
            // 
            this.interval.DataPropertyName = "Interval";
            this.interval.HeaderText = "interval";
            this.interval.Name = "interval";
            this.interval.Visible = false;
            // 
            // targetNum
            // 
            this.targetNum.DataPropertyName = "TargetNum";
            this.targetNum.HeaderText = "Uri";
            this.targetNum.Name = "targetNum";
            // 
            // remark
            // 
            this.remark.DataPropertyName = "Remark";
            this.remark.HeaderText = "备注";
            this.remark.Name = "remark";
            this.remark.Visible = false;
            // 
            // nextTaskTime
            // 
            this.nextTaskTime.DataPropertyName = "NextTaskTime";
            this.nextTaskTime.HeaderText = "下次执行时间";
            this.nextTaskTime.Name = "nextTaskTime";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(3, 301);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(434, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "已选择:";
            this.statusStrip1.Visible = false;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(136, 17);
            this.toolStripStatusLabel1.Text = "Number of task selected: 0";
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(203, 275);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 3;
            this.btnDel.Text = "删除";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(109, 275);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(6, 275);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.TabIndex = 1;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // TaskPage
            // 
            this.TaskPage.Controls.Add(this.groupBox1);
            this.TaskPage.Location = new System.Drawing.Point(4, 22);
            this.TaskPage.Name = "TaskPage";
            this.TaskPage.Padding = new System.Windows.Forms.Padding(3);
            this.TaskPage.Size = new System.Drawing.Size(440, 326);
            this.TaskPage.TabIndex = 0;
            this.TaskPage.Text = "添加计划任务";
            this.TaskPage.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkAllContact);
            this.groupBox1.Controls.Add(this.cmbTrigger);
            this.groupBox1.Controls.Add(this.dudInterval);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.chkLoop);
            this.groupBox1.Controls.Add(this.chkBoxSelf);
            this.groupBox1.Controls.Add(this.btnAddTask);
            this.groupBox1.Controls.Add(this.cmbTarget);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtPara);
            this.groupBox1.Controls.Add(this.labPara);
            this.groupBox1.Controls.Add(this.cmbTask);
            this.groupBox1.Controls.Add(this.timePicker);
            this.groupBox1.Controls.Add(this.dateTimePicker);
            this.groupBox1.Controls.Add(this.tskTxt);
            this.groupBox1.Controls.Add(this.timeTxt);
            this.groupBox1.Controls.Add(this.dateTxt);
            this.groupBox1.Location = new System.Drawing.Point(6, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(427, 306);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "任务时间－提示信息";
            // 
            // cmbTrigger
            // 
            this.cmbTrigger.Enabled = false;
            this.cmbTrigger.FormattingEnabled = true;
            this.cmbTrigger.Location = new System.Drawing.Point(224, 63);
            this.cmbTrigger.Name = "cmbTrigger";
            this.cmbTrigger.Size = new System.Drawing.Size(121, 21);
            this.cmbTrigger.TabIndex = 14;
            // 
            // dudInterval
            // 
            this.dudInterval.Enabled = false;
            this.dudInterval.Items.Add("1");
            this.dudInterval.Items.Add("2");
            this.dudInterval.Items.Add("3");
            this.dudInterval.Items.Add("4");
            this.dudInterval.Items.Add("5");
            this.dudInterval.Location = new System.Drawing.Point(136, 62);
            this.dudInterval.Name = "dudInterval";
            this.dudInterval.Size = new System.Drawing.Size(73, 20);
            this.dudInterval.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(106, 72);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "每隔:";
            // 
            // chkLoop
            // 
            this.chkLoop.AutoSize = true;
            this.chkLoop.Location = new System.Drawing.Point(17, 67);
            this.chkLoop.Name = "chkLoop";
            this.chkLoop.Size = new System.Drawing.Size(74, 17);
            this.chkLoop.TabIndex = 11;
            this.chkLoop.Text = "是否循环";
            this.chkLoop.UseVisualStyleBackColor = true;
            this.chkLoop.CheckedChanged += new System.EventHandler(this.chkLoop_CheckedChanged);
            // 
            // chkBoxSelf
            // 
            this.chkBoxSelf.AutoSize = true;
            this.chkBoxSelf.Location = new System.Drawing.Point(11, 202);
            this.chkBoxSelf.Name = "chkBoxSelf";
            this.chkBoxSelf.Size = new System.Drawing.Size(86, 17);
            this.chkBoxSelf.TabIndex = 10;
            this.chkBoxSelf.Text = "发送给自己";
            this.chkBoxSelf.UseVisualStyleBackColor = true;
            this.chkBoxSelf.CheckedChanged += new System.EventHandler(this.chkBoxSelf_CheckedChanged);
            // 
            // btnAddTask
            // 
            this.btnAddTask.Location = new System.Drawing.Point(285, 245);
            this.btnAddTask.Name = "btnAddTask";
            this.btnAddTask.Size = new System.Drawing.Size(75, 23);
            this.btnAddTask.TabIndex = 8;
            this.btnAddTask.Text = "添加";
            this.btnAddTask.UseVisualStyleBackColor = true;
            this.btnAddTask.Click += new System.EventHandler(this.btnAddTask_Click);
            // 
            // cmbTarget
            // 
            this.cmbTarget.FormattingEnabled = true;
            this.cmbTarget.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmbTarget.Location = new System.Drawing.Point(251, 197);
            this.cmbTarget.Name = "cmbTarget";
            this.cmbTarget.Size = new System.Drawing.Size(110, 21);
            this.cmbTarget.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(202, 206);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "收件人：";
            // 
            // txtPara
            // 
            this.txtPara.Location = new System.Drawing.Point(217, 126);
            this.txtPara.Name = "txtPara";
            this.txtPara.Size = new System.Drawing.Size(143, 20);
            this.txtPara.TabIndex = 7;
            // 
            // labPara
            // 
            this.labPara.AutoSize = true;
            this.labPara.Location = new System.Drawing.Point(174, 128);
            this.labPara.Name = "labPara";
            this.labPara.Size = new System.Drawing.Size(43, 13);
            this.labPara.TabIndex = 6;
            this.labPara.Text = "参数：";
            // 
            // cmbTask
            // 
            this.cmbTask.FormattingEnabled = true;
            this.cmbTask.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmbTask.Location = new System.Drawing.Point(58, 126);
            this.cmbTask.Name = "cmbTask";
            this.cmbTask.Size = new System.Drawing.Size(110, 21);
            this.cmbTask.TabIndex = 5;
            // 
            // timePicker
            // 
            this.timePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.timePicker.Location = new System.Drawing.Point(224, 18);
            this.timePicker.Name = "timePicker";
            this.timePicker.ShowUpDown = true;
            this.timePicker.Size = new System.Drawing.Size(121, 20);
            this.timePicker.TabIndex = 4;
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker.Location = new System.Drawing.Point(59, 18);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(110, 20);
            this.dateTimePicker.TabIndex = 3;
            // 
            // tskTxt
            // 
            this.tskTxt.AutoSize = true;
            this.tskTxt.Location = new System.Drawing.Point(9, 128);
            this.tskTxt.Name = "tskTxt";
            this.tskTxt.Size = new System.Drawing.Size(43, 13);
            this.tskTxt.TabIndex = 2;
            this.tskTxt.Text = "命令：";
            // 
            // timeTxt
            // 
            this.timeTxt.AutoSize = true;
            this.timeTxt.Location = new System.Drawing.Point(175, 23);
            this.timeTxt.Name = "timeTxt";
            this.timeTxt.Size = new System.Drawing.Size(43, 13);
            this.timeTxt.TabIndex = 1;
            this.timeTxt.Text = "时间：";
            // 
            // dateTxt
            // 
            this.dateTxt.AutoSize = true;
            this.dateTxt.Location = new System.Drawing.Point(13, 23);
            this.dateTxt.Name = "dateTxt";
            this.dateTxt.Size = new System.Drawing.Size(43, 13);
            this.dateTxt.TabIndex = 0;
            this.dateTxt.Text = "日期：";
            // 
            // tabOption
            // 
            this.tabOption.Controls.Add(this.TaskPage);
            this.tabOption.Controls.Add(this.pageTaskList);
            this.tabOption.Controls.Add(this.tabPageOption);
            this.tabOption.Controls.Add(this.pageBuddy);
            this.tabOption.Controls.Add(this.pluginPage);
            this.tabOption.Controls.Add(this.AboutFXRobotPage);
            this.tabOption.Location = new System.Drawing.Point(159, 33);
            this.tabOption.Name = "tabOption";
            this.tabOption.SelectedIndex = 0;
            this.tabOption.Size = new System.Drawing.Size(448, 352);
            this.tabOption.TabIndex = 1;
            // 
            // pageBuddy
            // 
            this.pageBuddy.Controls.Add(this.btnAddBuddy);
            this.pageBuddy.Controls.Add(this.chkMobile);
            this.pageBuddy.Controls.Add(this.txtBuddy);
            this.pageBuddy.Location = new System.Drawing.Point(4, 22);
            this.pageBuddy.Name = "pageBuddy";
            this.pageBuddy.Padding = new System.Windows.Forms.Padding(3);
            this.pageBuddy.Size = new System.Drawing.Size(440, 326);
            this.pageBuddy.TabIndex = 8;
            this.pageBuddy.Text = "添加好友";
            this.pageBuddy.UseVisualStyleBackColor = true;
            // 
            // btnAddBuddy
            // 
            this.btnAddBuddy.Location = new System.Drawing.Point(76, 94);
            this.btnAddBuddy.Name = "btnAddBuddy";
            this.btnAddBuddy.Size = new System.Drawing.Size(75, 23);
            this.btnAddBuddy.TabIndex = 2;
            this.btnAddBuddy.Text = "添加好友";
            this.btnAddBuddy.UseVisualStyleBackColor = true;
            this.btnAddBuddy.Click += new System.EventHandler(this.btnAddBuddy_Click);
            // 
            // chkMobile
            // 
            this.chkMobile.AutoSize = true;
            this.chkMobile.Location = new System.Drawing.Point(199, 41);
            this.chkMobile.Name = "chkMobile";
            this.chkMobile.Size = new System.Drawing.Size(86, 17);
            this.chkMobile.TabIndex = 1;
            this.chkMobile.Text = "通过手机号";
            this.chkMobile.UseVisualStyleBackColor = true;
            // 
            // txtBuddy
            // 
            this.txtBuddy.Location = new System.Drawing.Point(76, 41);
            this.txtBuddy.Name = "txtBuddy";
            this.txtBuddy.Size = new System.Drawing.Size(100, 20);
            this.txtBuddy.TabIndex = 0;
            // 
            // pluginPage
            // 
            this.pluginPage.Controls.Add(this.groupBox2);
            this.pluginPage.Controls.Add(this.groupBox3);
            this.pluginPage.Location = new System.Drawing.Point(4, 22);
            this.pluginPage.Name = "pluginPage";
            this.pluginPage.Padding = new System.Windows.Forms.Padding(3);
            this.pluginPage.Size = new System.Drawing.Size(440, 326);
            this.pluginPage.TabIndex = 6;
            this.pluginPage.Text = "插件管理";
            this.pluginPage.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtboxPluginDetail);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(165, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(272, 320);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "插件说明";
            // 
            // txtboxPluginDetail
            // 
            this.txtboxPluginDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtboxPluginDetail.Location = new System.Drawing.Point(3, 16);
            this.txtboxPluginDetail.Multiline = true;
            this.txtboxPluginDetail.Name = "txtboxPluginDetail";
            this.txtboxPluginDetail.ReadOnly = true;
            this.txtboxPluginDetail.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtboxPluginDetail.Size = new System.Drawing.Size(266, 301);
            this.txtboxPluginDetail.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listBoxPlugins);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(162, 320);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "插件列表";
            // 
            // listBoxPlugins
            // 
            this.listBoxPlugins.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxPlugins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxPlugins.FormattingEnabled = true;
            this.listBoxPlugins.Location = new System.Drawing.Point(3, 16);
            this.listBoxPlugins.Name = "listBoxPlugins";
            this.listBoxPlugins.Size = new System.Drawing.Size(156, 299);
            this.listBoxPlugins.TabIndex = 0;
            this.listBoxPlugins.SelectedIndexChanged += new System.EventHandler(this.listBoxPlugins_SelectedIndexChanged);
            // 
            // AboutFXRobotPage
            // 
            this.AboutFXRobotPage.Controls.Add(this.linkLabel1);
            this.AboutFXRobotPage.Controls.Add(this.txtVersion);
            this.AboutFXRobotPage.Controls.Add(this.label8);
            this.AboutFXRobotPage.Controls.Add(this.label6);
            this.AboutFXRobotPage.Controls.Add(this.label5);
            this.AboutFXRobotPage.Controls.Add(this.label4);
            this.AboutFXRobotPage.Controls.Add(this.label1);
            this.AboutFXRobotPage.Location = new System.Drawing.Point(4, 22);
            this.AboutFXRobotPage.Name = "AboutFXRobotPage";
            this.AboutFXRobotPage.Padding = new System.Windows.Forms.Padding(3);
            this.AboutFXRobotPage.Size = new System.Drawing.Size(440, 326);
            this.AboutFXRobotPage.TabIndex = 7;
            this.AboutFXRobotPage.Text = "关于HaozesFx";
            this.AboutFXRobotPage.UseVisualStyleBackColor = true;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkColor = System.Drawing.Color.Black;
            this.linkLabel1.Location = new System.Drawing.Point(65, 165);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(120, 13);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://solo.cnblogs.com";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // txtVersion
            // 
            this.txtVersion.AutoSize = true;
            this.txtVersion.Location = new System.Drawing.Point(117, 199);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(0, 13);
            this.txtVersion.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(62, 200);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Version:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(62, 230);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(223, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "此软件仅个人学习所写，无其他任何用途";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(62, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Haozes@gmail.com";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(90, 59);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(62, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Author:Haozes";
            // 
            // chkAllContact
            // 
            this.chkAllContact.AutoSize = true;
            this.chkAllContact.Location = new System.Drawing.Point(103, 202);
            this.chkAllContact.Name = "chkAllContact";
            this.chkAllContact.Size = new System.Drawing.Size(98, 17);
            this.chkAllContact.TabIndex = 15;
            this.chkAllContact.Text = "发送给所有人";
            this.chkAllContact.UseVisualStyleBackColor = true;
            // 
            // RobotFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(616, 399);
            this.Controls.Add(this.tabOption);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "RobotFrm";
            this.Padding = new System.Windows.Forms.Padding(4, 0, 4, 43);
            this.Text = "HaozesFx";
            this.Load += new System.EventHandler(this.RobotFrm_Load);
            this.Shown += new System.EventHandler(this.RobotFrm_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RobotFrm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPageOption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.contextMenuBuddy.ResumeLayout(false);
            this.pageTaskList.ResumeLayout(false);
            this.pageTaskList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.TaskPage.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabOption.ResumeLayout(false);
            this.pageBuddy.ResumeLayout(false);
            this.pageBuddy.PerformLayout();
            this.pluginPage.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.AboutFXRobotPage.ResumeLayout(false);
            this.AboutFXRobotPage.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel aboutRobotLink;
        private System.Windows.Forms.LinkLabel addTaskLink;
        private System.Windows.Forms.LinkLabel LinkPlugin;
        private System.Windows.Forms.LinkLabel linkTaskList;
        private System.Windows.Forms.LinkLabel linkOption;
        private System.Windows.Forms.TabPage tabPageOption;
        private System.Windows.Forms.Button btnDelPermit;
        private System.Windows.Forms.Button btnAddPermission;
        private System.Windows.Forms.ListBox lstPermitContactList;
        private System.Windows.Forms.ListBox listContactList;
        private System.Windows.Forms.TabPage pageTaskList;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.TabPage TaskPage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkBoxSelf;
        private System.Windows.Forms.Button btnAddTask;
        private System.Windows.Forms.ComboBox cmbTarget;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPara;
        private System.Windows.Forms.Label labPara;
        private System.Windows.Forms.ComboBox cmbTask;
        private System.Windows.Forms.DateTimePicker timePicker;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.Label tskTxt;
        private System.Windows.Forms.Label timeTxt;
        private System.Windows.Forms.Label dateTxt;
        private System.Windows.Forms.TabControl tabOption;
        private System.Windows.Forms.TabPage pluginPage;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtboxPluginDetail;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox listBoxPlugins;
        private System.Windows.Forms.TabPage AboutFXRobotPage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DomainUpDown dudInterval;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkLoop;
        private System.Windows.Forms.ComboBox cmbTrigger;
        private System.Windows.Forms.DataGridViewTextBoxColumn taskID;
        private System.Windows.Forms.DataGridViewTextBoxColumn targetName;
        private System.Windows.Forms.DataGridViewTextBoxColumn taskInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn taskTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn loop;
        private System.Windows.Forms.DataGridViewTextBoxColumn trigger;
        private System.Windows.Forms.DataGridViewTextBoxColumn interval;
        private System.Windows.Forms.DataGridViewTextBoxColumn targetNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn nextTaskTime;
        private System.Windows.Forms.Label txtVersion;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.TabPage pageBuddy;
        private System.Windows.Forms.LinkLabel linkBuddy;
        private System.Windows.Forms.Button btnAddBuddy;
        private System.Windows.Forms.CheckBox chkMobile;
        private System.Windows.Forms.TextBox txtBuddy;
        private System.Windows.Forms.ContextMenuStrip contextMenuBuddy;
        private System.Windows.Forms.ToolStripMenuItem menuItemDelBuddy;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chkAllContact;
	}
}