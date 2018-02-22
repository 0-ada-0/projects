using DC2016.Admin.Controllers.Common;
using DC2016.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC2016.Admin.Filter
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;

            ILogger logger = context.HttpContext.Items["__requestLogger"] as ILogger;
            if (logger != null)
            {
                string requestID = RequestIDModel.GetRequestID(context.HttpContext);
                logger.LogCritical("{0} {1}", requestID, context.Exception);

                APIReturn apiRet = new APIReturn(-98, $"未知系统错误：{context.Exception.Message}");
                apiRet.RequestID = requestID;
                context.Result = new JsonResult(apiRet);
            }
        }
    }
}
