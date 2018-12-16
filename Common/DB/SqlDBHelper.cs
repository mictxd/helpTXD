using System;
using System.Collections.Generic;
using System.Text;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TXD.CF.DB
{ 
    /// <summary>
    /// The SqlHelper class is intended to encapsulate high performance, 
    /// scalable best practices for common uses of SqlClient.
    /// </summary>
   
   public class SqlHelper
    {

       public static readonly string ConnectionStringLocalTransaction = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;



       public static void ExecuteDataAdapter(SqlTransaction trans, SqlConnection conn, CommandType cmdType, string cmdText, List<IDbDataParameter> cmdParams, ref SqlDataAdapter dataAdapter)
        {
            try
            {

                //SqlDataAdapter dataAdapter = null;
                if (dataAdapter == null)
                {
                    dataAdapter = new SqlDataAdapter();
                }
                //else
                //{
                //    dataAdapter = (SqlDataAdapter)dataAdapter;
                //}
                dataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                dataAdapter.SelectCommand = new SqlCommand(cmdText, conn, trans);
                PrepareCommand(trans, conn, dataAdapter.SelectCommand, cmdType, cmdText, cmdParams);
                if (conn.State == ConnectionState.Closed)
                {conn.Open();}
                SqlCommandBuilder builder = new SqlCommandBuilder(dataAdapter);

            }
            catch (Exception ex)
            {
                throw ex.GetBaseException();
            }
        }
       public static void ExecuteDataSet(SqlTransaction trans, SqlConnection conn, CommandType cmdType, string cmdText, List<IDbDataParameter> cmdParams, ref DataSet ds)
        {

            if (ds == null)
            {
                ds = new DataSet();
            }
            ds.Tables.Clear();
            SqlCommand oleCmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter();
            SqlDataAdapter sdaScm = new SqlDataAdapter();
            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(trans, conn, oleCmd, cmdType, cmdText, cmdParams);
                sda.SelectCommand = oleCmd;
               // sda.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                sda.Fill(ds);
                //sda.Fill(ds);
                //ds.Tables.Clear();
                
                DataSet lvds = new DataSet();
                sdaScm.SelectCommand = oleCmd;
                 sdaScm.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                
                sdaScm.FillSchema(lvds, SchemaType.Source);

                foreach (DataColumn lvdc in lvds.Tables[0].Columns)
                {
                    if (lvdc.DataType == typeof(System.String))
                    {
                        ds.Tables[0].Columns[lvdc.ColumnName].MaxLength = lvdc.MaxLength;
                    }
                }

                sda.SelectCommand.Parameters.Clear();
                sda.Dispose();
                oleCmd.Parameters.Clear();
                oleCmd.Dispose();
            }
            catch (System.Exception ex)
            {
               throw ex.GetBaseException();
            }
        }
       //public static DataSet ExecuteDataSet(SqlConnection conn, CommandType cmdType, string cmdText, List<IDbDataParameter> cmdParams)
        //{
        //    DataSet ds = new DataSet();

        //    SqlCommand oleCmd = new SqlCommand();
        //    oleCmd.CommandTimeout = 3000;
        //    // we use a try/catch here because if the method throws an exception we want to 
        //    // close the connection throw code, because no datareader will exist, hence the 
        //    // commandBehaviour.CloseConnection will not work
        //    try
        //    {

       //        PrepareCommand(oleCmd, conn, null, cmdType, cmdText, cmdParams);
        //        SqlDataAdapter sda = new SqlDataAdapter();
        //        sda.SelectCommand = oleCmd;
        //        sda.Fill(ds, "table");
        //        //oleCmd.Connection.Close();
        //        return ds;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex.GetBaseException();
        //    }
        //}
        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = RunSql(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
       /// <param name="cmdParams">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
       public static int ExecuteNonQuery(SqlTransaction trans, SqlConnection connection, CommandType cmdType, string cmdText, List<IDbDataParameter> cmdParams)
        {
            SqlCommand oleCmd = new SqlCommand();
            PrepareCommand(trans, connection, oleCmd, cmdType, cmdText, cmdParams);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            int val = oleCmd.ExecuteNonQuery();
            SqlParameter lvp=null;
            if ((cmdParams != null) && (oleCmd.Parameters != null))
            {
                foreach (IDbDataParameter ipar in cmdParams)
                {
                    if (ipar.Direction != ParameterDirection.Input)
                    {
                        for (int lvi = 0; lvi < oleCmd.Parameters.Count; lvi++)
                        {
                            lvp = oleCmd.Parameters[lvi];
                            if (lvp.ParameterName == ipar.ParameterName)
                            {
                                ipar.Value = lvp.Value;
                                break;
                            }
                        }
                    }
                }
            }
            oleCmd.Parameters.Clear();
            return val;
        }
        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = RunSql(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
       /// <param name="cmdParams">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
       public static int ExecuteNonQueryWithOutParams(SqlTransaction trans, SqlConnection connection, CommandType cmdType, string cmdText, ref List<IDbDataParameter> cmdParams)
        {
            SqlCommand oleCmd = new SqlCommand();
            PrepareCommand(trans, connection, oleCmd, cmdType, cmdText, cmdParams);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            int val = oleCmd.ExecuteNonQuery();
            //传出参数 重新赋值 ？？？？？???
            int lvi=0;
            foreach (IDbDataParameter lvcmd in cmdParams)
            {
                if (lvcmd.Direction!=ParameterDirection.Input)
                {
                    lvcmd.Value = oleCmd.Parameters[lvi].Value;
                }
                lvi++;
            }
            oleCmd.Parameters.Clear();

            return val;
        }
        ///// <summary>
        ///// Execute a SqlCommand (that returns no resultset) against an existing database connection 
        ///// using the provided parameters.
        ///// </summary>
        ///// <remarks>
        ///// e.g.:  
        /////  int result = RunSql(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        ///// </remarks>
        ///// <param name="conn">an existing database connection</param>
        ///// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ///// <param name="commandText">the stored procedure name or T-SQL command</param>
        ///// <param name="cmdParams">an array of SqlParamters used to execute the command</param>
        ///// <returns>an int representing the number of rows affected by the command</returns>
        //public static int ExecuteNonQuery(SqlTransaction trans, SqlConnection connection, CommandType cmdType, string cmdText, List<IDbDataParameter> cmdParams)
        //{
        //    SqlCommand oleCmd = new SqlCommand();
        //    PrepareCommand(trans, connection, oleCmd, cmdType, cmdText, cmdParams);
        //    if (connection.State == ConnectionState.Closed)
        //    {
        //        connection.Open();
        //    }
        //    int val = oleCmd.ExecuteNonQuery();
       
        //    oleCmd.Parameters.Clear();

        //    return val;
        //}
        // <summary>
        // Execute a SqlCommand (that returns no resultset) using an existing SQL Transaction 
        // using the provided parameters.
        // </summary>
        // <remarks>
        // e.g.:  
        //  int result = RunSql(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        // </remarks>
        // <param name="trans">an existing sql transaction</param>
        // <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        // <param name="commandText">the stored procedure name or T-SQL command</param>
        // <param name="cmdParams">an array of SqlParamters used to execute the command</param>
        // <returns>an int representing the number of rows affected by the command</returns>
        //public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, List<IDbDataParameter> cmdParams)
        //{
        //    SqlCommand oleCmd = new SqlCommand();
        //    PrepareCommand(oleCmd, trans.Connection, trans, cmdType, cmdText, cmdParams);
        //    int val = oleCmd.ExecuteNonQuery();
        //    oleCmd.Parameters.Clear();
        //    return val;
        //}

        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = RunSql(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="cmdParams">an array of SqlParamters used to execute the command</param>
        ///// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        //public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, List<IDbDataParameter> cmdParams)
        //{
        //    SqlCommand oleCmd = new SqlCommand();

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        PrepareCommand(oleCmd, connection, null, cmdType, cmdText, cmdParams);
        //        object val = oleCmd.ExecuteScalar();
        //        oleCmd.Parameters.Clear();
        //        return val;
        //    }
        //}

        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = RunSql(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="cmdParams">an array of SqlParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(SqlTransaction trans, SqlConnection conn, CommandType cmdType, string cmdText, List<IDbDataParameter> cmdParams)
        {
            SqlCommand oleCmd = new SqlCommand();
            PrepareCommand(trans, conn, oleCmd, cmdType, cmdText, cmdParams);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            object val = oleCmd.ExecuteScalar();
            oleCmd.Parameters.Clear();
            return val;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParams"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(SqlTransaction trans, SqlConnection conn, CommandType cmdType, string cmdText, List<IDbDataParameter> cmdParams)
        {
            if (conn.State== ConnectionState.Closed) { conn.Open(); }
            SqlCommand oleCmd = new SqlCommand();
            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(trans, conn, oleCmd, cmdType, cmdText, cmdParams);
                SqlDataReader rdr = oleCmd.ExecuteReader(CommandBehavior.CloseConnection);
                oleCmd.Parameters.Clear();
                return rdr;
            }
            catch (System.Exception ex)
            {
                conn.Close();
                throw ex.GetBaseException();
            }
        }
        ///// <summary>
        ///// Prepare a command for execution
        ///// </summary>
        ///// <param name="oleCmd">SqlCommand object</param>
        ///// <param name="conn">SqlConnection object</param>
        ///// <param name="trans">SqlTransaction object</param>
        ///// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
        ///// <param name="cmdText">Command text, e.g. Select * from Products</param>
        ///// <param name="cmdParms">SqlParameters to use in the command</param>
        //private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, List<IDbDataParameter> cmdParms)
        //{
        //    try
        //    {

        //        if (conn.State != ConnectionState.Open)
        //            conn.Open();

        //        oleCmd.Connection = conn;
        //        oleCmd.CommandText = cmdText;

        //        if (trans != null)
        //            oleCmd.Transaction = trans;

        //        oleCmd.CommandType = cmdType;

        //        if (oleCmdParms != null)
        //        {
        //            foreach (SqlParameter parm in cmdParms)
        //                oleCmd.Parameters.Add(parm);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.GetBaseException();
        //    }
        //}
    }
}

    
