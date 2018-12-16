using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Text;
using MySql.Data.MySqlClient;
using TXD.CF;

namespace TXD.CF.DB
{
    public class MySqlDBDataAccess : IDataAccess
    {
        #region Property

        /// <summary>
        /// 事务嵌套层数。内部变量，暂不支持提交某层的事务。
        /// </summary>
        private int TranCallCount = 0;

        private MySqlConnection _connection;
        private MySqlTransaction _transaction;
#if DebugSql
        private StringBuilder pv_DebugSql = null;
#endif

        private int PV_CommandTimeout = 30;

        #endregion

        public MySqlDBDataAccess()
        {
            string isJiami = string.Empty;
            string ConnectionStringLocalTransaction = string.Empty;
            if (ConfigurationManager.ConnectionStrings["MainDBConnectionString"] == null) //为了通过ci集成测试。调整部署容错。
            {
                Assembly asembly = Assembly.GetExecutingAssembly();
                string cfgname = asembly.Location;
                System.Configuration.Configuration lvvv = System.Configuration.ConfigurationManager.OpenExeConfiguration(cfgname);
                isJiami = lvvv.AppSettings.Settings["encryptDBP"].Value;
                ConnectionStringLocalTransaction = lvvv.ConnectionStrings.ConnectionStrings["MainDBConnectionString"].ConnectionString;
                //Console.WriteLine("db:asembly " + asembly.FullName);
                //Console.WriteLine("db:asembly.Location " + asembly.Location);
                //Console.WriteLine("db:lvvv.FilePath " + lvvv.FilePath);
            }
            else
            {
                isJiami = ConfigurationManager.AppSettings["encryptDBP"];
                ConnectionStringLocalTransaction = ConfigurationManager.ConnectionStrings["MainDBConnectionString"].ConnectionString;
            }

            bool lv加密 = true;
            MySqlConnectionStringBuilder lvSB = new MySqlConnectionStringBuilder();
            lvSB.ConnectionString = ConnectionStringLocalTransaction;

            lv加密 = Convert.ToBoolean(isJiami);
            if (lv加密)
            {
                lvSB.Password = ConfigurationManager.AppSettings["encryptString"];
                //ILog alog = log4net.LogManager.GetLogger("SysRun");
                //alog.Error(lvSB.Password); //+ex.TargetSite.GetParameters

                lvSB.Password = CryptoHelper.Decrypt_Static(lvSB.Password, HelpTXD.KeyEcr);
                //alog.Error(HelpTXD.KeyEcr); //+ex.TargetSite.GetParameters
            }
            _connection = new MySqlConnection(lvSB.ConnectionString);
            _connection.Open();
        }

        public MySqlDBDataAccess(string conn)
        {
            _connection = new MySqlConnection(conn);
        }

        public void Close()
        {
            if (_transaction == null)
            {
                if (_connection != null)
                {
                    _connection.Close();
                    _connection.Dispose();
                    _connection = null;
                }
            }
            else
            {
                throw new Exception("事务未结束。请处理后关闭连接。");
            }
        }

        public string GetServerName()
        {
            return _connection.DataSource;
        }

        #region IDataAccess 成员

        public int ExecuteNonQuery(string cmdText, List<IDbDataParameter> param_List)
        {
            MySqlCommand oleCmd = new MySqlCommand();
            try
            {
                if (_transaction == null)
                {
                    if (_connection.State == ConnectionState.Closed)
                    {
                        _connection.Open();
                    }
                    oleCmd.Connection = _connection;
                }
                else
                {
                    oleCmd.Transaction = _transaction;
                    oleCmd.Connection = _transaction.Connection;
                }
                oleCmd.CommandTimeout = PV_CommandTimeout;
                oleCmd.CommandText = cmdText;
#if DebugSql
                if (pv_DebugSql != null)
                {
                    pv_DebugSql.AppendLine(cmdText);
                    if (param_List != null)
                    {
                        foreach (IDbDataParameter parm in param_List)
                        {
                            pv_DebugSql.AppendLine(parm.ParameterName + ":" + parm.Value.ToString());
                        }
                    }
                }
#endif
                ParameterValue_Introduce(param_List, oleCmd);
                int val = oleCmd.ExecuteNonQuery();
                ParameterValue_Deliver(oleCmd, param_List);
                oleCmd.Parameters.Clear();
                return val;
            }
            catch (Exception ex)
            {
                oleCmd.Parameters.Clear();
                throw ex.GetBaseException();
            }
        }

