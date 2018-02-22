using System.Collections;

namespace IF2017.Admin.Controllers.Common
{
    /// <summary>
    /// API结果类
    /// </summary>
    public class APIReturn
    {
        /// <summary>
        /// 获取 返回码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 获取 返回消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 获取 返回消息
        /// </summary>
        public string RequestID { get; set; }

        /// <summary>
        /// 获取 业务数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 默认构造
        /// </summary>
        public APIReturn()
        {
            this.Data = new Hashtable();
        }

        /// <summary>
        /// 根据返回码构造
        /// </summary>
        /// <param name="code">返回码</param>
        public APIReturn(int code) : this() { this.Code = code; }

        /// <summary>
        /// 根据返回码、消息构造
        /// </summary>
        /// <param name="code">返回码</param>
        /// <param name="msg">消息</param>
        public APIReturn(int code, string msg) : this(code)
        {
            this.Msg = msg;
        }

        /// <summary>
        /// 根据返回码、数据构造
        /// </summary>
        /// <param name="code">返回码</param>
        /// <param name="data">数据</param>
        public APIReturn(int code, object data) : this(code)
        {
            this.Data = data;
        }

        /// <summary>
        /// 根据返回码、消息、数据构造
        /// </summary>
        /// <param name="code">返回码</param>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        public APIReturn(int code, string message, object data) : this(code, message)
        {
            this.Data = data;
        }
    }
}
