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
        private readonly ILogger logger = LogManager.GetLogger("logfile");

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
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
                var statusCode = context.Response.StatusCode;
                AjaxResultModel<string> ajaxResultModel = new AjaxResultModel<string>(ex.Message);
                ajaxResultModel.code = 501;

                logger.Fatal(ex);
                throw ex;
            }
            finally
            {
                var statusCode = context.Response.StatusCode;
                // 未授权
                if (statusCode == 401)
                {
                    //AjaxResultModel<string> ajaxResultModel = new AjaxResultModel<string>("未授权");
                    //await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(ajaxResultModel));
                }

            }
        }

        //private Task HandleExceptionAsync(HttpContext context)
        //{
        //    if (con)
        //    {

        //    }
        //}
    }
}