        public int ExecuteNonQueryProc(string ProcName, List<IDbDataParameter> param_List)
        {
            MySqlCommand oleCmd = new MySqlCommand();
            try
            {
                if (_transaction == null)
                {
                    if (_connection.State == ConnectionState.Closed)
                    {
                        _connection.Open();
                    }
                    oleCmd.Connection = _connection;
                }
                else
                {
                    oleCmd.Transaction = _transaction;
                    oleCmd.Connection = _transaction.Connection;
                }
                oleCmd.CommandTimeout = PV_CommandTimeout;
                oleCmd.CommandText = ProcName;
                oleCmd.CommandType = CommandType.StoredProcedure;
                ParameterValue_Introduce(param_List, oleCmd);
                int val = oleCmd.ExecuteNonQuery();
                ParameterValue_Deliver(oleCmd, param_List);
                oleCmd.Parameters.Clear();
                return val;
            }
            catch (Exception ex)
            {
                oleCmd.Parameters.Clear();
                throw ex.GetBaseException();
            }
        }

        public IDataReader ExecuteReader(string cmdText, List<IDbDataParameter> param_List)
        {
            //关闭连接多余。
            MySqlCommand oleCmd = new MySqlCommand();
            if (_transaction == null)
            {
                try
                {
                    if (_connection.State == ConnectionState.Closed)
                    {
                        _connection.Open();
                    }
                    oleCmd.Connection = _connection;
                    oleCmd.CommandText = cmdText;
                    oleCmd.CommandTimeout = PV_CommandTimeout;
#if DebugSql
                    if (pv_DebugSql != null)
                    {
                        pv_DebugSql.AppendLine(cmdText);
                        if (param_List != null)
                        {
                            foreach (IDbDataParameter parm in param_List)
                            {
                                pv_DebugSql.AppendLine(parm.ParameterName + ":" + parm.Value.ToString());
                            }
                        }
                    }
#endif
                    ParameterValue_Introduce(param_List, oleCmd);
                    MySqlDataReader rdr = oleCmd.ExecuteReader();
                    ParameterValue_Deliver(oleCmd, param_List);
                    oleCmd.Parameters.Clear();
                    return rdr;
                }
                catch (System.Exception ex)
                {
                    //_connection.Close();
                    throw ex.GetBaseException();
                }
            }
            else
            {
                try
                {
                    oleCmd.Transaction = _transaction;
                    oleCmd.Connection = _transaction.Connection;
                    oleCmd.CommandText = cmdText;
                    oleCmd.CommandTimeout = PV_CommandTimeout;
                    ParameterValue_Introduce(param_List, oleCmd);
                    MySqlDataReader rdr = oleCmd.ExecuteReader();
                    ParameterValue_Deliver(oleCmd, param_List);
                    oleCmd.Parameters.Clear();
                    return rdr;
                }
                catch (System.Exception ex)
                {
                    throw ex.GetBaseException();
                }
            }
        }

        public object ExecuteScalar(string cmdText, List<IDbDataParameter> param_List)
        {
            MySqlCommand oleCmd = new MySqlCommand();
            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                oleCmd.Connection = _connection;
            }
            else
            {
                oleCmd.Transaction = _transaction;
                oleCmd.Connection = _transaction.Connection;
            }
            oleCmd.CommandText = cmdText;
            oleCmd.CommandTimeout = PV_CommandTimeout;
            ParameterValue_Introduce(param_List, oleCmd);
            object val = oleCmd.ExecuteScalar();
            ParameterValue_Deliver(oleCmd, param_List);
            oleCmd.Parameters.Clear();
            return val;
        }

