using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC2016.Admin.Common
{
    public class ReturnMsg
    {
        public int Code { get; protected set; }

        public string Message { get; protected set; }

        public bool Succeed { get; protected set; }

        public ReturnMsg()
        {

        }

        public ReturnMsg(int code)
        {
            this.Code = code;
        }

        public ReturnMsg(int code, string message)
        {
            this.Message = message;
            this.Code = code;
        }

        public ReturnMsg(bool isSucceed, string message, int code)
        {
            this.Succeed = isSucceed;
            this.Message = message;
            this.Code = code;
        }
    }
}
