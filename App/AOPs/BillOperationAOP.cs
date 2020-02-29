using Castle.DynamicProxy;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog.Extensions.Logging;

namespace App.AOPs
{
    public class BillOperationAOP : IInterceptor
    {
        private static readonly ILogger logger = LogManager.GetLogger("logdb");
        public void Intercept(IInvocation invocation)
        { 
            invocation.Proceed();
        }
    }
}
