using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
/*
config like this
 <connectionStrings>
   <add name="MainDBConnectionString" connectionString="server=.;uid=username; pwd=xxx;database=dbname" />

  </connectionStrings>
  <appSettings>
  <!-- as my code : IDataAccess to AccessDataAccess or to SqlDBDataAccess or to MySqlDBDataAccess else on -->
    <!--<add key="DataBaseType" value="TXD.CF.DB.DataAccess.AccessDataAccess" />-->
    <!--<add key="DataBaseType" value="TXD.CF.DB.DataAccess.SqlDBDataAccess"/>-->
    <add key="DataBaseType" value="TXD.CF.DB.MySqlDBDataAccess" />
	<!--  if encrypted  value=true then read pass from "encryptString" -->
    <add key="encryptDBP" value="true" />
    <add key="encryptString" value="fake" />
  </appSettings>
*/
namespace TXD.CF.DB
{
    public class DataAccessFactory
    {
        private static string DataBaseType = System.Configuration.ConfigurationManager.AppSettings["DataBaseType"];

        public static IDataAccess CreateDataAccess()
        {
            Assembly asembly = Assembly.GetExecutingAssembly();
            //Console.Write("cons zijixied "+ asembly.FullName);
            //throw new Exception(asembly.FullName);
            //log4net.ILog logsys = log4net.LogManager.GetLogger("SysRun");
            //logsys.Debug(asembly.FullName);

            try
            {
                asembly = Assembly.Load("TXD.CF.DB");
            }
            catch
            {
            }
            if (string.IsNullOrEmpty(DataBaseType)) //为了通过ci集成测试。调整部署容错。
            {
                string cfgname = asembly.Location;
                System.Configuration.Configuration lvvv = System.Configuration.ConfigurationManager.OpenExeConfiguration(cfgname);
                DataBaseType = lvvv.AppSettings.Settings["DataBaseType"].Value;
                //Console.WriteLine("asembly " + asembly.FullName);
                //Console.WriteLine("asembly.Location " + asembly.Location);
                //Console.WriteLine("lvvv.FilePath " + lvvv.FilePath);
                //if (!string.IsNullOrEmpty(DataBaseType))
                //{
                //    Console.WriteLine(DataBaseType);
                //}
            }
            //return (TXD.CF.DB.IDataAccess)Assembly.Load("TXD.CF.DB").CreateInstance(DataBaseType);
            return (TXD.CF.DB.IDataAccess) asembly.CreateInstance(DataBaseType);
        }

        public static IDataAccess CreateDataAccess(string connStr)
        {
            string[] args = new string[1];
            args[0] = connStr;
            Assembly asembly = Assembly.GetExecutingAssembly();
            try
            {
                asembly = Assembly.Load("TXD.CF.DB");
            }
            catch
            {
            }

            object frmObj = null;
            try
            {
                frmObj = asembly.CreateInstance("TXD.CF.DB.MySqlDBDataAccess", false, BindingFlags.Default, Type.DefaultBinder, args, null, null);
            }
            catch
            {
            }
            if (frmObj == null)
            {
                frmObj = asembly.CreateInstance("MySqlDBDataAccess", false, BindingFlags.Default, Type.DefaultBinder, args, null, null);
            }
            //if (frmObj == null)
            //{ frmObj = new TXD.CF.DB.SqlDBDataAccess(connStr); }
            return (TXD.CF.DB.IDataAccess) frmObj;
        }

        public static IDataAccess CreateDataAccess(string server, string TheDBName, string logID, string pwd)
        {
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
            builder.DataSource = server;
            builder.InitialCatalog = TheDBName;
            builder.UserID = logID;
            builder.Password = pwd;

            string connStr = builder.ConnectionString;
            string[] args = new string[1];
            args[0] = connStr;

            return (TXD.CF.DB.IDataAccess) Assembly.Load("TXD.CF.DB").CreateInstance(DataBaseType, false, BindingFlags.Default, Type.DefaultBinder, args, null, null);
        }
    }
}