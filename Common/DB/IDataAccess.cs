using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

//using System.Data.Common;

namespace TXD.CF.DB
{
    public interface IDataAccess
    {
        void SetCmdTimeOut(int interal);

        /// <summary>
        /// 一般SQL语句。
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="param_List"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string cmdText, List<IDbDataParameter> param_List);

        /// <summary>
        /// 一般SQL语句。
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="param_List"></param>
        /// <returns></returns>
        int ExecuteNonQueryProc(string ProcName, List<IDbDataParameter> param_List);

        /// <summary>
        /// 传输MODEL的时候用比较好. 释放datareader不会关闭连接，
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="param_List"></param>
        /// <returns></returns>
        IDataReader ExecuteReader(string cmdText, List<IDbDataParameter> param_List);

        //IDataReader ExecuteReaderProc(string cmdText, List<IDbDataParameter> param_List);
        //object ExecuteScalarProc( string cmdText, List<IDbDataParameter> param_List);
        object ExecuteScalar(string cmdText, List<IDbDataParameter> param_List);

        /// <summary>
        /// 参数前缀没机会修正，请使用带名称参数的
        /// </summary>
        /// <returns></returns>
        //System.Data.IDbDataParameter CreatParameter();
        System.Data.IDbDataParameter CreatParameter(string name, Object paraValue);

        System.Data.IDbDataParameter CreatParameter(string name, DbType paraType, Object paraValue);

        /// <summary>
        /// 开启事务。获取的事务对象会在以后的配对调用中被消除。需要保证子类实现可嵌套事务。
        /// </summary>
        /// <returns></returns>
        void utlBeginTransaction();

        /// <summary>
        /// 事务回滚
        /// </summary>
        /// <param name="tran">要回滚的事务。将在回滚完消除。</param>
        void utlRollback();

        /// <summary>
        /// 事务提交
        /// </summary>
        /// <param name="tran">要提交的事务。将在提交后消除。</param>
        void utlCommit();

        int ExecuteInsert(string table, List<IDbDataParameter> param_List, out int FAutoIncrease);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="param_List"></param>
        /// <returns></returns>
        int ExecuteInsert(string table, List<IDbDataParameter> sqlparam);


        /// <summary>
        /// 带事务版更新语句
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="cmdType">SQL使用类型</param>
        /// <param name="table">表名</param>
        /// <param name="param_List">参数表。set 开始，where的在末尾</param>
        /// <param name="p条件参数个">where参数总共多少个，必须是sqlparam的末尾部分</param>
        /// <param name="p更多where">比较复杂的where条件可以放这里。必须以and开头</param>
        /// <returns>影响行数</returns>
        int ExecuteUpdate(string table, List<IDbDataParameter> sqlparam, int p条件参数个, string p更多where);

        //int ExecuteUpdate(string table, List<IDbDataParameter> sqlparam, int p条件参数个, string append_str);
        int ExecuteUpdate(string table, List<IDbDataParameter> sqlparam, int p条件参数个);
        DataTable ExecuteDataTable(string cmdText);
        DataTable ExecuteDataTable(string cmdText, List<IDbDataParameter> param_List);

        DataTable ExecuteDataTableProc(string proc_name, List<IDbDataParameter> param_List);

        /// <summary>
        /// 可关闭的对服务端的必须支持。在压力大的情况下，检查代码的关闭动作。
        /// </summary>
        void Close();
#if DebugSql
        void DebugOn();
        string DebugOff();
#endif
        string GetServerName();
        DateTime GetServerTime();
    }
}