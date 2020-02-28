using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Extensions
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly Microsoft.Extensions.Logging.ILogger<ErrorHandlingMiddleware> logger;

        public ErrorHandlingMiddleware(RequestDelegate next, Microsoft.Extensions.Logging.ILogger<ErrorHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            //logger.Name
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {

                var logger1 = LogManager.GetLogger("logdb");
                var logger2 = LogManager.GetLogger("logfile");
                var logger3 = LogManager.GetCurrentClassLogger();
                var statusCode = context.Response.StatusCode;
                AjaxResultModel<string> ajaxResultModel = new AjaxResultModel<string>(ex.Message);
                ajaxResultModel.code = 501;
                //context.Response.WriteAsync() 
                logger1.Debug(DateTime.Now);
                logger2.Error(DateTime.Now);
                logger3.Info(DateTime.Now);
                throw ex;
            }
            finally
            {
                var statusCode = context.Response.StatusCode;
                // 未授权
                if (statusCode == 401)
                {

                }

            }
        }

        //private Task HandleExceptionAsync(HttpContext context)
    }
}
