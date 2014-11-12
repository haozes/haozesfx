using System;
using System.Collections.Generic;
using System.Text;
using Imps.Client.Data;
using System.Windows.Forms;
using Haozes.FxClient.CommUtil;
using Haozes.FxClient.Core;
using Haozes.Robot.Utils;
namespace Haozes.Robot
{
    public class RobotPermissionMgr
    {
        private User currentUser;
        private SQLiteHelper dbHelper;
        private IList<Contact> permitContactList = new List<Contact>();
        public RobotPermissionMgr(User currentUser)
        {
            this.currentUser = currentUser;
            dbHelper = SQLiteHelper.Instance;
            InitializeDataBase();
        }
        private void InitializeDataBase()
        {
            List<string> tableList = this.dbHelper.Objects;
            StringBuilder builder;
            if (!tableList.Contains("Permit"))
            {
                builder = new StringBuilder();
                builder.AppendLine("CREATE TABLE Permit(");
                builder.AppendLine("Uri VARCHAR(256) PRIMARY KEY,");
                builder.AppendLine("UserName VARCHAR(256),");
                builder.AppendLine("Sid VARCHAR(256),");
                builder.AppendLine("Remark VARCHAR(256)");
                builder.AppendLine(")");
                builder.AppendLine("");
                try
                {
                    this.dbHelper.ExecuteNonQuery(builder.ToString(), null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    RobotCore.Log.Error(ex.ToString());
                }
            }
            Refresh();
        }
        public IList<Contact> PermitContactList
        {
            get
            {
                return permitContactList;
            }
        }
        public void Add(Contact c)
        {
            RobotCore.DBHelper.ExecuteNonQuery("INSERT INTO [Permit] ([Uri], [UserName],[Sid]) VALUES (@Uri, @UserName, @Sid)", new object[] { c.Uri.Raw, c.DisplayName, c.Uri.Sid });
            this.Refresh();
        }
        public void Delete(Contact c)
        {
            RobotCore.DBHelper.ExecuteNonQuery("DELETE FROM Permit WHERE Uri=@Uri", new object[] { c.Uri.Raw });
            this.Refresh();
        }
        private void Refresh()
        {
            SQLiteDataReader reader = RobotCore.DBHelper.ExecuteReader("SELECT * FROM Permit", null);
            permitContactList = CommonUtil.ToContactList(reader);
           
        }
        public bool IsPermitUser(string uri)
        {
            bool isPermit = false;
            if (uri == RobotCore.Host.Uri.Raw)
                return true;
            foreach (Contact c in permitContactList)
            {
                if (c.Uri.Raw == uri)
                {
                    isPermit = true;
                    break;
                }
            }
            return isPermit;
        }
    }
}
