using DC2016.BLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC2016.Model
{
    public partial class ValidinfoInfo
    {
        public bool Is待验证
        {
            get { return this.State == (int)ESTATE.待验证; }
        }

        public bool Is已验证
        {
            get { return this.State == (int)ESTATE.已验证; }
        }
    }
}
