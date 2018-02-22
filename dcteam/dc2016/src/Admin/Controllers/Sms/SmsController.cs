using Admin.Controllers.Sms.Models;
using dywebsdk.Extension;
using dywebsdk.ICC;
using dywebsdk.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using Newtonsoft.Json;
using DC2016.Admin.Controllers.Base;
using DC2016.Admin.Controllers.Common;
using DC2016.BLL;
using DC2016.Model;
using DC2016.BLL.Enums;

namespace DC2016.Admin.Controllers.Sms
{
    [Route("common")]
    public class SmsController : BaseController<SmsController>
    {
        public SmsController(ILogger<SmsController> logger) : base(logger) { }

        #region 测试
        [HttpGet(@"test/sendsmscode")]
        public IActionResult test_sendSMSCode([FromQuery]SendSMSCodeModel model)
        {
            return func_sendSMSCode(model);
        }
        #endregion
        #region 上行短信相关
        [HttpPost(@"smscallback")]
        public IActionResult func_smscallback(string message, string telnumber, int port)
        {
            ValidinfoInfo mode = Validinfo.hasRecord(telnumber, message);
            if (mode != null)
            {
                if (!mode.Is待验证) return this.FuncResult(new APIReturn(10000, "验证码已验证或者过期"));
                mode.State = (int)ESTATE.已验证;
                mode.Updatetime = DateTime.Now;
                int i = Validinfo.Update(mode);
                if(i>0)
                    return this.FuncResult(new APIReturn(0, "Ok"));
                else
                    return this.FuncResult(new APIReturn(10000, "WRONG"));
            }
            else
                return this.FuncResult(new APIReturn(10000, "WRONG"));
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
            SendSMSModel smsModel = new SendSMSModel();
            smsModel.pname = model.pname ?? string.Empty;
            smsModel.telnumbers = model.telnumbers ?? string.Empty;
            smsModel.type = model.type;

            Random r = new Random();
            string code = r.Next(111111, 999999).ToString();
            smsModel.message = code;
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
            if (model.message.IsNullOrEmpty())
            {
                return this.FuncResult(new APIReturn(10000, $"参数错误(message)"));
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
                //JToken jobject = JToken.Parse(callResult.Message);
                JToken jobject = JToken.Parse(callResult.Source.ToString());
                int code = jobject.Value<int>("Code");

                //1验证码 2国际短信 3语音短信 4单向互动短信 5双向互动短信 6游戏内招募短信 7通知短信 8内部报警短信 11礼包短信
                if (code == 0 && (model.type == 1 || model.type == 4))
                {
                    string key = Guid.NewGuid().ToString("N");
                    RedisHelper.Set(key, model.validcode, 5 * 60);

                    Hashtable table = new Hashtable();
                    table["smskey"] = key;
                    return this.FuncResult(new APIReturn(0, "发送成功", table));
                }
                else
                {
                    return this.FuncResult(new APIReturn(10130, $"发送失败({code},{ jobject.Value<string>("Message")})"));
                }
            }
            else
            {
                throw new Exception($"调用发送短信中控接口失败({callResult.Message})");
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
