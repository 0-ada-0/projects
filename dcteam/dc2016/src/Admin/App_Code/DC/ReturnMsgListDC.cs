using DC2016.Admin.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC2016.Admin.DC
{
    public class ReturnMsgListDC : ReturnMsg
    {
        public List<CLSObject[]> DCData { get; private set; }

        public ReturnMsgListDC(bool isSucceed) : base(isSucceed, string.Empty, -9999)
        {
            this.DCData = new List<CLSObject[]>();
        }

        public ReturnMsgListDC(bool isSucceed, string message, int code) : base(isSucceed, message, code)
        {
            this.DCData = new List<CLSObject[]>();
        }

        public ReturnMsgListDC(bool isSucceed, string message, int code, List<CLSObject[]> dcData) : base(isSucceed, message, code)
        {
            this.DCData = dcData;
        }
    }
}
