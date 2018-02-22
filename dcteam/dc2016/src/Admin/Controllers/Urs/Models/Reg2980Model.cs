using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC2016.Admin.Controllers.Urs.Models
{
    /// <summary>
    /// 注册2980帐号参数实体
    /// </summary>
    public class Reg2980Model
    {
        public Reg2980Model()
        {
            this.mobile = string.Empty;
            this.password = string.Empty;
            this.account = string.Empty;
            this.name = string.Empty;
            this.idnumber = string.Empty;
        }

        /// <summary>
        ///手机号码
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 帐号
        /// </summary>
        public string account { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string idnumber { get; set; }
    }
}
