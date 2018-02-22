using DC2016.Admin.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DC2016.Admin.DC2;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using DC2016.Admin.Controllers.Common;
using DC2016.BLL;
using DC2016.Model;
using dywebsdk.Extension;
using dywebsdk.Common;
using DC2016.Admin.App_Code.WNT;
using DC2016.Admin.Configs;
using DC2016.Admin.Controllers.Activity.Mode;

namespace DC2016.Admin.Controllers.Activity
{
    [Route("dc2/activity")]
    public partial class ActivityController : BaseController<ActivityController>
    {
        private ViewRenderService _viewRender;
        public ActivityController(ILogger<ActivityController> logger, ViewRenderService viewRender) : base(logger)
        {
            this._viewRender = viewRender;
        }

        #region 抽奖信息
        /// <summary>
        /// 记录抽奖信息
        /// </summary>
        /// <param name="tel">手机号码（muset）</param>
        /// <param name="gate">产品代号（must）</param>
        /// <param name="rewarditem">奖励编号（must）</param>
        /// <param name="activeid">活动编号（must）</param>
        /// <returns></returns>
        [HttpGet(@"record_raffleinfo")]
        public IActionResult func_record_raffleinfo(string tel, string gate, int rewarditem, string activeid)
        {
            if (tel.IsNullOrEmpty())
            {
                return this.FuncResult(new APIReturn(10000, $"参数不正确tel={tel}"));
            }
            if (gate.IsNullOrEmpty() || gate.Length > 8)
                return this.FuncResult(new APIReturn(10000, $"参数不正确gate={gate}"));
            if (rewarditem < 10000)
                return this.FuncResult(new APIReturn(10000, $"参数不正确rewarditem={rewarditem}"));
            if (activeid.IsNullOrEmpty())
                return this.FuncResult(new APIReturn(10000, $"参数不正确activeid={activeid}"));
            BaishiRewardInfo raffleinfo = BaishiReward.hareInfo(gate,tel, activeid);
            if (raffleinfo == null)
            {
                raffleinfo = new BaishiRewardInfo();
                raffleinfo.Activeid = activeid;
                raffleinfo.Gate = gate;
                raffleinfo.Tel = tel;
                raffleinfo.Rewarditem = rewarditem;
                raffleinfo.Time = DateTime.Now;
                raffleinfo = BaishiReward.Insert(raffleinfo);
                if (raffleinfo == null)
                    return this.FuncResult(new APIReturn(19600, "记录数据失败"));
                else
                    return this.FuncResult(new APIReturn(0, "记录信息成功"));
            }
            else
            {
                Hashtable ht = new Hashtable();
                ht["tel"] = raffleinfo.Tel;
                ht["time"] = raffleinfo.Time;
                ht["rewarditem"] = raffleinfo.Rewarditem;
                return this.FuncResult(new APIReturn(19601, "已有抽奖记录", ht));
            }

        }

        /// <summary>
        /// 获取抽奖信息
        /// </summary>
        /// <param name="tel">手机号码（muset）</param>
        /// <param name="gate">产品代号（must）</param>
        /// <param name="activeid">活动编号（must）</param>
        /// <returns></returns>
        [HttpGet(@"get_raffleinfo")]
        public IActionResult func_get_raffleinfo(string tel, string gate,string activeid)
        {
            if (tel.IsNullOrEmpty())
            {
                return this.FuncResult(new APIReturn(10205, $"手机号格式错误tel={tel}"));
            }
            if (gate.IsNullOrEmpty() || gate.Length > 8)
                return this.FuncResult(new APIReturn(10000, $"参数不正确gate={gate}"));
            if (activeid.IsNullOrEmpty())
                return this.FuncResult(new APIReturn(10000, $"参数不正确activeid={activeid}"));
            BaishiRewardInfo raffleinfo = BaishiReward.hareInfo(gate, tel, activeid);
            if (raffleinfo == null)
            {
                return this.FuncResult(new APIReturn(19602, "没有记录"));
            }
            else if (raffleinfo.State==1)
            {
                return this.FuncResult(new APIReturn(19603, "奖励已领取"));
            }
            else
            {
                raffleinfo.State = 1;
                BaishiReward.Update(raffleinfo);

                Hashtable ht = new Hashtable();
                ht["tel"] = raffleinfo.Tel;
                ht["time"] = raffleinfo.Time;
                ht["rewarditem"] = raffleinfo.Rewarditem;
                return this.FuncResult(new APIReturn(0, "已有抽奖记录", ht)); //分享记录失败
            }
        }

        #endregion

