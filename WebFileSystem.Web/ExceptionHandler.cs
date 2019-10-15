using Abp.Dependency;
using Abp.Events.Bus.Exceptions;
using Abp.Events.Bus.Handlers;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFileSystem.Web {
    public class ExceptionHandler : IEventHandler<AbpHandledExceptionData>, ITransientDependency {
        public ILogger Logger { get; set; }
        public ExceptionHandler()
        {
            Logger = NullLogger.Instance;
        }
        public void HandleEvent(AbpHandledExceptionData eventData)
        {
            Logger.Error("服务器内部错误：" + eventData.Exception.Message, eventData.Exception);
        }
    }
}