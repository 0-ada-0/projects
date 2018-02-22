using DC2016.Admin.Common;
using DC2016.Admin.Configs;
using DC2016.Admin.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC2016.Admin.DC
{
    public class DCRequest
    {
        public string ProdType { get; private set; }

        public string MethodType { get; private set; }

        public DCRequest(string prodType, DCMethodTypes methodType)
        {
            this.ProdType = prodType;
            this.MethodType = methodType.ToString();
        }

        public DCRequest(DCProdTypes prodType, DCMethodTypes methodType)
        {
            this.ProdType = prodType.ToString();
            this.MethodType = methodType.ToString();
        }

        private bool IsNewGame(string gate)
        {
            string[] oldgames = { "mx", "xy", "dw", "sw" };
            return Array.IndexOf(oldgames, gate.ToLower()) == -1 && gate != "urs";
        }

        #region 返回 ReturnMsgDC

        public ReturnMsgDC ExecRequest(string fileName, object[] values)
        {
            if (values == null || values.Length % 2 != 0)
            {
                throw new ArgumentException("DC请求参数错误");
            }
            Hashtable htbParams = new Hashtable();
            for (int i = 0; i < values.Length; i += 2)
            {
                htbParams.Add(values[i], values[i + 1]);
            }
            return this.ExecRequest(fileName, htbParams);
        }

        public ReturnMsgDC ExecRequest(string targetFile, Hashtable htbParams)
        {
            string baseUrl = DCConf.GetDCUrl(this.ProdType, this.MethodType);
            string url = $"{ baseUrl.TrimEnd('/')}/{targetFile}";
            if (htbParams != null && htbParams.Count > 0)
            {
                List<string> lstValues = new List<string>();
                foreach (string key in htbParams.Keys)
                {
                    lstValues.Add($"{key}={htbParams[key]}");
                }
                url = $"{url}?{ string.Join("&", lstValues.ToArray())}";
            }
            HttpComm comm = new HttpComm();
            Dictionary<string, string> dicRequestParams = new Dictionary<string, string>();
            dicRequestParams.Add("url", url);
            HttpResult result = comm.ExecWebRequest(dicRequestParams);
            if (result.IsSucceed)
            {
                return this.ResolveD2(result.Content);
            }
            else
            {
                throw new Exception("请求DC发生异常", result.HttpException);
            }
        }

        private ReturnMsgDC ResolveD2(string rcstr)
        {
            int index = rcstr.IndexOf('|');
            int num2 = rcstr.IndexOf(',');
            if ((index == -1) && (num2 == -1))
            {
                return new ReturnMsgDC(true, string.Empty, this.TryGetCode(rcstr));
            }
            if ((index == -1) && (num2 != -1))
            {
                return ResolveD2Compatible(rcstr);
            }
            int code = this.TryGetCode(rcstr.Substring(0, index));
            string[] strArray = rcstr.Remove(0, index + 1).Split("&=".ToCharArray());
            if ((strArray.Length % 2) != 0)
            {
                return ResolveD2Compatible(rcstr);
            }
            CLSHashtable data = new CLSHashtable();
            for (int i = 0; i < strArray.Length; i += 2)
            {
                string str3 = strArray[i];
                string str4 = strArray[i + 1];
                data[str3] = str4;
            }
            return new ReturnMsgDC(true, string.Empty, code, data);
        }

        private ReturnMsgDC ResolveD2Compatible(string rcstr)
        {
            rcstr = rcstr.Replace("\r\n", "\n");
            rcstr = rcstr.Replace("\r", "\n");
            string[] strArray = rcstr.Split(",\n".ToCharArray());
            int code = this.TryGetCode(strArray[0]);
            if (code < 0)
            {
                return new ReturnMsgDC(false);
            }
            CLSHashtable data = new CLSHashtable();
            for (int i = 1; i < strArray.Length; i++)
            {
                string str = string.Format("item{0}", i);
                string str2 = strArray[i];
                data[str] = str2;
            }
            return new ReturnMsgDC(true, string.Empty, code, data);
        }

        private int TryGetCode(string codeStr)
        {
            int code = -1;
            int.TryParse(codeStr, out code);
            return code;
        }

        #endregion

        #region 返回 ReturnListMsgDC

        public ReturnMsgListDC ExecRequestList(string fileName, object[] values)
        {
            if (values == null || values.Length % 2 != 0)
            {
                throw new ArgumentException("DC请求参数错误");
            }
            Hashtable htbParams = new Hashtable();
            for (int i = 0; i < values.Length; i += 2)
            {
                htbParams.Add(values[i], values[i + 1]);
            }
            return this.ExecRequestList(fileName, htbParams);
        }

        public ReturnMsgListDC ExecRequestList(string targetFile, Hashtable htbParams)
        {
            string baseUrl = DCConf.GetDCUrl(this.ProdType, this.MethodType);
            string url = $"{ baseUrl.TrimEnd('/')}/{targetFile}";
            if (htbParams != null && htbParams.Count > 0)
            {
                List<string> lstValues = new List<string>();
                foreach (string key in htbParams.Keys)
                {
                    lstValues.Add($"{key}={htbParams[key]}");
                }
                url = $"{url}?{ string.Join("&", lstValues.ToArray())}";
            }
            HttpComm comm = new HttpComm();
            Dictionary<string, string> dicRequestParams = new Dictionary<string, string>();
            dicRequestParams.Add("url", url);
            HttpResult result = comm.ExecWebRequest(dicRequestParams);
            if (result.IsSucceed)
            {
                string[] strArray = result.Content.Split("\n".ToCharArray());
                int code = -1;
                List<CLSObject[]> data = new List<CLSObject[]>();
                for (int i = 0; i < strArray.Length; i++)
                {
                    string[] strArray2 = strArray[i].Split(",".ToCharArray());
                    if ((i == 0) && (strArray2.Length >= 1))
                    {
                        code = int.Parse(strArray2[0]);
                    }
                    CLSObject[] item = new CLSObject[strArray2.Length];
                    for (int j = 0; j < strArray2.Length; j++)
                    {
                        item[j] = strArray2[j];
                    }
                    data.Add(item);
                }
                return new DC.ReturnMsgListDC(true, result.Content, code, data);
            }
            else
            {
                throw new Exception("请求DC发生异常", result.HttpException);
            }
        }

        #endregion
    }
}
