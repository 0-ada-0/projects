using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Controllers.Sms.Models
{
    public class SendSMSModel : SendSMSCodeModel
    {
        /// <summary>
        /// 发送内容（中英文都算一个字符）
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 只能是字母+数字（验证码、语音验证码需要此参数，此参数是辅助作用，在某些时刻message没法到达时，短信中心会想办法把validcode发送）
        /// </summary>
        public string validcode { get; set; }

        /// <summary>
        /// 语音验证码的显号（可不传，有默认值）
        /// </summary>
        public string displaynum { get; set; }

        /// <summary>
        /// 服务器编号（双向互动短信才需要此参数）
        /// </summary>
        public string server { get; set; }

        /// <summary>
        /// 短信发送人ID（双向互动短信才需要此参数）
        /// </summary>
        public string userid_from { get; set; }

        /// <summary>
        /// 短信收信人ID（双向互动短信才需要此参数）
        /// </summary>
        public string userid_to { get; set; }
    }
}
