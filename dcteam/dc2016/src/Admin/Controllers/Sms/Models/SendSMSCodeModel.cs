using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Controllers.Sms.Models
{
    public class SendSMSCodeModel
    {
        /// <summary>
        /// 产品代号（由消息中心提供）
        /// </summary>
        public string pname { get; set; }

        /// <summary>
        /// 手机号码（群发多个号码，用英文逗号隔开，其中验证码、语音验证码、国际短信不支持群发）
        /// </summary>
        public string telnumbers { get; set; }

        /// <summary>
        /// 短信类型（1验证码 2国际短信 3语音短信 4单向互动短信 5双向互动短信 6游戏内招募短信 7通知短信 8内部报警短信 11礼包短信）
        /// </summary>
        public int type { get; set; }
    }
}
