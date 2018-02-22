using Admin.Controllers.Models;
using dywebsdk.Extension;
using dywebsdk.ICC;
using dywebsdk.Models;
using IF2017.Admin.Controllers.Common;
using IF2017.Admin.Filter;
using IF2017.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;

namespace IF2017.Admin.Controllers
{
    [Route("common")]
    [HideInDocs]
    public class SmsController : BaseController<SmsController>
    {
        public SmsController(ILogger<SmsController> logger) : base(logger) { }
        /// <summary>
        /// 1是国内短信，2是国际短信
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string smsMsg(int type, string code)
        {
            if (type == 1)
            {
                return "您的验证码是" + code + "，感谢您使用多益网络手机认证功能！";
            }
            else if (type == 2)
            {
                return "[Duoyi Network] Your Verification Code is: " + code;
            }
            return "";
        }
        #region 测试
        [HttpGet(@"test/sendsmscode")]
        public IActionResult test_sendSMSCode([FromQuery]SendSMSCodeModel model)
        {
#if DEBUG

# endif
            return func_sendSMSCode(model);
        }
        #endregion

        /// <summary>
        /// 获取短信验证码
        /// </summary>
        /// <param name="model">请求参数</param>
        /// <returns>执行结果</returns>
        [HttpGet(@"sendsmscode")]
        public IActionResult func_sendSMSCode(SendSMSCodeModel model)
        {
            if (model.telnumbers.IsNullOrEmpty() || model.telnumbers.Length < 7)
            {
                return this.FuncResult(new APIReturn(10000, $"参数错误{model.telnumbers}"));
            }
            if (model.pname.IsNullOrEmpty())
            {
                return this.FuncResult(new APIReturn(10000, $"参数错误{model.pname}"));
            }
            if (model.type < 1)
                model.type = 1;
            SendSMSModel smsModel = new SendSMSModel();
            smsModel.pname = model.pname ?? string.Empty;
            smsModel.telnumbers = model.telnumbers ?? string.Empty;
            smsModel.type = model.type;

            Random r = new Random();
            string code = r.Next(111111, 999999).ToString();
            smsModel.message = smsMsg(model.type, code);
            smsModel.validcode = code;

            return func_sendSMS(smsModel);
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="model">请求参数</param>
        /// <returns>执行结果</returns>
        [HttpGet(@"sendsms")]
        public IActionResult func_sendSMS(SendSMSModel model)
        {
            if (model.pname.IsNullOrEmpty())
            {
                return this.FuncResult(new APIReturn(10000, $"参数错误(pname)"));
            }
            if (model.message.IsNullOrEmpty() && (model.type == 1 || model.type == 2))
            {
                return this.FuncResult(new APIReturn(10000, $"参数错误(message)"));
            }
            else if (model.type == 3)
            {
                model.message = model.validcode;
            }
            if (model.type == 0)
            {
                return this.FuncResult(new APIReturn(10000, $"参数错误(type)"));
            }
            if ((model.type == 1 || model.type == 4) && model.validcode.IsNullOrEmpty())
            {
                return this.FuncResult(new APIReturn(10000, $"参数错误(validcode)"));
            }

            /*
            string apm_enc = string.Format("act={0}&pname={1}&message={2}&telnumbers={3}&type={4}&validcode={5}&displaynum={6}&server={7}&userid_from={8}&userid_to={9}", "sendsms", model.pname, model.message, model.telnumbers, model.type, model.validcode, model.displaynum, model.server, model.userid_from, model.userid_to);
            string result = string.Empty;
            result = this.SendToICC(apm_enc);
            */

            SmsType smsType = (SmsType)model.type;
            CallResult callResult = ESClient.SendSms("if2017", model.pname, model.telnumbers, model.message, smsType, model.validcode);
            if (callResult.Code == 0)
            {
                Hashtable table = new Hashtable();
                //1验证码 2国际短信 3语音短信 4单向互动短信 5双向互动短信 6游戏内招募短信 7通知短信 8内部报警短信 11礼包短信
                if (model.type == 1 || model.type == 2|| model.type==3)
                {
                    string key = Guid.NewGuid().ToString("N");
                    RedisHelper.Set(key, model.validcode, 5 * 60);

                    table["smskey"] = key;
                }
                return this.FuncResult(new APIReturn(0, "发送成功", table));
            }
            else
            {
                return this.FuncResult(new APIReturn(10130, $"发送失败({callResult.Code},{callResult.Message})"));
            }
        }

        /// <summary>
        /// 校验短信验证码
        /// </summary>
        /// <param name="smskey">获取短信验证码返回的KEY</param>
        /// <param name="smscode">短信验证码</param>
        /// <returns>执行结果</returns>
        [HttpGet(@"checksms")]
        public IActionResult func_checksms(string smskey, string smscode)
        {
            return this.FuncResult(CheckSMS(smskey, smscode));
        }

        /// <summary>
        /// 校验短信验证码
        /// </summary>
        /// <param name="smskey">获取短信验证码返回的KEY</param>
        /// <param name="smscode">短信验证码</param>
        /// <returns>执行结果</returns>
        public static APIReturn CheckSMS(string smskey, string smscode)
        {
            if (smskey.IsNullOrEmpty())
            {
                return new APIReturn(10000, $"参数错误(smskey)");
            }
            if (smscode.IsNullOrEmpty())
            {
                return new APIReturn(10000, $"参数错误(smscode)");
            }
            string code = RedisHelper.Get(smskey);
            if (code.IsNullOrEmpty())
            {
                return new APIReturn(10132, $"验证码KEY错误或验证码已过期，有效时间5分钟");
            }
            if (code != smscode)
            {
                return new APIReturn(10131, $"短信验证码错误");
            }

            RedisHelper.Remove(smskey);
            return new APIReturn(0, "验证通过");
        }
    }
}
