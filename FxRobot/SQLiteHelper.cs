using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Imps.Client.Data;
using Haozes.Robot.Utils;
namespace Haozes.Robot
{

    public class SQLiteHelper : IDisposable
    {
        private SQLiteConnection _connection;
        private string _dataSource = string.Empty;
        private static SQLiteHelper _instance = new SQLiteHelper();
        private bool _isFirstUse=true;
        private Dictionary<int, SQLiteTransaction> _localTransactionCollection;
        private static object _locker = new object();
        private string _password = string.Empty;
        private static int _refCount = 0;
        private const string CONNECTION_STRING_FORMAT = "Data Source={0};Password={1}";
        private const string DATABASE_NAME = "HaozesFx.dat";

        private SQLiteHelper()
        {

        }

        public void CommitTransaction()
        {
            lock (_locker)
            {
                int managedThreadId = Thread.CurrentThread.ManagedThreadId;
                if (this.LocalTransactionCollection.ContainsKey(managedThreadId))
                {
                    this.LocalTransactionCollection[managedThreadId].Commit();
                    _refCount--;
                    this.LocalTransactionCollection.Remove(managedThreadId);
                    if (_refCount == 0)
                    {
                        this._connection.Close();
                    }
                }
            }
        }

        public SQLiteCommand CreateCommand(string sql, params object[] parameters)
        {
            SQLiteCommand command = null;
            int managedThreadId = Thread.CurrentThread.ManagedThreadId;
            if (this.LocalTransactionCollection.ContainsKey(managedThreadId) && (this.LocalTransactionCollection[managedThreadId] != null))
            {
                command = new SQLiteCommand(sql, this._connection, this.LocalTransactionCollection[managedThreadId]);
            }
            else
            {
                command = new SQLiteCommand(sql, this._connection);
            }
            if (parameters != null)
            {
                foreach (SQLiteParameter parameter in this.DeriveParameters(sql, parameters))
                {
                    command.Parameters.Add(parameter);
                }
            }
            return command;
        }

        public List<SQLiteParameter> DeriveParameters(string commandText, object[] paramList)
        {
            if (paramList == null)
            {
                return null;
            }
            List<SQLiteParameter> list = new List<SQLiteParameter>();
            string input = commandText.Substring(commandText.IndexOf("@")).Replace(",", " ,").Replace(")", " )");
            string pattern = @"(@)\S*(.*?)\b";
            MatchCollection matchs = new Regex(pattern, RegexOptions.IgnoreCase).Matches(input);
            List<string> list2 = new List<string>();
            foreach (Match match in matchs)
            {
                if (!list2.Contains(match.Value))
                {
                    list2.Add(match.Value);
                }
            }
            string[] strArray = list2.ToArray();
            int index = 0;
            Type type = null;
            foreach (object obj2 in paramList)
            {
                if (obj2 == null)
                {
                    SQLiteParameter item = new SQLiteParameter();
                    item.DbType = DbType.Object;
                    item.ParameterName = strArray[index];
                    item.Value = DBNull.Value;
                    list.Add(item);
                }
                else
                {
                    type = obj2.GetType();
                    SQLiteParameter parameter2 = new SQLiteParameter();
                    switch (type.ToString())
                    {
                        case "System.String":
                            parameter2.DbType = DbType.String;
                            parameter2.ParameterName = strArray[index];
                            parameter2.Value = (string)paramList[index];
                            list.Add(parameter2);
                            goto Label_0408;

                        case "System.Byte[]":
                            parameter2.DbType = DbType.Binary;
                            parameter2.ParameterName = strArray[index];
                            parameter2.Value = (byte[])paramList[index];
                            list.Add(parameter2);
                            goto Label_0408;

                        case "System.Int64":
                            parameter2.DbType = DbType.Int64;
                            parameter2.ParameterName = strArray[index];
                            parameter2.Value = (long)paramList[index];
                            list.Add(parameter2);
                            goto Label_0408;

                        case "System.Int32":
                            parameter2.DbType = DbType.Int32;
                            parameter2.ParameterName = strArray[index];
                            parameter2.Value = (int)paramList[index];
                            list.Add(parameter2);
                            goto Label_0408;

                        case "System.Boolean":
                            parameter2.DbType = DbType.Boolean;
                            parameter2.ParameterName = strArray[index];
                            parameter2.Value = (bool)paramList[index];
                            list.Add(parameter2);
                            goto Label_0408;

                        case "System.DateTime":
                            parameter2.DbType = DbType.DateTime;
                            parameter2.ParameterName = strArray[index];
                            parameter2.Value = Convert.ToDateTime(paramList[index]);
                            list.Add(parameter2);
                            goto Label_0408;

                        case "System.Double":
                            parameter2.DbType = DbType.Double;
                            parameter2.ParameterName = strArray[index];
                            parameter2.Value = Convert.ToDouble(paramList[index]);
                            list.Add(parameter2);
                            goto Label_0408;

                        case "System.Decimal":
                            parameter2.DbType = DbType.Decimal;
                            parameter2.ParameterName = strArray[index];
                            parameter2.Value = Convert.ToDecimal(paramList[index]);
                            goto Label_0408;

                        case "System.Guid":
                            parameter2.DbType = DbType.Guid;
                            parameter2.ParameterName = strArray[index];
                            parameter2.Value = (Guid)paramList[index];
                            goto Label_0408;

                        case "System.Object":
                            parameter2.DbType = DbType.Object;
                            parameter2.ParameterName = strArray[index];
                            parameter2.Value = paramList[index];
                            list.Add(parameter2);
                            goto Label_0408;
                    }
                    throw new SystemException("Value is of unknown data type");
                }
            Label_0408:
                index++;
            }
            return list;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposed)
        {
            try
            {
                if (disposed)
                {
                    if (this._localTransactionCollection != null)
                    {
                        lock (_locker)
                        {
                            foreach (SQLiteTransaction transaction in this._localTransactionCollection.Values)
                            {
                                try
                                {
                                    transaction.Rollback();
                                    transaction.Dispose();
                                    continue;
                                }
                                catch
                                {
                                    continue;
                                }
                            }
                            this._localTransactionCollection.Clear();
                            this._localTransactionCollection = null;
                        }
                    }
                    if (this._connection != null)
                    {
                        this._connection.Close();
                        this._connection.Dispose();
                    }
                }
            }
            catch
            {
            }
            finally
            {
                this._connection = null;
            }
        }

