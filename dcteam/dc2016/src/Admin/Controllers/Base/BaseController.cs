using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DC2016.BLL;
using DC2016.Model;
using Microsoft.Extensions.Logging;
using DC2016.Admin.Controllers.Common;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using dywebsdk.Web;
using DC2016.Admin.Configs;
using DC2016.Admin.Models;
using System.IO;

namespace DC2016.Admin.Controllers.Base
{
    /// <summary>
    /// 业务控制器基类，同一继承
    /// </summary>
    public class BaseController<T> : Controller
    {
        /// <summary>
        /// 日记记录器
        /// </summary>
        protected readonly ILogger<T> Logger;

        #region 安全校验白名单

        private static string[] WhitelistActions = { };

        #endregion

        #region 属性

        /// <summary>
        /// 获取 请求标识
        /// </summary>
        public string RequestID
        {
            get
            {
                return RequestIDModel.GetRequestID(this.HttpContext);
            }
        }

        /// <summary>
        /// 获取IP
        /// </summary>
        public string IP
        {
            get
            {
                return this.HttpContext.Connection.RemoteIpAddress.ToString();
                //return WebHelper.GetRealIP(this.HttpContext.Request);
            }
        }

        #endregion

        #region 构造

        public BaseController(ILogger<T> logger)
        {
            this.Logger = logger;
        }

        #endregion

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //将当前日志组件传递到其他中间件
            this.HttpContext.Items.Add("__requestLogger", this.Logger);

            string path = context.HttpContext.Request.Path.Value.ToLower();
            if (!path.Contains("/test/") && !WhitelistActions.Contains(path))
            {
                WebParams webParams = new WebParams(context.HttpContext.Request);
                
                #region 安全检查、passport验证

                string passport = webParams["passport"];
                
                if (string.IsNullOrEmpty(passport))
                {
                    context.Result = this.FuncResult(new APIReturn(10009, "缺少passport参数"));
                }
                else if (passport != DC2Conf.Passport)
                {
                    context.Result = this.FuncResult(new APIReturn(10750, "passport参数错误"));
                }

                #endregion
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {

        }

        /// <summary>
        /// 统一处理API返回值
        /// </summary>
        /// <param name="apiRet">API返回值</param>
        /// <returns>API返回值</returns>
        protected IActionResult FuncResult(APIReturn apiRet)
        {
            //根据不同策略可以返回纯文本、JSON、XML等格式
            apiRet.RequestID = this.RequestID;

            return this.Json(apiRet);
        }
    }
}
