using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace App.Filters
{
    //public class TransactionAttribute : Attribute, IActionFilter,IExceptionFilter
    //{
    //    TransactionScope Scope = null;

    //    public void OnActionExecuted(ActionExecutedContext context)
    //    {
    //        Scope.Complete();
    //        Scope.Dispose();
    //    }

    //    public void OnActionExecuting(ActionExecutingContext context)
    //    {
    //        Scope = new TransactionScope();
    //    }

    //    public void OnException(ExceptionContext context)
    //    {
    //        Scope.Dispose();
    //    }
    //}
}
