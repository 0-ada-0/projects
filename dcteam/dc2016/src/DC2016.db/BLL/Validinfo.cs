using DC2016.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC2016.BLL
{
    public partial  class Validinfo
    {
        public static ValidinfoInfo hasRecord(string tel, string message)
        {
            return Select.WhereMobile(tel).WhereCode(message).OrderBy("addtime desc").ToOne();
        }
    }
}
