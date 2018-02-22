using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC2016.Admin.Controllers.Activity
{
    public partial class ActivityController
    {
        /// <summary>
        /// 记录抽奖信息测试
        /// </summary>
        /// <param name="tel">手机号码（muset）</param>
        /// <param name="gate">产品代号（must）</param>
        /// <param name="rewarditem">奖励编号（must）</param>
        /// <param name="activeid">活动编号（must）</param>
        /// <returns></returns>
        [HttpGet(@"test/record_raffleinfo")]
        public IActionResult test_record_raffleinfo([FromQuery]string tel, [FromQuery]string gate, [FromQuery]int rewarditem, [FromQuery]string activeid)
        {
            return func_record_raffleinfo(tel,gate, rewarditem,activeid);
        }
        /// <summary>
        /// 获取抽奖信息
        /// </summary>
        /// <param name="tel">手机号码（muset）</param>
        /// <param name="gate">产品代号（must）</param>
        /// <param name="activeid">活动编号（must）</param>
        /// <returns></returns>
        [HttpGet(@"test/get_raffleinfo")]
        public IActionResult test_get_raffleinfo([FromQuery]string tel, [FromQuery]string gate, [FromQuery]string activeid)
        {
            return func_get_raffleinfo(tel, gate, activeid);
        }
        /// <summary>
        /// 记录分享点击信息
        /// </summary>
        /// <param name="guid">设备信息相关的json数据</param>
        /// <param name="shareid">分享id</param>
        /// <returns></returns>
        [HttpPost(@"test/record_shareinfo")]
        public IActionResult test_record_shareinfo([FromQuery]string guid, [FromQuery]string shareid)
        {
           return  func_record_shareinfo(guid,shareid);
        }
        /// <summary>
        /// 获取分享url
        /// </summary>
        /// <param name="number">帐号number</param>
        /// <param name="userid">角色id</param>
        /// <param name="gate">产品编号</param>
        /// <param name="server">服务器编号</param>
        /// <returns></returns>
        [HttpGet(@"test/get_shareurl")]
        public IActionResult test_getshareid([FromQuery]int number, [FromQuery]int userid, [FromQuery]string gate, [FromQuery]int server)
        {
            return func_getshareid(number, userid, gate, server);
        }
        /// <summary>
        /// 获取分享者的信息
        /// </summary>
        /// <param name="guid">设备信息相关的json数据</param>
        /// <param name="number">点击分享者的帐号</param>
        /// <param name="userid">点击分享者的角色id</param>
        /// <param name="gate">游戏产品代号</param>
        /// <returns></returns>
        [HttpPost(@"test/get_shareinfo")]
        public IActionResult test_get_shareinfo([FromQuery]string guid, [FromQuery]int number, [FromQuery]int userid, [FromQuery]string gate)
        {
            return func_get_shareinfo(guid,number,userid,gate);
        }
     }
}
