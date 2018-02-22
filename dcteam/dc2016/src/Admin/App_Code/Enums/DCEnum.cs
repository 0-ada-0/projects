using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC2016.Admin.Enums
{
    public enum DCProdTypes
    {
        /// <summary>
        /// 梦想
        /// </summary>
        MX,
        /// <summary>
        /// 神武
        /// </summary>
        SW,
        /// <summary>
        /// 帝王
        /// </summary>
        DW,
        /// <summary>
        /// 逍遥
        /// </summary>
        XY,
        /// <summary>
        /// 游戏角色信息
        /// </summary>
        XIYI,
        /// <summary>
        /// 战盟帐号信息
        /// </summary>
        URS
    }

    public enum DCMethodTypes
    {
        /// <summary>
        /// 读操作
        /// </summary>
        R,
        /// <summary>
        /// 写操作
        /// </summary>
        W
    }
}