        public void EnableConnection()
        {
            if (this._connection == null)
            {
                string connectionString = string.Format("Data Source={0};Password={1}", this._dataSource, this._password);
                this._connection = new SQLiteConnection(connectionString);
            }
            if (this._connection.State == ConnectionState.Closed)
            {
                this._connection.Open();
            }
        }

        public int ExecuteNonQuery(string sql, params object[] parameters)
        {
            this.EnableConnection();
            return this.CreateCommand(sql, parameters).ExecuteNonQuery();
        }

        public SQLiteDataReader ExecuteReader(string sql, params object[] parameters)
        {
            this.EnableConnection();
            return this.CreateCommand(sql, parameters).ExecuteReader();
        }

        public int ExecuteScalar(string sql, params object[] parameters)
        {
            this.EnableConnection();
            object obj2 = this.CreateCommand(sql, parameters).ExecuteScalar();
            if (obj2 != null)
            {
                return int.Parse(obj2.ToString());
            }
            return 0;
        }

        ~SQLiteHelper()
        {
            this.Dispose(false);
        }

        public bool InitializeDatabase(string currentUserSid)
        {
            bool flag;
            lock (_locker)
            {
                if (!this.Disposed)
                {
                    this.Dispose();
                }
                //string app = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                //app = Path.Combine(app, "Fetion");
                //string path = Path.Combine(app, currentUserSid);
                string path = Path.Combine(CommonUtil.AppFolder, currentUserSid);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                this._dataSource = Path.Combine(path, DATABASE_NAME);
                this._password = currentUserSid;
                this._localTransactionCollection = new Dictionary<int, SQLiteTransaction>();
                try
                {
                    if (!File.Exists(this._dataSource))
                    {
                        SQLiteConnection.CreateFile(this._dataSource);
                        string connectionString = string.Format("Data Source={0};Password={1}", this._dataSource, this._password);
                        this._connection = new SQLiteConnection(connectionString);
                        this._connection.SetPassword(this._password);
                    }
                    flag = true;
                }
                catch
                {
                    this.Dispose();
                    File.Delete(this._dataSource);
                    flag = false;
                }
            }
            return flag;
        }

        public void JoinTransaction()
        {
            lock (_locker)
            {
                this.EnableConnection();
                _refCount++;
                int managedThreadId = Thread.CurrentThread.ManagedThreadId;
                if (!this.LocalTransactionCollection.ContainsKey(managedThreadId))
                {
                    this.LocalTransactionCollection.Add(managedThreadId, this._connection.BeginTransaction());
                }
            }
        }

        public void RollbackTransaction()
        {
            lock (_locker)
            {
                int managedThreadId = Thread.CurrentThread.ManagedThreadId;
                if (this.LocalTransactionCollection.ContainsKey(managedThreadId))
                {
                    this.LocalTransactionCollection[managedThreadId].Rollback();
                    _refCount--;
                    this.LocalTransactionCollection.Remove(managedThreadId);
                    if (_refCount == 0)
                    {
                        this._connection.Close();
                    }
                }
            }
        }

        public bool Disposed
        {
            get
            {
                return (this._connection != null);
            }
        }

        public static SQLiteHelper Instance
        {
            get
            {
                return _instance;
            }
        }

        public bool IsFirstUse
        {
            get
            {
                return this._isFirstUse;
            }
        }

        private Dictionary<int, SQLiteTransaction> LocalTransactionCollection
        {
            get
            {
                lock (_locker)
                {
                    if (this._localTransactionCollection == null)
                    {
                        this._localTransactionCollection = new Dictionary<int, SQLiteTransaction>();
                    }
                    return this._localTransactionCollection;
                }
            }
        }

        public List<string> Objects
        {
            get
            {
                lock (_locker)
                {
                    List<string> list = new List<string>();
                    using (SQLiteDataReader reader = this.ExecuteReader("SELECT [Name] FROM [SQLITE_MASTER] WHERE ([type] = 'table') OR ([type] = 'view')", null))
                    {
                        while (reader.Read())
                        {
                            list.Add(reader["name"].ToString());
                        }
                    }
                    return list;
                }
            }
        }
    }
}