        #region 分享功能
        /// <summary>
        /// 记录分享点击信息
        /// </summary>
        /// <param name="guid">设备信息相关的json数据</param>
        /// <param name="shareid">分享id</param>
        /// <returns></returns>
        [HttpPost(@"record_shareinfo")]
        public IActionResult func_record_shareinfo(string guid, string shareid)
        {
            if (string.IsNullOrEmpty(guid) || shareid.IsNullOrEmpty())
            {
                Hashtable htb = new Hashtable();
                htb.Add("guid", guid);
                htb.Add("shareid", shareid);
                return this.FuncResult(new APIReturn(10000, "参数错误", htb));
            }
            string deviceinfo = guid;

            ShareGuidInfo shareguidinfo = new ShareGuidInfo(deviceinfo);
            guid = shareguidinfo.getGuid();

            ShareInfo mode = Share.hasShareInfoWhithSahreId(shareid);
            if (mode == null)
            {
               return this.FuncResult(new APIReturn(1070033, "分享id不存在 - " + shareid));               
            }

            ShareInfo state = Share.hasShareInfo(guid, mode.Gate);
            if (state != null && state.State == 1)//已经有绑定等级使用的记录
                return this.FuncResult(new APIReturn(1070032, "已经有绑定等级使用的记录 "));

            ShareInfo s = Share.hasShareInfo(guid,mode.Number??-1,mode.Userid??-1,mode.Gate,mode.Server??-1);
            if (s == null)//表示没有点击记录,记录完数据后请求wnt推送
            {
                if (mode.Guid == null)
                {
                    mode.Guid = guid;
                    mode.Time = DateTime.Now;
                    mode.Extends = deviceinfo;
                    mode.Ip = shareguidinfo.getIP();
                    Share.Update(mode);
                }
                else
                {
                    ShareInfo share = new ShareInfo();
                    share.Guid = guid;
                    share.Userid = mode.Userid;
                    share.Number = mode.Userid;
                    share.Gate = mode.Gate;
                    share.Time = DateTime.Now;
                    share.Server = mode.Server;
                    share.Shareid = shareid;
                    ShareInfo shareinfo = Share.Insert(share);
                    if (shareinfo==null)
                        return this.FuncResult(new APIReturn(1070031, "分享记录失败"));//分享记录失败
                }

                WNTClass wnt = new WNTClass("notifyacct", mode.Server + "", "m2jtsharereward", mode.Number ?? -1, mode.Userid ?? -1, "1","true");
                ReturnMsgWnt ret = wnt.getResult();
                return this.FuncResult(new APIReturn(0, "分享记录成功"));//分享记录成功
            }
            else//表示有点击记录，更新该记录的点击时间
            {
                s.Time = DateTime.Now;
                Share.Update(s);
                return this.FuncResult(new APIReturn(1070032, "已经有绑定等级使用的记录 "));
            }
        }
        /// <summary>
        /// 获取分享者的信息
        /// </summary>
        /// <param name="guid">设备信息相关的json数据</param>
        /// <param name="number">点击分享者的帐号</param>
        /// <param name="userid">点击分享者的角色id</param>
        /// <param name="gate">游戏产品代号</param>
        /// <returns></returns>
        [HttpPost(@"get_shareinfo")]
        public IActionResult func_get_shareinfo(string guid, int number, int userid, string gate)
        {
            if (string.IsNullOrEmpty(guid) || number < 10000 || userid < 10000 || string.IsNullOrEmpty(gate))
            {
                Hashtable htb = new Hashtable();
                htb.Add("gate", gate);
                htb.Add("number", number);
                htb.Add("guid", guid);
                htb.Add("userid", userid);
                return this.FuncResult(new APIReturn(10000, "参数错误", htb));
            }
            string deviceinfo = guid;

            ShareGuidInfo shareguidinfo = new ShareGuidInfo(deviceinfo);
            guid = shareguidinfo.getGuid();

            ShareInfo mode = Share.hasShareInfo(guid, gate);
            if (mode == null)
                return this.FuncResult(new APIReturn(1070030, "没有分享点击"));//没有查到分享点击
            else
            {
                //1、更新分享使用者的userid，number，还有状态
                //2、将查询到的分享者userid，number返回给游戏
                mode.Used_userid = userid;
                mode.Used_number = number;
                mode.State = 1;
                Share.Update(mode);
                Hashtable h = new Hashtable();
                h["number"] = mode.Number;
                h["userid"] = mode.Userid;
                h["server"] = mode.Server;
                return this.FuncResult(new APIReturn(0, "", h));
            }
        }
        /// <summary>
        /// 获取分享id
        /// </summary>
        /// <param name="number"></param>
        /// <param name="userid"></param>
        /// <param name="gate"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        [HttpGet(@"get_shareurl")]
        public IActionResult func_getshareid(int number,int userid,string gate,int server)
        {
            if (number < 10000 || userid < 10000 || string.IsNullOrEmpty(gate) || server <= 0)
            {
                Hashtable htb = new Hashtable();
                htb.Add("gate", gate);
                htb.Add("number", number);
                htb.Add("server", server);
                htb.Add("userid", userid);
                return this.FuncResult(new APIReturn(10000, "参数错误", htb));
            }
            ShareInfo shareinfo = Share.hasShareInfo(number,userid,gate,server);
            if (shareinfo==null)
            {
                string shareid = DC2016.Admin.DC2.Lib.D2lib.Lib_BuildCdKey(Share.getCount(), 6);

                ShareInfo share = new ShareInfo();
                share.Gate = gate;
                share.Number = number;
                share.Server = server;
                share.Userid = userid;
                share.Shareid = shareid;
                share = Share.Insert(share);
                if (share == null)
                    return this.FuncResult(new APIReturn(10031, "获取分享id失败")); //获取分享id失败
                else
                {
                    Hashtable htb = new Hashtable();
                    htb.Add("shareid", DC2Conf.getshareUrl(gate)+ shareid);
                    return this.FuncResult(new APIReturn(0, "获取分享id成功", htb)); 
                }
            }
            else
            {
                Hashtable htb = new Hashtable();
                htb.Add("shareid", DC2Conf.getshareUrl(gate) + shareinfo.Shareid);
                return this.FuncResult(new APIReturn(0, "获取分享id成功", htb));
            }
        }

        #endregion
    }
}
