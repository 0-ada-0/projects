using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IF2017.Admin.Controllers.DK.Models
{
    public enum EWAY
    {
        EBilling = 3,
        JCard = 8,
        汇付 = 1,
        神州付 = 2,
        微信 = 0x16,
        银联 = 5,
        支付宝 = 4
    }
}
