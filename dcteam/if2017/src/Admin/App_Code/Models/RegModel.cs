using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Controllers.Models
{
    /// <summary>
    /// 注册普通帐号参数实体
    /// </summary>
    public class RegModel
    {
        public RegModel()
        {
            this.email = string.Empty;
            this.pass = string.Empty;
            this.encrypt = string.Empty;
            this.tname = string.Empty;
            this.idcard = string.Empty;
            this.gatesrc = string.Empty;
            this.pstype = string.Empty;
            this.ip = string.Empty;
            this.vcode = string.Empty;
            this.vregval = string.Empty;
            this.language = string.Empty;
            this.extargs = string.Empty;
        }
        public string email { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string pass { get; set; }

        /// <summary>
        /// 密码是否加密
        /// </summary>
        public string encrypt { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string tname { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string idcard { get; set; }

        /// <summary>
        /// 游戏代号
        /// </summary>
        public string gatesrc { get; set; }

        /// <summary>
        /// 默认game
        /// </summary>
        public string pstype { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public int qq { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string ip { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string vcode { get; set; }

        /// <summary>
        /// 需要验证码时提供
        /// </summary>
        public string vregval { get; set; }

        /// <summary>
        /// 页面推广ID
        /// </summary>
        public int adsid { get; set; }

        /// <summary>
        /// 桌面推广ID
        /// </summary>
        public int tgaccount { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        public string language { get; set; }

        /// <summary>
        /// 扩展字段
        /// </summary>
        public string extargs { get; set; }
    }
}
