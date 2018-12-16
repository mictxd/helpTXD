using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using Oracle.ManagedDataAccess.Client;
using TXD.CF;
//not all
namespace TXD.CF.DB
{
    public class OracleDBDataAccess : IDataAccess
    {
        #region Property

        /// <summary>
        /// 参数前缀，这样不用改上层代码。习惯用@的继续用。
        /// </summary>
        private string pv_PF = ":";

        private OracleConnection _connection;
        private OracleTransaction _transaction;
        private int PV_CommandTimeout = 30;

        #endregion

        public OracleDBDataAccess()
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
            OracleConnectionStringBuilder lvSB = new OracleConnectionStringBuilder();
            lvSB.ConnectionString = ConnectionStringLocalTransaction;

            lv加密 = Convert.ToBoolean(isJiami);
            if (lv加密)
            {
                lvSB.Password = ConfigurationManager.AppSettings["encryptString"];
                lvSB.Password = CryptoHelper.Decrypt_Static(lvSB.Password, HelpTXD.KeyEcr);
            }
            _connection = new OracleConnection(lvSB.ConnectionString);
            _connection.Open();
        }

        public OracleDBDataAccess(string conn)
        {
            _connection = new OracleConnection(conn);
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
            OracleCommand Cmd = new OracleCommand();
            try
            {
                if (_transaction == null)
                {
                    if (_connection.State == ConnectionState.Closed)
                    {
                        _connection.Open();
                    }
                    Cmd.Connection = _connection;
                }
                else
                {
                    Cmd.Transaction = _transaction;
                    Cmd.Connection = _transaction.Connection;
                }
                Cmd.CommandTimeout = PV_CommandTimeout;
                Cmd.CommandText = cmdText;

                ParameterValue_Introduce(param_List, Cmd);
                PT_改变sql脚本的参数前缀(Cmd);
                int val = Cmd.ExecuteNonQuery();
                ParameterValue_Deliver(Cmd, param_List);
                Cmd.Parameters.Clear();
                return val;
            }
            catch (Exception ex)
            {
                Cmd.Parameters.Clear();
                throw ex.GetBaseException();
            }
        }

        public int ExecuteNonQueryProc(string ProcName, List<IDbDataParameter> param_List)
        {
            OracleCommand Cmd = new OracleCommand();
            try
            {
                if (_transaction == null)
                {
                    if (_connection.State == ConnectionState.Closed)
                    {
                        _connection.Open();
                    }
                    Cmd.Connection = _connection;
                }
                else
                {
                    Cmd.Transaction = _transaction;
                    Cmd.Connection = _transaction.Connection;
                }
                Cmd.CommandTimeout = PV_CommandTimeout;
                Cmd.CommandText = ProcName;
                Cmd.CommandType = CommandType.StoredProcedure;
                ParameterValue_Introduce(param_List, Cmd);
                PT_改变sql脚本的参数前缀(Cmd);
                int val = Cmd.ExecuteNonQuery();
                ParameterValue_Deliver(Cmd, param_List);
                Cmd.Parameters.Clear();
                return val;
            }
            catch (Exception ex)
            {
                Cmd.Parameters.Clear();
                throw ex.GetBaseException();
            }
        }

