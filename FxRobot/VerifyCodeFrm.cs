using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Haozes.Robot
{
    public partial class VerifyCodeFrm : Form
    {
        public VerifyCodeFrm()
        {
            InitializeComponent();
        }

        public VerifyCodeFrm(string reason, byte[] picCode)
        {
            InitializeComponent();
            this.lblTip.Text = reason;

            using (MemoryStream stream = new MemoryStream(picCode, true))
            {
                Bitmap bmp = new Bitmap(stream);
                this.pictureBox1.Image = bmp;
            }
        }

        public string VerfyCode
        {
            get;
            set;
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            this.VerfyCode = this.txtPicNum.Text.Trim();
            this.DialogResult = DialogResult.OK;
        }
    }
}
