using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
namespace TXD.CF.DB
{
    public class SqlDBDataAccess : IDataAccess
    {
        #region Property
       
        private SqlConnection _connection;
        private SqlTransaction _transaction;
        private int PV_CommandTimeout = 6000;
        #endregion

        public SqlDBDataAccess()
        {
              string ConnectionStringLocalTransaction = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

            _connection = new SqlConnection(ConnectionStringLocalTransaction);
        }
        public SqlDBDataAccess(string conn)
        {
            _connection = new SqlConnection(conn);
        }
        public string GetServerName()
        {
            return _connection.DataSource;
        }

        #region IDataAccess 成员

      
        public int ExecuteNonQuery(string cmdText, List<IDbDataParameter> param_List)
        {
            SqlCommand oleCmd = new SqlCommand();
            try
            {
                if (_transaction == null)
                {
                    if (_connection.State == ConnectionState.Closed)
                    { _connection.Open(); }
                    oleCmd.Connection = _connection;
                }
                else
                {
                    oleCmd.Transaction = _transaction;
                    oleCmd.Connection = _transaction.Connection;
                }
                oleCmd.CommandText = cmdText;
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
            SqlCommand oleCmd = new SqlCommand();
            try
            {
                if (_transaction == null)
                {
                    if (_connection.State == ConnectionState.Closed)
                    { _connection.Open(); }
                    oleCmd.Connection = _connection;
                }
                else
                {
                    oleCmd.Transaction = _transaction;
                    oleCmd.Connection = _transaction.Connection;
                }
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
            //技术说明
            //不带事务时  SqlDataReader 出错不会关闭连接，即使调用参数加上。所以catch到错误时，关闭连接。
            //带上事务时  情况复杂，不能关闭连接。
            SqlCommand oleCmd = new SqlCommand();
            if (_transaction == null)
            {
                try
                {
                    if (_connection.State == ConnectionState.Closed)
                    { _connection.Open(); }
                    oleCmd.Connection = _connection;
                    oleCmd.CommandText = cmdText;
                    ParameterValue_Introduce(param_List, oleCmd);
                    SqlDataReader rdr = oleCmd.ExecuteReader(CommandBehavior.CloseConnection);
                    ParameterValue_Deliver(oleCmd, param_List);
                    oleCmd.Parameters.Clear();
                    return rdr;
                }
                catch (System.Exception ex)
                {
                    _connection.Close();
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
                    ParameterValue_Introduce(param_List, oleCmd);
                    SqlDataReader rdr = oleCmd.ExecuteReader();
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
            SqlCommand oleCmd = new SqlCommand();
            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                { _connection.Open(); }
                oleCmd.Connection = _connection;
            }
            else
            {
                oleCmd.Transaction = _transaction;
                oleCmd.Connection = _transaction.Connection;
            }
            oleCmd.CommandText = cmdText;
            ParameterValue_Introduce(param_List, oleCmd);
            object val = oleCmd.ExecuteScalar();
            ParameterValue_Deliver(oleCmd, param_List);
            oleCmd.Parameters.Clear();
            return val;
        }

        public object ExecuteScalar(string cmdText)
        {
           
            SqlCommand oleCmd = new SqlCommand();
            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                { _connection.Open(); }
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
            return new SqlParameter();
        }

        public IDbDataParameter CreatParameter(string name, object paraValue)
        {
            if (paraValue == null)
                paraValue = DBNull.Value;
            SqlParameter paramter = new SqlParameter();
            paramter.ParameterName = name;
            paramter.Value = paraValue;
            return paramter;
        }

        public IDbDataParameter CreatParameter(string name, DbType paraType, object paraValue)
        {
            SqlParameter paramter = new SqlParameter();
            paramter.ParameterName = name;
            if (paraType == DbType.Object)
            { paramter.SqlDbType = SqlDbType.Image; }
            else { paramter.DbType = paraType; }
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
            { return; }
            if (_connection.State == ConnectionState.Closed)
            { _connection.Open(); }
            _transaction = _connection.BeginTransaction(); ;
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
            FAutoIncrease = 0;//表示自增长还没获取
          

            string sql = "insert into " + table + " (";
            for (int i = 0; i < param_List.Count; i++)
            {
                SqlParameter param = (SqlParameter)param_List[i];
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
                SqlParameter param = (SqlParameter)param_List[i];
                param.ParameterName.Substring(1);
                if (i != (param_List.Count - 1))
                    sql += param.ParameterName + ",";
                else
                    sql += param.ParameterName;
            }
            sql += ")";
            SqlCommand sql_cmd = new SqlCommand();

            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                { _connection.Open(); }
                sql_cmd.Connection = _connection;
            }
            else
            {
                sql_cmd.Transaction = _transaction;
                sql_cmd.Connection = _transaction.Connection;
            }
            ParameterValue_Introduce(param_List, sql_cmd);
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
            ParameterValue_Deliver(sql_cmd, param_List);
            sql_cmd.Parameters.Clear();
            sql = "select  @@IDENTITY as iden from " + table;
            sql_cmd.CommandText = sql;
            object lvo = null;
            lvo = sql_cmd.ExecuteScalar();
            if (lvo != null && lvo != DBNull.Value)
            { FAutoIncrease = Convert.ToInt32(lvo); }
            return ret;
        }

        public int ExecuteInsert(string table, List<IDbDataParameter> param_List)
        {
            int ret = 0;
           
            string sql = "insert into " + table + " (";
            for (int i = 0; i < param_List.Count; i++)
            {
                SqlParameter param = (SqlParameter)param_List[i];
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
                SqlParameter param = (SqlParameter)param_List[i];
                param.ParameterName.Substring(1);
                if (i != (param_List.Count - 1))
                    sql += param.ParameterName + ",";
                else
                    sql += param.ParameterName;
            }
            sql += ")";
            SqlCommand sql_cmd = new SqlCommand();
            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                { _connection.Open(); }
                sql_cmd.Connection = _connection;
            }
            else
            {
                sql_cmd.Transaction = _transaction;
                sql_cmd.Connection = _transaction.Connection;
            }
            ParameterValue_Introduce(param_List, sql_cmd);
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
          
            SqlParameter param = null;



            for (int i = 0; i < param_List.Count - p条件参数个; i++)
            {
                param = (SqlParameter)param_List[i];
                if (i != 0)
                    sql += "," + param.ParameterName.Substring(1) + "=" + param.ParameterName;
                else
                    sql += param.ParameterName.Substring(1) + "=" + param.ParameterName;
            }
            string str_where = "";
            for (int i = param_List.Count - p条件参数个; i < param_List.Count; i++)
            {
                param = (SqlParameter)param_List[i];
                if (i != param_List.Count - p条件参数个)
                    str_where += " and " + param.ParameterName.Substring(1) + "=" + param.ParameterName;
                else
                    str_where += param.ParameterName.Substring(1) + "=" + param.ParameterName;
            }
            sql += " where " + str_where;
            if (pwhere语句_其他部分 != null)
                sql += " " + pwhere语句_其他部分;
            SqlCommand sql_cmd = new SqlCommand();
            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                { _connection.Open(); }
                sql_cmd.Connection = _connection;
            }
            else
            {
                sql_cmd.Transaction = _transaction;
                sql_cmd.Connection = _transaction.Connection;
            }
            ParameterValue_Introduce(param_List, sql_cmd);
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
            SqlCommand sql_cmd = new SqlCommand();

            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                { _connection.Open(); }
                sql_cmd.Connection = _connection;
            }
            else
            {
                sql_cmd.Transaction = _transaction;
                sql_cmd.Connection = _transaction.Connection;
            }
            ParameterValue_Introduce(param_List, sql_cmd);

