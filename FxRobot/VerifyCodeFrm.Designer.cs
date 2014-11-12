namespace Haozes.Robot
{
    partial class VerifyCodeFrm
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
            this.btnSure = new System.Windows.Forms.Button();
            this.lblTip = new System.Windows.Forms.Label();
            this.txtPicNum = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.linkRetrive = new System.Windows.Forms.LinkLabel();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSure
            // 
            this.btnSure.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSure.Location = new System.Drawing.Point(69, 133);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(75, 23);
            this.btnSure.TabIndex = 0;
            this.btnSure.Text = "确定";
            this.btnSure.UseVisualStyleBackColor = true;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // lblTip
            // 
            this.lblTip.AutoSize = true;
            this.lblTip.Location = new System.Drawing.Point(11, 9);
            this.lblTip.Name = "lblTip";
            this.lblTip.Size = new System.Drawing.Size(55, 13);
            this.lblTip.TabIndex = 1;
            this.lblTip.Text = "提示信息";
            // 
            // txtPicNum
            // 
            this.txtPicNum.Location = new System.Drawing.Point(69, 49);
            this.txtPicNum.Name = "txtPicNum";
            this.txtPicNum.Size = new System.Drawing.Size(211, 20);
            this.txtPicNum.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(69, 85);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(143, 30);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // linkRetrive
            // 
            this.linkRetrive.AutoSize = true;
            this.linkRetrive.Location = new System.Drawing.Point(234, 98);
            this.linkRetrive.Name = "linkRetrive";
            this.linkRetrive.Size = new System.Drawing.Size(91, 13);
            this.linkRetrive.TabIndex = 4;
            this.linkRetrive.TabStop = true;
            this.linkRetrive.Text = "重新获取验证码";
            this.linkRetrive.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(214, 133);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // VerifyCodeFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 188);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.linkRetrive);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtPicNum);
            this.Controls.Add(this.lblTip);
            this.Controls.Add(this.btnSure);
            this.Name = "VerifyCodeFrm";
            this.Text = "VerifyCodeFrm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSure;
        private System.Windows.Forms.Label lblTip;
        private System.Windows.Forms.TextBox txtPicNum;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel linkRetrive;
        private System.Windows.Forms.Button btnCancel;
    }
}