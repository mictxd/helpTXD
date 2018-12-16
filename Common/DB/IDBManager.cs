using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace TXD.CF.DB
{
    public interface IDBManager
    {
        DbConnectionStringBuilder DBCSB { get; }
        //; SqlConnectionStringBuilder
    }

    public static class DBMCreator
    {
        public static IDBManager Create()
        {
            string DataBaseManager = ConfigurationManager.AppSettings["DataBaseManager"];
            Assembly asembly = Assembly.GetExecutingAssembly();
            try
            {
                asembly = Assembly.Load("TXD.CF.DB");
            }
            catch
            {
            }
            //return (TXD.CF.DB.IDataAccess)Assembly.Load("TXD.CF.DB").CreateInstance(DataBaseType);
            return (TXD.CF.DB.IDBManager) asembly.CreateInstance(DataBaseManager);
        }
    }
}