            sql_cmd.CommandText = cmdText;
            SqlDataAdapter sqladapter = new SqlDataAdapter(sql_cmd);
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
            SqlCommand sql_cmd = new SqlCommand();


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

            sql_cmd.CommandText = proc_name;

            // Define the data adapter and fill the dataset
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(sql_cmd))
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

        #endregion



        /// <summary>
        /// 将参数引入到command里。保证了两边顺序是一致的。
        /// </summary>
        /// <param name="sourceParams">传入参数清单</param>
        /// <param name="SqlCmd">目的地：CMD对象</param>
        public static void ParameterValue_Introduce(List<IDbDataParameter> sourceParams, SqlCommand SqlCmd)
        {
            if (sourceParams == null)
            { return; }
            SqlParameter parm = null;
            foreach (IDbDataParameter Cp in sourceParams)
            {
                parm = new SqlParameter();
                parm.ParameterName = Cp.ParameterName;
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
        public static void ParameterValue_Deliver(SqlCommand SqlCmd, List<IDbDataParameter> destParams)
        {
            if ((destParams == null) || (SqlCmd.Parameters == null))
            { return; }
            for (int lvi = 0; lvi < destParams.Count; lvi++)
            {
                if (destParams[lvi].Direction != ParameterDirection.Input)
                {
                    destParams[lvi].Value = SqlCmd.Parameters[lvi].Value;
                }
            }
        }

        private static void PrepareCommand(SqlTransaction trans, SqlConnection conn, SqlCommand oleCmd, CommandType cmdType, string cmdText, List<IDbDataParameter> cmdParms)
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


      
    }
}