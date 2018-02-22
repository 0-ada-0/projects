using dywebsdk.Cryptography;
using dywebsdk.Extension;
using dywebsdk.Models;
using dywebsdk.Web;
using IF2017.Admin.Common;
using IF2017.Admin.Configs;
using IF2017.Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;

namespace IF2017.Admin.Controllers.Common
{
    /// <summary>
    /// 基础控制器基类
    /// </summary>
    public class BaseController<T> : Controller
    {
        private string ip;
        public string getClientIP()
        {
            return ip;
        }
        #region 安全校验白名单

        /*
         * dk/check_apple_status 需要IP范围授权，无需签名
         * 
         */
        private static string[] WhitelistActions = { "/m_game/log", "/dk/check_apple_status", "/urs/dev_test" };

        #endregion

        #region 字段

        /// <summary>
        /// 日记记录器
        /// </summary>
        protected static ILogger<T> Logger;

        #endregion

        #region 构造

        public BaseController(ILogger<T> logger)
        {
            Logger = logger;
        }

        static BaseController()
        {
            //初始化全局静态数据
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取 参数解密实体
        /// </summary>
        public RequestDataModel ReqData
        {
            get
            {
                return (RequestDataModel)HttpContext.Items[RequestDataModel.RequestDataKey];
            }
        }

        private string _requestID;

        /// <summary>
        /// 获取 请求标识
        /// </summary>
        public string ReqID
        {
            get
            {
                if (string.IsNullOrEmpty(this._requestID))
                {
                    this._requestID = RequestIDModel.GetRequestID(this.HttpContext);
                }
                return this._requestID;
            }
        }

        /// <summary>
        /// 获取 Web请求参数对象
        /// </summary>
        public Dictionary<string, object> DicParams
        {
            get
            {
                var dicParams = new Dictionary<string, object>();
                dicParams.Add(RequestIDModel.RequestIDKey, this.ReqID);
                return dicParams;
            }
        }

        /// <summary>
        /// 获取IP
        /// </summary>
        public string IP
        {
            get
            {
                //return this.HttpContext.Connection.RemoteIpAddress.ToString();
                return WebHelper.GetClientRealIP(this.HttpContext);
            }
        }

        #endregion

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ip = context.HttpContext.Request.Host.Host;
            //将当前日志组件传递到其他中间件
            this.HttpContext.Items.Add("__requestLogger", Logger);

            HttpRequest request = context.HttpContext.Request;
            WebParams webParams = new WebParams(request);

            string path = request.Path.Value.ToLower();
            if (!path.Contains("/test/") && !WhitelistActions.Contains(path))
            {
                #region 签名验证

                if (context.Result == null)
                {
                    string signKey = IFConfigReader.SignKey;
                    NameValueCollection param = new NameValueCollection();
                    foreach (var webParam in webParams)
                    {
                        param.Add(webParam.Key, webParam.Value.ToString());
                    }
                    string uriString = $"{request.Scheme}://{request.Host}{request.Path}";
                    Uri uri = new Uri(uriString);
                    if (!UrlValidator.ValidatorSign(uri, param, signKey))
                    {
                        APIReturn apiResult = new APIReturn(-93, "签名验证失败");
                        context.Result = this.FuncResult(apiResult);
                    }
                }

                #endregion

                #region 时间戳验证

                string stampKey = "timestamp";
                if (context.Result == null && webParams.ContainsKey(stampKey))
                {
                    long timeStamp = webParams[stampKey].ToLong();
                    DateTime urlTime = DateTimeHelper.StampToDateTime(timeStamp);
                    if (urlTime.AddMinutes(3) < DateTime.Now)
                    {
                        APIReturn apiResult = new APIReturn(-92, "URL已失效");
                        context.Result = this.FuncResult(apiResult);
                    }
                }

                #endregion

                #region 加密验证

                /*  暂不启用加密验证
                 *  
                if (context.Result == null)
                {
                    if (!this.ReqData.IsEncryptDatas || !this.ReqData.IsDecryptSucceed)
                    {
                        APIReturn apiResult = new APIReturn(-91, "解密参数失败" + (this.ReqData.DecryptException != null ? $"({this.ReqData.DecryptException.Message})" : string.Empty));
                        context.Result = this.FuncResult(apiResult);
                    }
                }
                *
                */

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
            apiRet.RequestID = this.ReqID;
            return this.Json(apiRet);
        }

        protected int IPToLong(string ip)
        {
            try
            {
                return BitConverter.ToInt16(IPAddress.Parse(ip).GetAddressBytes(), 0);
            }
            catch
            {
                return 0;
            }
        }
    }
}