        public IDataReader ExecuteReader(string cmdText, List<IDbDataParameter> param_List)
        {
            //关闭连接多余。
            OracleCommand Cmd = new OracleCommand();
            if (_transaction == null)
            {
                try
                {
                    if (_connection.State == ConnectionState.Closed)
                    {
                        _connection.Open();
                    }
                    Cmd.Connection = _connection;
                    Cmd.CommandText = cmdText;
                    Cmd.CommandTimeout = PV_CommandTimeout;
                    ParameterValue_Introduce(param_List, Cmd);
                    PT_改变sql脚本的参数前缀(Cmd);
                    OracleDataReader rdr = Cmd.ExecuteReader();
                    ParameterValue_Deliver(Cmd, param_List);
                    Cmd.Parameters.Clear();
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
                    Cmd.Transaction = _transaction;
                    Cmd.Connection = _transaction.Connection;
                    Cmd.CommandText = cmdText;
                    Cmd.CommandTimeout = PV_CommandTimeout;
                    ParameterValue_Introduce(param_List, Cmd);
                    PT_改变sql脚本的参数前缀(Cmd);
                    OracleDataReader rdr = Cmd.ExecuteReader();
                    ParameterValue_Deliver(Cmd, param_List);
                    Cmd.Parameters.Clear();
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
            OracleCommand Cmd = new OracleCommand();
            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                Cmd.Connection = _connection;
            }
            else
            {
                Cmd.Transaction = _transaction;
                Cmd.Connection = _transaction.Connection;
            }
            Cmd.CommandText = cmdText;
            Cmd.CommandTimeout = PV_CommandTimeout;
            ParameterValue_Introduce(param_List, Cmd);
            PT_改变sql脚本的参数前缀(Cmd);
            object val = Cmd.ExecuteScalar();
            ParameterValue_Deliver(Cmd, param_List);
            Cmd.Parameters.Clear();
            return val;
        }

        public object ExecuteScalar(string cmdText)
        {
            OracleCommand Cmd = new OracleCommand();
            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                Cmd.Connection = _connection;
            }
            else
            {
                Cmd.Transaction = _transaction;
                Cmd.Connection = _transaction.Connection;
            }
            Cmd.CommandText = cmdText;
            object val = Cmd.ExecuteScalar();
            return val;
        }

        /// <summary>
        /// 尽量不用。参数前缀不一样。
        /// </summary>
        /// <returns></returns>
        public IDbDataParameter CreatParameter()
        {
            return new OracleParameter();
        }

        public IDbDataParameter CreatParameter(string name, object paraValue)
        {
            if (paraValue == null)
                paraValue = DBNull.Value;
            OracleParameter paramter = new OracleParameter();
            paramter.ParameterName = name.Replace("@", pv_PF);
            paramter.Value = paraValue;
            return paramter;
        }

        public IDbDataParameter CreatParameter(string name, DbType paraType, object paraValue)
        {
            OracleParameter paramter = new OracleParameter();
            paramter.ParameterName = name.Replace("@", pv_PF);
            if (paraType == DbType.Object)
            {
                paramter.OracleDbType = OracleDbType.Blob;
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
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }

        public void utlCommit()
        {
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
                OracleParameter param = (OracleParameter) param_List[i];
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
                OracleParameter param = (OracleParameter) param_List[i];
                param.ParameterName.Substring(1);
                if (i != (param_List.Count - 1))
                    sql += param.ParameterName + ",";
                else
                    sql += param.ParameterName;
            }
            sql += ")";
            OracleCommand Cmd = new OracleCommand();

            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                Cmd.Connection = _connection;
            }
            else
            {
                Cmd.Transaction = _transaction;
                Cmd.Connection = _transaction.Connection;
            }
            ParameterValue_Introduce(param_List, Cmd);
            Cmd.CommandTimeout = PV_CommandTimeout;
            Cmd.CommandText = sql;
            try
            {
                PT_改变sql脚本的参数前缀(Cmd);
                ret = Cmd.ExecuteNonQuery();
                FAutoIncrease = -1;
                ParameterValue_Deliver(Cmd, param_List);
            }
            catch //(Exception ex)
            {
                Cmd.Parameters.Clear();
                throw;
            }
            /*
            try
            {
                //需要 seq的名字。
                Cmd.CommandText = "select seq.currval  from " + table;
                FAutoIncrease = Convert.ToInt32(Cmd.ExecuteScalar());
            }
            catch //(Exception ex)
            {
                Cmd.Parameters.Clear();
                throw;
            }
            */
            //throw new NotImplementedException("自增主键问题。");
            ///
            //FAutoIncrease = Convert.ToInt32(Cmd.LastInsertedId);
            // FAutoIncrease = Convert.ToInt32(Cmd.LastInsertedId);

