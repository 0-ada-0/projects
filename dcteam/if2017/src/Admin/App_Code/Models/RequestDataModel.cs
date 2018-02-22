using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IF2017.Admin.Models
{
   /// <summary>
   /// 请求参数解密实体
   /// </summary>
    public class RequestDataModel
    {
        /// <summary>
        /// 获取 请求参数解密实体KEY
        /// </summary>
        public static readonly string RequestDataKey = "__requestData";

        /// <summary>
        /// 获取或设置 参数是否加密
        /// </summary>
        public bool IsEncryptDatas { get; set; }

        /// <summary>
        /// 获取或设置 参数是否解密成功
        /// </summary>
        public bool IsDecryptSucceed { get; set; }

        /// <summary>
        /// 获取或设置 参数解密后数据
        /// </summary>
        public IDictionary<string, string> DicDecryptDatas { get; set; }

        /// <summary>
        /// 获取或设置 参数解密异常对象
        /// </summary>
        public Exception DecryptException { get; set; }
    }
}
