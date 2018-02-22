using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC2016.BLL.Enums
{
    public enum EGST
    {
        未发送 = 1,
        已发送 = 2,
        正在修改 = 3,
        已完成 = 4,
        异常 = 5,
        修改失败_密码相同 = 6,
        修改失败 = 7,
    }
}
