using System;
using System.Data;
using System.Data.Common;
using  System.Collections.Generic;
using System.Text;

using System.Globalization;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using TXD.CF.DB;
using log4net;

namespace TXD.CF
{
    public class LineServices : ILogin//,IContractSe
    {
        public const string LogName="SysRun";
        //private DataAccessPool Pv_DAPool = new DataAccessPool();
        private const string pv_AccDeny = "请先登录。";
        //public log4net.ILog logOndb = null;
        DateTime StrToDate_STD(string dt)
        {
            IFormatProvider culture = new CultureInfo("zh-CN", true);
            DateTime lvdt = DateTime.ParseExact(dt, "yyyy-MM-dd", culture);
            return lvdt;
        }
        #region IContractDataset Members
        public void TestOnline(ref string aTestValue)
        {
            aTestValue = "服务器在线";
        }

        public void Login(ref LoginUserWcf loginUser)
        {
            DataTable lvdt = null;
            try
            {
                loginUser.errorCode = -1;
                string ConnectionStringLocalTransaction = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
                IDataAccess dataAccess = DataAccessFactory.CreateDataAccess(ConnectionStringLocalTransaction);
                //IDataAccess dataAccess = DataAccessFactory.CreateDataAccess();
                List<IDbDataParameter> lvParams = new List<IDbDataParameter>();
                lvParams.Add(dataAccess.CreatParameter("@FLogID", loginUser.LogID));
                //登录请求数 在规定时间内必须少于3次。否则不做任何处理。就退出。这个是为安全设计。增加暴力破解的难度
                object lvo=dataAccess.ExecuteScalar("SELECT COUNT(*) FROM S_LogTry WITH(NOLOCK) WHERE FLogID= @FLogID  AND FDateCreate> DATEADD(ss,-30, GETDATE())",lvParams);
                int lv尝试登录次数=4;
                try{lv尝试登录次数= Convert.ToInt32(lvo.ToString());}catch{}
                ////提供方法执行的上下文环境
                //OperationContext context = OperationContext.Current;
                ////获取传进的消息属性
                //MessageProperties properties = context.IncomingMessageProperties;
                ////获取消息发送的远程终结点IP和端口
                //RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                //Console.WriteLine(string.Format("Hello {0},You are   from {1}:{2}", name, endpoint.Address, endpoint.Port));
                //string ip= string.Format("Hello {0},You are   from {1}:{2}", name, endpoint.Address, endpoint.Port);

                lvo = dataAccess.ExecuteScalar("INSERT INTO S_LogTry(FLogID,FDateCreate) VALUES( @FLogID,GETDATE())", lvParams);
                
                if (lv尝试登录次数 > 3) { return; }
                
                //验证账户密码

                 lvdt = dataAccess.ExecuteDataTable("select *,getdate() as DbSysTime from Mem_User WITH(NOLOCK) WHERE FDeleted = 0 AND FDeleted=0 and FLogID=@FLogID", lvParams);
                if (lvdt.Rows.Count > 0)
                {
                    if (loginUser.Pass == lvdt.Rows[0]["FPassWord"].ToString())
                    {
                        loginUser.Uid = Convert.ToInt32(lvdt.Rows[0]["FUserID"].ToString());
                       
                        loginUser.FirstName = lvdt.Rows[0]["FUserName"].ToString();
                        loginUser.Pass = "";
                        loginUser.MSG = "OK";
                        loginUser.timeIn = Convert.ToDateTime(lvdt.Rows[0]["DbSysTime"]);

                        loginUser.Gid = 0;
                        string mesge = "";
                        loginUser.AX = ""; HelpTXD.ToMD5(loginUser.timeIn.ToString("yyyyMMddHHmmssfff"), ref loginUser.AX, ref mesge);
                        if (mesge == "")
                        {
                            HelpTXD.Key2Base(ref loginUser.AX, ref  loginUser.AT);
                            loginUser.Gid = Convert.ToInt64(dataAccess.ExecuteScalar("INSERT INTO Mem_User_LogIn(FUserID,FSessionKey,FDateLogIn,FDateLogOut)  VALUES(" + loginUser.Uid.ToString() + ",'" + loginUser.AT + "','" + loginUser.timeIn.ToString("yyyy-MM-dd HH:mm:ss") + "',null);select SCOPE_IDENTITY() ", null));
                            loginUser.XT = CryptoHelper.Encrypt(ConnectionStringLocalTransaction, LoginUserWcf.CytcryptKey);
                            loginUser.errorCode = 0;
                        }
                       
                    }
                    else { loginUser.errorCode = 2; loginUser.MSG = "密码错误"; }
                }
                else { loginUser.errorCode = 1; loginUser.MSG = "无此账号"; }
            }
            catch (Exception ex)
            {
                loginUser.errorCode = 3; loginUser.MSG = "未知错误,请查看日志";
                log4net.ILog logsys = log4net.LogManager.GetLogger(LogName);
                logsys.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
     
        public void LogOut(ref LoginUserWcf loginUser)
        {
            //寫注冊時間。

        }

        public void LogSet(ref LoginUserWcf loginUser, string OldPass, string Newps)
        {
            int lvrow = -1;
            //IDbTransaction lvTrans = dataAccess.utlBeginTransaction();
            IDataAccess dataAccess = DataAccessFactory.CreateDataAccess();
            IDbDataParameter lvParam = null;
            List<IDbDataParameter> lvParams = new List<IDbDataParameter>();
            lvParam = dataAccess.CreatParameter();
            lvParam.DbType = DbType.String;
            lvParam.Size = 100;
            lvParam.ParameterName = "@LogID";
            lvParam.Value = loginUser.LogID;
            lvParams.Add(lvParam);
            DataTable lvdt = dataAccess.ExecuteDataTable("select * from  Mem_User WITH(NOLOCK) where FLogID=@LogID and FDeleted=0", lvParams);
            if (lvdt.Rows.Count != 1) { return; }
            if (lvdt.Rows[0]["FPassWord"].ToString() != loginUser.XT.ToString()) { loginUser.errorCode = 3; return; }


            lvParam = dataAccess.CreatParameter();
            lvParam.DbType = DbType.String;
            lvParam.Size = 100;
            lvParam.Value = loginUser.NT;
            lvParam.ParameterName = "@PassWordNew";
            lvParams.Add(lvParam);
            //lvParam = dataAccess.CreatParameter();
            //lvParam.DbType = DbType.String;
            //lvParam.Size = 100;

            //lvParam.Value = aUserCertify.XT;
            //lvParam.ParameterName = "@PassWordOld";
            //lvParams.Add(lvParam);
            lvrow = dataAccess.ExecuteNonQuery("update Mem_User set FPassWord=@PassWordNew  where FLogID=@LogID ", lvParams);
            if (lvrow == 1)
            { loginUser.errorCode = 0; }

        }
        public void LogSign(ref LoginUserWcf loginUser, string Msg, ref Boolean Done)
        {
            //检查此人登录
            DateTime lvtimeStart = DateTime.Now.AddHours(-8);
            string lvSql = "select count(*) from Mem_User_LogIn with(nolock) where FUserID=" + loginUser.Uid.ToString() + @" and FDateLogIn>@FDateLogIn";
            
            IDataAccess dataAccess = DataAccessFactory.CreateDataAccess();
           
            List<IDbDataParameter> lvParams = new List<IDbDataParameter>();
            object lvo = dataAccess.ExecuteScalar(lvSql, lvParams);
            if ((lvo != null) && (lvo != DBNull.Value))
            { if (Convert.ToInt32(lvo) <= 0) { return; } }
            //最近登陆过的。那就把日志写到文件里。
            //提供方法执行的上下文环境
            OperationContext context = OperationContext.Current;
            //获取传进的消息属性
            MessageProperties properties = context.IncomingMessageProperties;
            //获取消息发送的远程终结点IP和端口
            RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                
            string ip = string.Format("From {0}:{1}",  endpoint.Address, endpoint.Port);
            log4net.ILog logsys = log4net.LogManager.GetLogger(LogName);
            logsys.Info(ip + "  " + loginUser.LogID + ":" + loginUser.FirstName.Trim() + Environment.NewLine + Msg);
            Done = true;
        }
        #endregion
        public static bool LogPermmision(ref LoginUserWcf loginUser)
        {
            bool lvRet = false;
            string ConnectionStringLocalTransaction = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            IDataAccess dataAccess = DataAccessFactory.CreateDataAccess(ConnectionStringLocalTransaction);
            lvRet = LogPermmision(dataAccess, ref  loginUser);
            return lvRet;
        }
        /// <summary>
        /// 传来的用户，是否已经登录。
        /// </summary>
        /// <param name="dataAccess"></param>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        public static bool LogPermmision(IDataAccess dataAccess, ref LoginUserWcf loginUser)
        {
            bool lvresult = false;

            lvresult = LogPermmision(dataAccess, ref loginUser, ref loginUser.MSG);
            
            return lvresult;

        }
        private static bool LogPermmision(IDataAccess dataAccess, ref  LoginUserWcf aUserCertify, ref string MSG)
        {
            // LoginUserWcf lvLUW = GetUserLogInfo(dataAccess, aUserCertify.Gid, aUserCertify.Uid);
            bool lvresult = false;
            //LoginUserWcf aUserCertify = new LoginUserWcf();
            string AT = "";
            DataTable lvdt = dataAccess.ExecuteDataTable("SELECT *  FROM Mem_User_LogIn WITH(NOLOCK) where FID_L=" + aUserCertify.Gid.ToString() + " and FUserID=" + aUserCertify.Uid.ToString(), null);
            try
            {
                AT = lvdt.Rows[0]["FSessionKey"].ToString();
            }
            catch { }
            if (AT == "") { MSG = pv_AccDeny; }
            if (aUserCertify.AT == AT) { lvresult = true; }
            else { MSG = pv_AccDeny; }
            return lvresult;

        }


    }

}