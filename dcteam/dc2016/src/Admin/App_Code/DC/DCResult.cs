using dywebsdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dywebsdk.Extension;

namespace DC2016.Admin.DC
{
    #region DC返回值对象

    /// <summary>
    /// 整形类DC返回值
    /// </summary>
    public class IntDCValue
    {
        public int Code { get; private set; }

        public IntDCValue(string content)
        {
            this.Code = int.Parse(content);
        }
    }

    /// <summary>
    /// 字典类DC返回值
    /// 格式：code|key1=value1&key2=value2&...
    /// </summary>
    public class DicDCValue : Dictionary<string, string>
    {
        public int Code { get; private set; }

        public string Content { get; private set; }

        public DicDCValue(string content)
        {
            this.Code = -1;
            this.Content = content;

            char[] separator = { '|', ',' };
            string[] items = content.Split(separator);
            this.Code = int.Parse(items[0]);
            if (items.Length > 1 && items[1].IsNotNullOrEmpty())
            {
                string[] values = items[1].Split('&');
                foreach (var item in values)
                {
                    if (!item.Contains('=')) continue;

                    string[] pairs = item.Split('=');
                    this.Add(pairs[0], pairs[1]);
                }
            }
        }
    }

    /// <summary>
    /// 集合类DC返回值
    /// 格式：code,记录数\n值1,值2,...\n值1,值2,...，
    /// 例如：http://219.132.195.247:11011/urs/R/active_getlist.php 
    /// </summary>
    public class ListDCValue
    {
        public int Code { get; private set; }

        public string Content { get; private set; }

        public List<string[]> ListDatas { get; private set; }

        public ListDCValue(string content)
        {
            this.Code = -1;
            this.Content = content;
            this.ListDatas = new List<string[]>();

            char separator = '\n';
            content = content.Replace("\r\n", separator.ToString());
            content = content.Replace("\r", separator.ToString());
            string[] items = content.Split(separator);
            for (int i = 0; i < items.Length; i++)
            {
                string[] values = items[i].Split(',');
                if (i == 0)
                {
                    this.Code = int.Parse(values[0]);
                    continue;
                }
                this.ListDatas.Add(values);
            }
        }
    }

    #endregion

    public class DCResult
    {
        public CallResult Result { get; set; }

        public DCResult(CallResult result)
        {
            this.Result = result;
        }

        public IntDCValue GetIntDCValue()
        {
            try
            {
                return new IntDCValue(this.Result.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"解释DC返回值失败(GetDicDCValue,{this.Result.Source})", ex);
            }
        }

        public DicDCValue GetDicDCValue()
        {
            try
            {
                return new DicDCValue(this.Result.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"解释DC返回值失败(GetDicDCValue,{this.Result.Source})", ex);
            }
        }

        public ListDCValue GetListDCValue()
        {
            try
            {
                return new ListDCValue(this.Result.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"解释DC返回值失败(GetListDCValue,{this.Result.Source})", ex);
            }
        }
    }
}
