using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DC2016.Admin.App_Code.WNT
{
    public class WNTClass
    {
        private string _action;
        public string action { get { return _action; } set { this._action = value; } }
        private string _server;
        public string server { get { return _server; } set { this._server = value; } }
        private string _op;
        public string op { get { return _op; } set { this._op = value; } }
        private int _number;
        private int number { get { return _number; } set { this._number = value; } }
        private int _userid;
        private int userid { get { return _userid; } set { this._userid = value; } }
        private string _type;
        private string type { get { return _type; } set { this._type = value; } }
        private string noWait { get; set; }

    public WNTClass()
        {
        }
        public WNTClass(string action, string server, string op, int number, int userid, string type,string noWait)
        {
            this.action = action;
            this.server = server;
            this.op = op;
            this.number = number;
            this.userid = userid;
            this.type = type;
            this.noWait = noWait;
        }

        private string psotjson()
        {
            string rmsg = "";
            Hashtable ht = new Hashtable();

            ht["action"] = this.action;
            ht["server"] = this.server;
            ht["noWait"] = this.noWait;

            Hashtable msg = new Hashtable();
            msg["op"] = this.op;
            msg["number"] = this.number;

            Hashtable parm = new Hashtable();
            parm["userid"] = this.userid;
            parm["type"] = this.type;
            msg["params"] = parm;

            ht["msg"] = msg;
            rmsg = JsonConvert.SerializeObject(ht);
            return rmsg;
        } 

        public ReturnMsgWnt getResult()
        {
            ReturnMsgWnt ret =  PWNTInvoker.InvokeHttp("message", psotjson());
            return ret;
        }
        
      }
    }
