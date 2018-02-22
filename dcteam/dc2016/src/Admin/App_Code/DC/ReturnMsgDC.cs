using DC2016.Admin.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC2016.Admin.DC
{
    public class ReturnMsgDC : ReturnMsg
    {
        public CLSHashtable CHT_DC { get; private set; }

        public ReturnMsgDC(bool isSucceed) : base(isSucceed, string.Empty, -9999)
        {
            this.CHT_DC = new CLSHashtable();
        }

        public ReturnMsgDC(bool isSucceed, string message, int code) : base(isSucceed, message, code)
        {
            this.CHT_DC = new CLSHashtable();
        }

        public ReturnMsgDC(bool isSucceed, string message, int code, CLSHashtable CHT_DC) : base(isSucceed, message, code)
        {
            this.CHT_DC = CHT_DC;
        }
    }
}
