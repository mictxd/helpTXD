using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using TXD.CF.DB;

namespace TXD.CF
{
    public static partial class HelpTXD
    {
        //private static IDataAccess iDac = null;
        //多类型数据库，这个功能移入到接口中。
        //public static DateTime pt_serverDatetime(IDataAccess idac)
        //{
        //    return Convert.ToDateTime(idac.ExecuteScalar("select getdate()", null));
        //}
        //public static int GetDocID(IDataAccess iDac, int DocClassID)
        //{ 
        //    List<IDbDataParameter> slp = new List<IDbDataParameter>();
        //    slp.Add(iDac.CreatParameter("@FClassID", DocClassID));
        //    int lvRet = 0;
        //    iDac.ExecuteInsert("TS_DOC_ID", slp, out lvRet);
        //    return lvRet;
        //}
        public static int GetDocID(IDataAccess 假的, string TableName)
        {
            IDataAccess iDac = DataAccessFactory.CreateDataAccess();
            List<IDbDataParameter> slp = new List<IDbDataParameter>();
            slp.Add(iDac.CreatParameter("@TableName", TableName));
            slp.Add(iDac.CreatParameter("@FInterID", 0));
            slp[1].Direction = ParameterDirection.Output;
            //int lvRet = 0;
            iDac.ExecuteNonQueryProc("UP_GET_BASE_DOC_ID", slp);
            iDac.Close();
            return Convert.ToInt32(slp[1].Value);
        }

        /// <summary>
        /// 得到单据的新ID。mysql注意  这里必须开启事务
        /// </summary>
        /// <param name="假的">mySql下单独打开一个进行</param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static int GetBillID(IDataAccess 假的, string TableName)
        {
            IDataAccess iDac = DataAccessFactory.CreateDataAccess();
            List<IDbDataParameter> slp = new List<IDbDataParameter>();
            slp.Add(iDac.CreatParameter("@TableName", TableName));
            slp.Add(iDac.CreatParameter("@FInterID", 0));
            slp[1].Direction = ParameterDirection.Output;
            //int lvRet = 0;
            iDac.ExecuteNonQueryProc("UP_GET_TS_BILL_ID", slp);
            iDac.Close();
            return Convert.ToInt32(slp[1].Value);
        }

        public static int GetBillNO(string TableName)
        {
            IDataAccess iDac = DataAccessFactory.CreateDataAccess();
            List<IDbDataParameter> slp = new List<IDbDataParameter>();
            slp.Add(iDac.CreatParameter("@TableName", TableName));
            slp.Add(iDac.CreatParameter("@FInterID", 0));
            slp[1].Direction = ParameterDirection.Output;
            //int lvRet = 0;
            iDac.ExecuteNonQueryProc("UP_GET_TS_BILL_ID", slp);
            iDac.Close();
            return Convert.ToInt32(slp[1].Value);
        }

        //public static string pt_Id转换脚本条件(List<int> aIDList)
        //{
        //    string lvRet = "";
        //    foreach (int i in aIDList)
        //    {
        //        lvRet += i + ",";
        //    }

        //    if (lvRet.Length > 0)
        //    {
        //        lvRet = lvRet.Substring(0, lvRet.Length - 1);
        //    }
        //    return lvRet;
        //}
    }
}