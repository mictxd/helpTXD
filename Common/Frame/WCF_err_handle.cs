using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using log4net;


/*
 使用方法 
 在wcf实现契约的类上声明
   [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    [WCF_ExceptionHandler.WCF_ExceptionBehaviourAttribute(typeof(WCF_ExceptionHandler))]
    比如我这里

    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    [WCF_ExceptionHandler.WCF_ExceptionBehaviourAttribute(typeof(WCF_ExceptionHandler))]
    public class SvrEduEnglish : IEduEngSvr
    {。。。。。。
    }
     
     */
/// <summary> 
/// WCF服务端异常处理器 
/// </summary> 
public class WCF_ExceptionHandler : IErrorHandler
{
    #region IErrorHandler Members

    /// <summary> 
    /// HandleError 
    /// </summary> 
    /// <param name="ex">ex</param> 
    /// <returns>true</returns> 
    public bool HandleError(Exception ex)
    {
        return true;
    }

    /// <summary> 
    /// ProvideFault 
    /// </summary> 
    /// <param name="ex">ex</param> 
    /// <param name="version">version</param> 
    /// <param name="msg">msg</param> 
    public void ProvideFault(Exception ex, MessageVersion version, ref Message msg)
    {
        // 
        //在这里处理服务端的消息，将消息写入服务端的日志 
        // 
        string err = string.Format("调用WCF接口 '{0}' 出错", ex.TargetSite.ReflectedType.FullName + "." + ex.TargetSite.Name) + Environment.NewLine + ex.Message;
        var newEx = new FaultException(err);
        ILog alog = log4net.LogManager.GetLogger("SysRun");
        alog.Error(string.Format("调用WCF接口 '{0}' 出错", ex.TargetSite.ReflectedType.FullName + "." + ex.TargetSite.Name) + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace); //+ex.TargetSite.GetParameters
        MessageFault msgFault = newEx.CreateMessageFault();
        msg = Message.CreateMessage(version, msgFault, newEx.Action);
    }

    #endregion


    /// <summary> 
    /// WCF服务类的特性 
    /// </summary> 
    public class WCF_ExceptionBehaviourAttribute : Attribute, IServiceBehavior
    {
        private readonly Type _errorHandlerType;

        public WCF_ExceptionBehaviourAttribute(Type errorHandlerType)
        {
            _errorHandlerType = errorHandlerType;
        }

        #region IServiceBehavior Members

        public void Validate(ServiceDescription description,
            ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription description,
            ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection parameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription description,
            ServiceHostBase serviceHostBase)
        {
            var handler =
                (IErrorHandler) Activator.CreateInstance(_errorHandlerType);

            foreach (ChannelDispatcherBase dispatcherBase in
                serviceHostBase.ChannelDispatchers)
            {
                var channelDispatcher = dispatcherBase as ChannelDispatcher;
                if (channelDispatcher != null)
                    channelDispatcher.ErrorHandlers.Add(handler);
            }
        }

        #endregion
    }
}