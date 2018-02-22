using DC2016.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC2016.BLL
{
    public partial class BaishiReward
    {
        public static BaishiRewardInfo hareInfo(string gate, string tel,string actived)
        {
            return Select.WhereTel(tel).WhereGate(gate).WhereActiveid(actived).ToOne();
        }
    }
}
