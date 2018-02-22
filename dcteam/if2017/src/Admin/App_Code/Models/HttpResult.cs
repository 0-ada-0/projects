using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IF2017.Admin.Models
{
    public class HttpResult
    {
        /// <summary>
        /// 获取 请求是否成功
        /// </summary>
        public bool IsSucceed { get; private set; }

        /// <summary>
        /// 获取 返回内容
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// 获取 Http异常
        /// </summary>
        public Exception HttpException { get; private set; }

        public HttpResult(string content)
        {
            this.IsSucceed = true;
            this.Content = content;
        }

        public HttpResult(Exception exception)
        {
            this.IsSucceed = false;
            this.HttpException = exception;
        }
    }
}