            //Cmd.Parameters.Clear();
            //sql = "select  @@IDENTITY as iden from " + table;
            //Cmd.CommandText = sql;
            //object lvo = null;
            //lvo = Cmd.ExecuteScalar();
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
                OracleParameter param = (OracleParameter) param_List[i];
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
                OracleParameter param = (OracleParameter) param_List[i];
                param.ParameterName.Substring(1);
                if (i != (param_List.Count - 1))
                    sql += param.ParameterName + ",";
                else
                    sql += param.ParameterName;
            }
            sql += ")";
            OracleCommand Cmd = new OracleCommand();
            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                Cmd.Connection = _connection;
            }
            else
            {
                Cmd.Transaction = _transaction;
                Cmd.Connection = _transaction.Connection;
            }
            ParameterValue_Introduce(param_List, Cmd);
            Cmd.CommandTimeout = PV_CommandTimeout;
            Cmd.CommandText = sql;
            try
            {
                PT_改变sql脚本的参数前缀(Cmd);
                ret = Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Cmd.Parameters.Clear();
                throw ex.GetBaseException();
            }
            ParameterValue_Deliver(Cmd, param_List);
            Cmd.Parameters.Clear();
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

            OracleParameter param = null;


            for (int i = 0; i < param_List.Count - p条件参数个; i++)
            {
                param = (OracleParameter) param_List[i];
                param.ParameterName = param.ParameterName.Replace("@", "?");
                if (i != 0)
                    sql += "," + param.ParameterName.Substring(1) + "=" + param.ParameterName;
                else
                    sql += param.ParameterName.Substring(1) + "=" + param.ParameterName;
            }
            string str_where = "";
            for (int i = param_List.Count - p条件参数个; i < param_List.Count; i++)
            {
                param = (OracleParameter) param_List[i];
                param.ParameterName = param.ParameterName.Replace("@", "?");
                if (i != param_List.Count - p条件参数个)
                    str_where += " and " + param.ParameterName.Substring(1) + "=" + param.ParameterName;
                else
                    str_where += param.ParameterName.Substring(1) + "=" + param.ParameterName;
            }
            sql += " where " + str_where;
            if (pwhere语句_其他部分 != null)
                sql += " " + pwhere语句_其他部分;
            OracleCommand Cmd = new OracleCommand();
            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                Cmd.Connection = _connection;
            }
            else
            {
                Cmd.Transaction = _transaction;
                Cmd.Connection = _transaction.Connection;
            }
            ParameterValue_Introduce(param_List, Cmd);
            Cmd.CommandTimeout = PV_CommandTimeout;
            Cmd.CommandText = sql;
            try
            {
                PT_改变sql脚本的参数前缀(Cmd);
                ret = Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Cmd.Parameters.Clear();
                throw ex.GetBaseException();
            }
            ParameterValue_Deliver(Cmd, param_List);
            Cmd.Parameters.Clear();
            return ret;
        }

        public DataTable ExecuteDataTable(string cmdText)
        {
            return ExecuteDataTable(cmdText, null);
        }

        public DataTable ExecuteDataTable(string cmdText, List<IDbDataParameter> param_List)
        {
            DataTable DT = null;
            OracleCommand Cmd = new OracleCommand();

            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                Cmd.Connection = _connection;
            }
            else
            {
                Cmd.Transaction = _transaction;
                Cmd.Connection = _transaction.Connection;
            }
            ParameterValue_Introduce(param_List, Cmd);
            Cmd.CommandTimeout = PV_CommandTimeout;
            Cmd.CommandText = cmdText;
            OracleDataAdapter sqladapter = new OracleDataAdapter(Cmd);
            DT = new DataTable();
            try
            {
                PT_改变sql脚本的参数前缀(Cmd);
                sqladapter.Fill(DT);
            }
            catch
            {
                Cmd.Parameters.Clear();
                throw;
            }
            ParameterValue_Deliver(Cmd, param_List);
            Cmd.Parameters.Clear();
            return DT;
        }

        public DataTable ExecuteDataTableProc(string proc_name, List<IDbDataParameter> param_List)
        {
            DataTable DT = new DataTable();
            OracleCommand Cmd = new OracleCommand();


            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
                Cmd.Connection = _connection;
            }
            else
            {
                Cmd.Transaction = _transaction;
                Cmd.Connection = _transaction.Connection;
            }
            Cmd.CommandType = CommandType.StoredProcedure;
            PT_改变sql脚本的参数前缀(Cmd);
            ParameterValue_Introduce(param_List, Cmd);
            Cmd.CommandTimeout = PV_CommandTimeout;
            Cmd.CommandText = proc_name;

            // Define the data adapter and fill the dataset
            try
            {
                PT_改变sql脚本的参数前缀(Cmd);
                using (OracleDataAdapter da = new OracleDataAdapter(Cmd))
                {
                    da.Fill(DT);
                }
            }
            catch
            {
                Cmd.Parameters.Clear();
                throw;
            }
            ParameterValue_Deliver(Cmd, param_List);
            Cmd.Parameters.Clear();
            return DT;
        }

        public DateTime GetServerTime()
        {
            OracleCommand Cmd = new OracleCommand();
            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                Cmd.Connection = _connection;
            }
            else
            {
                Cmd.Transaction = _transaction;
                Cmd.Connection = _transaction.Connection;
            }
            Cmd.CommandText = "select now()";

            object val = Cmd.ExecuteScalar();
            return Convert.ToDateTime(val);
        }

        #endregion

        public void PT_改变sql脚本的参数前缀(OracleCommand Cmd)
        {
            foreach (OracleParameter lvop in Cmd.Parameters)
            {
                Cmd.CommandText.Replace("@" + lvop.ParameterName.Substring(1), ":" + lvop.ParameterName.Substring(1));
            }
        }


        /// <summary>
        /// 将参数引入到command里。保证了两边顺序是一致的。
        /// </summary>
        /// <param name="sourceParams">传入参数清单</param>
        /// <param name="SqlCmd">目的地：CMD对象</param>
        public static void ParameterValue_Introduce(List<IDbDataParameter> sourceParams, OracleCommand SqlCmd)
        {
            if (sourceParams == null)
            {
                return;
            }
            OracleParameter parm = null;
            foreach (IDbDataParameter Cp in sourceParams)
            {
                parm = Cp as OracleParameter;
                if (parm == null)
                {
                    throw new Exception("必须传递原始参数表。不能转序列传送。");
                }
                if (parm.Value.ToString().Length > 4000)
                {
                    parm.OracleDbType = OracleDbType.Clob;
                }
                //parm.ParameterName = Cp.ParameterName; //.Replace("@","?");
                //parm.DbType = Cp.DbType;
                //parm.Size = Cp.Size;
                //parm.Value = Cp.Value;
                //parm.Precision = Cp.Precision;
                //parm.Scale = Cp.Scale;
                //parm.Direction = Cp.Direction;
                SqlCmd.Parameters.Add(parm);
            }
        }

        /// <summary>
        /// SQL传出参数的赋值。在保证参数转换时，顺序一致的情况下。调用。
        /// </summary>
        /// <param name="SqlCmd">来源：CMD对象</param>
        /// <param name="destParams">传出：参数目的地</param>
        public static void ParameterValue_Deliver(OracleCommand SqlCmd, List<IDbDataParameter> destParams)
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

        private static void PrepareCommand(OracleTransaction trans, OracleConnection conn, OracleCommand Cmd, CommandType cmdType, string cmdText, List<IDbDataParameter> cmdParms)
        {
            Cmd.Connection = conn;
            Cmd.CommandText = cmdText;
            if (trans != null)
            {
                Cmd.Transaction = trans;
                Cmd.Connection = trans.Connection;
            }
            Cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                ParameterValue_Introduce(cmdParms, Cmd);
            }
        }

        public void SetCmdTimeOut(int interal)
        {
            PV_CommandTimeout = interal;
        }
#if DebugSql
        public void DebugOn()
        {
            throw new NotImplementedException();
        }

        public string DebugOff()
        {
            throw new NotImplementedException();
        }
#endif
    }
}