        public object ExecuteScalar(string cmdText)
        {
            MySqlCommand oleCmd = new MySqlCommand();
            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                oleCmd.Connection = _connection;
            }
            else
            {
                oleCmd.Transaction = _transaction;
                oleCmd.Connection = _transaction.Connection;
            }
            oleCmd.CommandText = cmdText;
            object val = oleCmd.ExecuteScalar();
            return val;
        }

        public IDbDataParameter CreatParameter()
        {
            return new MySqlParameter();
        }

        public IDbDataParameter CreatParameter(string name, object paraValue)
        {
            if (paraValue == null)
                paraValue = DBNull.Value;
            MySqlParameter paramter = new MySqlParameter();
            paramter.ParameterName = name;
            paramter.Value = paraValue;
            return paramter;
        }

        public IDbDataParameter CreatParameter(string name, DbType paraType, object paraValue)
        {
            MySqlParameter paramter = new MySqlParameter();
            paramter.ParameterName = name;
            if (paraType == DbType.Object)
            {
                paramter.MySqlDbType = MySqlDbType.Blob;
            }
            else
            {
                paramter.DbType = paraType;
            }
            paramter.Value = paraValue;
            if ((paraType == DbType.String) || (paraType == DbType.AnsiString))
            {
                paramter.Size = paraValue.ToString().Length;
            }
            return paramter;
        }

        public void utlBeginTransaction()
        {
            TranCallCount++;
            if (_transaction != null)
            {
                return;
            }
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            _transaction = _connection.BeginTransaction();
            ;
        }

        public void utlRollback()
        {
            TranCallCount--;
            if (TranCallCount > 0)
            {
                return;
            }
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }

        public void utlCommit()
        {
            TranCallCount--;
            if (TranCallCount > 0)
            {
                return;
            }
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }

        public int ExecuteInsert(string table, List<IDbDataParameter> param_List, out int FAutoIncrease)
        {
            int ret = 0;
            FAutoIncrease = 0; //表示自增长还没获取


            string sql = "insert into " + table + " (";
            for (int i = 0; i < param_List.Count; i++)
            {
                MySqlParameter param = (MySqlParameter) param_List[i];
                param.ParameterName.Substring(1);
                if (i != (param_List.Count - 1))
                    sql += param.ParameterName.Substring(1) + ",";
                else
                    sql += param.ParameterName.Substring(1);
            }
            sql += ")";
            sql += "values (";
            for (int i = 0; i < param_List.Count; i++)
            {
                MySqlParameter param = (MySqlParameter) param_List[i];
                param.ParameterName.Substring(1);
                if (i != (param_List.Count - 1))
                    sql += param.ParameterName + ",";
                else
                    sql += param.ParameterName;
            }
            sql += ")";
            MySqlCommand sql_cmd = new MySqlCommand();

            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                sql_cmd.Connection = _connection;
            }
            else
            {
                sql_cmd.Transaction = _transaction;
                sql_cmd.Connection = _transaction.Connection;
            }
            ParameterValue_Introduce(param_List, sql_cmd);
            sql_cmd.CommandTimeout = PV_CommandTimeout;
            sql_cmd.CommandText = sql;
            try
            {
                ret = sql_cmd.ExecuteNonQuery();
            }
            catch //(Exception ex)
            {
                sql_cmd.Parameters.Clear();
                throw;
            }
            FAutoIncrease = Convert.ToInt32(sql_cmd.LastInsertedId);
            ParameterValue_Deliver(sql_cmd, param_List);
            //sql_cmd.Parameters.Clear();
            //sql = "select  @@IDENTITY as iden from " + table;
            //sql_cmd.CommandText = sql;
            //object lvo = null;
            //lvo = sql_cmd.ExecuteScalar();
            //if (lvo != null && lvo != DBNull.Value)
            //{ FAutoIncrease = Convert.ToInt32(lvo); }
            return ret;
        }

        public int ExecuteInsert(string table, List<IDbDataParameter> param_List)
        {
            int ret = 0;

            string sql = "insert into " + table + " (";
            for (int i = 0; i < param_List.Count; i++)
            {
                MySqlParameter param = (MySqlParameter) param_List[i];
                param.ParameterName.Substring(1);
                if (i != (param_List.Count - 1))
                    sql += param.ParameterName.Substring(1) + ",";
                else
                    sql += param.ParameterName.Substring(1);
            }
            sql += ")";
            sql += "values (";
            for (int i = 0; i < param_List.Count; i++)
            {
                MySqlParameter param = (MySqlParameter) param_List[i];
                param.ParameterName.Substring(1);
                if (i != (param_List.Count - 1))
                    sql += param.ParameterName + ",";
                else
                    sql += param.ParameterName;
            }
            sql += ")";
            MySqlCommand sql_cmd = new MySqlCommand();
            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                sql_cmd.Connection = _connection;
            }
            else
            {
                sql_cmd.Transaction = _transaction;
                sql_cmd.Connection = _transaction.Connection;
            }
            ParameterValue_Introduce(param_List, sql_cmd);
            sql_cmd.CommandTimeout = PV_CommandTimeout;
            sql_cmd.CommandText = sql;
            try
            {
                ret = sql_cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                sql_cmd.Parameters.Clear();
                throw ex.GetBaseException();
            }
            ParameterValue_Deliver(sql_cmd, param_List);
            sql_cmd.Parameters.Clear();
            return ret;
        }

        public int ExecuteUpdate(string table, List<IDbDataParameter> param_List, int p条件参数个)
        {
            return ExecuteUpdate(table, param_List, p条件参数个, null);
        }

        public int ExecuteUpdate(string table, List<IDbDataParameter> param_List, int p条件参数个, string pwhere语句_其他部分)
        {
            int ret = 0;
            string sql = "update " + table + " set ";

            MySqlParameter param = null;


            for (int i = 0; i < param_List.Count - p条件参数个; i++)
            {
                param = (MySqlParameter) param_List[i];
                param.ParameterName = param.ParameterName.Replace("@", "?");
                if (i != 0)
                    sql += "," + param.ParameterName.Substring(1) + "=" + param.ParameterName;
                else
                    sql += param.ParameterName.Substring(1) + "=" + param.ParameterName;
            }
            string str_where = "";
            for (int i = param_List.Count - p条件参数个; i < param_List.Count; i++)
            {
                param = (MySqlParameter) param_List[i];
                param.ParameterName = param.ParameterName.Replace("@", "?");
                if (i != param_List.Count - p条件参数个)
                    str_where += " and " + param.ParameterName.Substring(1) + "=" + param.ParameterName;
                else
                    str_where += param.ParameterName.Substring(1) + "=" + param.ParameterName;
            }
            sql += " where " + str_where;
            if (pwhere语句_其他部分 != null)
                sql += " " + pwhere语句_其他部分;
            MySqlCommand sql_cmd = new MySqlCommand();
            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                sql_cmd.Connection = _connection;
            }
            else
            {
                sql_cmd.Transaction = _transaction;
                sql_cmd.Connection = _transaction.Connection;
            }
            ParameterValue_Introduce(param_List, sql_cmd);
            sql_cmd.CommandTimeout = PV_CommandTimeout;
            sql_cmd.CommandText = sql;
            try
            {
                ret = sql_cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                sql_cmd.Parameters.Clear();
                throw ex.GetBaseException();
            }
            ParameterValue_Deliver(sql_cmd, param_List);
            sql_cmd.Parameters.Clear();
            return ret;
        }

        public DataTable ExecuteDataTable(string cmdText)
        {
            return ExecuteDataTable(cmdText, null);
        }

        public DataTable ExecuteDataTable(string cmdText, List<IDbDataParameter> param_List)
        {
            DataTable DT = null;
            MySqlCommand sql_cmd = new MySqlCommand();

            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                sql_cmd.Connection = _connection;
            }
            else
            {
                sql_cmd.Transaction = _transaction;
                sql_cmd.Connection = _transaction.Connection;
            }
            ParameterValue_Introduce(param_List, sql_cmd);
            sql_cmd.CommandTimeout = PV_CommandTimeout;
            sql_cmd.CommandText = cmdText;
            MySqlDataAdapter sqladapter = new MySqlDataAdapter(sql_cmd);
            DT = new DataTable();
            try
            {
                sqladapter.Fill(DT);
            }
            catch
            {
                sql_cmd.Parameters.Clear();
                throw;
            }
            ParameterValue_Deliver(sql_cmd, param_List);
            sql_cmd.Parameters.Clear();
            return DT;
        }

        public DataTable ExecuteDataTableProc(string proc_name, List<IDbDataParameter> param_List)
        {
            DataTable DT = new DataTable();
            MySqlCommand sql_cmd = new MySqlCommand();


            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
                sql_cmd.Connection = _connection;
            }
            else
            {
                sql_cmd.Transaction = _transaction;
                sql_cmd.Connection = _transaction.Connection;
            }
            sql_cmd.CommandType = CommandType.StoredProcedure;
            ParameterValue_Introduce(param_List, sql_cmd);
            sql_cmd.CommandTimeout = PV_CommandTimeout;
            sql_cmd.CommandText = proc_name;

            // Define the data adapter and fill the dataset
            try
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(sql_cmd))
                {
                    da.Fill(DT);
                }
            }
            catch
            {
                sql_cmd.Parameters.Clear();
                throw;
            }
            ParameterValue_Deliver(sql_cmd, param_List);
            sql_cmd.Parameters.Clear();
            return DT;
        }

        public DateTime GetServerTime()
        {
            MySqlCommand oleCmd = new MySqlCommand();
            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                oleCmd.Connection = _connection;
            }
            else
            {
                oleCmd.Transaction = _transaction;
                oleCmd.Connection = _transaction.Connection;
            }
            oleCmd.CommandText = "select now()";

            object val = oleCmd.ExecuteScalar();
            return Convert.ToDateTime(val);
        }

        #endregion


        /// <summary>
        /// 将参数引入到command里。保证了两边顺序是一致的。
        /// </summary>
        /// <param name="sourceParams">传入参数清单</param>
        /// <param name="SqlCmd">目的地：CMD对象</param>
        public static void ParameterValue_Introduce(List<IDbDataParameter> sourceParams, MySqlCommand SqlCmd)
        {
            if (sourceParams == null)
            {
                return;
            }
            MySqlParameter parm = null;
            foreach (IDbDataParameter Cp in sourceParams)
            {
                parm = new MySqlParameter();
                parm.ParameterName = Cp.ParameterName; //.Replace("@","?");
                parm.DbType = Cp.DbType;
                parm.Size = Cp.Size;
                parm.Value = Cp.Value;
                parm.Precision = Cp.Precision;
                parm.Scale = Cp.Scale;
                parm.Direction = Cp.Direction;
                SqlCmd.Parameters.Add(parm);
            }
        }

        /// <summary>
        /// SQL传出参数的赋值。在保证参数转换时，顺序一致的情况下。调用。
        /// </summary>
        /// <param name="SqlCmd">来源：CMD对象</param>
        /// <param name="destParams">传出：参数目的地</param>
        public static void ParameterValue_Deliver(MySqlCommand SqlCmd, List<IDbDataParameter> destParams)
        {
            if ((destParams == null) || (SqlCmd.Parameters == null))
            {
                return;
            }
            for (int lvi = 0; lvi < destParams.Count; lvi++)
            {
                if (destParams[lvi].Direction != ParameterDirection.Input)
                {
                    destParams[lvi].Value = SqlCmd.Parameters[lvi].Value;
                }
            }
        }

        private static void PrepareCommand(MySqlTransaction trans, MySqlConnection conn, MySqlCommand oleCmd, CommandType cmdType, string cmdText, List<IDbDataParameter> cmdParms)
        {
            oleCmd.Connection = conn;
            oleCmd.CommandText = cmdText;
            if (trans != null)
            {
                oleCmd.Transaction = trans;
                oleCmd.Connection = trans.Connection;
            }
            oleCmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                ParameterValue_Introduce(cmdParms, oleCmd);
            }
        }

        public void SetCmdTimeOut(int interal)
        {
            PV_CommandTimeout = interal;
        }
#if DebugSql
        public void DebugOn()
        {
            pv_DebugSql = new StringBuilder();
        }

        public string DebugOff()

        {
            string lvre = "";
            if (pv_DebugSql != null)
            {
                lvre = pv_DebugSql.ToString();
                pv_DebugSql = null;
            }
            return lvre;
        }
#endif
    }
}