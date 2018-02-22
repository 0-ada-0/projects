using System;
using System.Collections;
using System.IO;
using System.Text;

namespace DC2016.Admin.DC
{
    public class CLSArrayList : ArrayList
    {
        private CLSHashtable cht;
        private string m_filename;
        private const string TOKEN_COLON = ":";
        private const string TOKEN_COMMA = ",";
        private const string TOKEN_CURLY_CLOSE = "}";
        private const string TOKEN_CURLY_OPEN = "{";
        private const string TOKEN_FALSE = "false";
        private const string TOKEN_NULL = "null";
        private const string TOKEN_SQUARED_CLOSE = "]";
        private const string TOKEN_SQUARED_OPEN = "[";
        private const string TOKEN_STRING = "\"";
        private const string TOKEN_TRUE = "true";

        public CLSArrayList()
        {
            this.cht = new CLSHashtable();
            this.m_filename = "";
            this.cht["array"] = this;
        }

        public CLSArrayList(ICollection c) : base(c)
        {
            this.cht = new CLSHashtable();
            this.m_filename = "";
            this.cht["array"] = this;
        }

        public override int Add(object value)
        {
            if (value is CLSVData)
            {
                return base.Add((CLSVData) value);
            }
            CLSVData data = new CLSVData(this.cht, value);
            return base.Add(data);
        }

        public int AddDict(params object[] args)
        {
            if ((args.Length % 2) > 0)
            {
                return -1;
            }
            CLSHashtable hashtable = new CLSHashtable();
            for (int i = 0; i < args.Length; i += 2)
            {
                hashtable.Add(args[i], args[i + 1]);
            }
            return this.Add(hashtable);
        }

        public CLSHashtable AddNodeCLSHashtable()
        {
            CLSHashtable hashtable = new CLSHashtable();
            base.Add(hashtable);
            return hashtable;
        }

        public CLSArrayList AddObjects(params object[] args)
        {
            foreach (object obj2 in args)
            {
                this.Add(new CLSVData(obj2));
            }
            return this;
        }

        public override void Clear()
        {
            base.Clear();
        }

        private string GetJsonKeyName(string key)
        {
            return string.Format("{0}{1}{2}", "\"", key, "\"");
        }

        private string GetJsonValueStr(CLSVData value)
        {
            StringBuilder builder = new StringBuilder();
            if (value.ValueIsNull)
            {
                builder.Append("null");
            }
            else if (CLSVData.Helper_CCDecimal(value.Value))
            {
                builder.Append(value.ValueDecimal);
            }
            else if (value.Value is bool)
            {
                builder.Append(this.GetJsonValueStr_Boolean(value.ValueBool));
            }
            else if (value.Value is string)
            {
                builder.Append(this.GetJsonValueStr_String(value.Value as string));
            }
            else if (value.Value is DateTime)
            {
                builder.Append(this.GetJsonValueStr_String(value.Value.ToString()));
            }
            else if (value.Value is CLSHashtable)
            {
                builder.Append(this.GetJsonValueStr_CLSHashtable(value.Value as CLSHashtable));
            }
            else if (value.Value is CLSArrayList)
            {
                builder.Append(this.GetJsonValueStr_Array(value.Value as CLSArrayList));
            }
            else if (value.Value is ArrayList)
            {
                CLSArrayList arr = new CLSArrayList();
                foreach (object obj2 in value.ValueArrayList)
                {
                    arr.Add(obj2);
                }
                builder.Append(this.GetJsonValueStr_Array(arr));
            }
            else if (value.Value is IEnumerable)
            {
                CLSArrayList list2 = new CLSArrayList();
                foreach (object obj3 in (IEnumerable) value.Value)
                {
                    list2.Add(obj3);
                }
                builder.Append(this.GetJsonValueStr_Array(list2));
            }
            else
            {
                builder.Append(this.GetJsonValueStr_Default(value.Value));
            }
            return builder.ToString();
        }

        private string GetJsonValueStr_Array(CLSArrayList arr)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            foreach (CLSVData data in arr)
            {
                builder.Append(this.GetJsonValueStr(data));
                builder.Append(",");
            }
            if (arr.Count > 0)
            {
                builder.Remove(builder.Length - 1, 1);
            }
            builder.Append("]");
            return builder.ToString();
        }

        private string GetJsonValueStr_Boolean(bool b)
        {
            return (b ? "true" : "false");
        }

        private string GetJsonValueStr_CLSHashtable(CLSHashtable cht)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            foreach (string str in cht.KeyArray)
            {
                builder.Append(this.GetJsonKeyName(str));
                builder.Append(":");
                builder.Append(this.GetJsonValueStr(cht[str]));
                builder.Append(",");
            }
            if (cht.KeyArray.Length > 0)
            {
                builder.Remove(builder.Length - 1, 1);
            }
            builder.Append("}");
            return builder.ToString();
        }

        private string GetJsonValueStr_Default(object obj)
        {
            return this.GetJsonValueStr_String((obj == null) ? "" : obj.ToString());
        }

        private string GetJsonValueStr_Number(decimal number)
        {
            return number.ToString();
        }

        private string GetJsonValueStr_String(string str)
        {
            return string.Format("{0}{1}{2}", "\"", str.Replace(@"\", @"\\").Replace("\"", "\\\""), "\"");
        }

        public CLSHashtable GetValueCLSHT(int index)
        {
            object obj2 = this[index];
            CLSHashtable valueRaw = null;
            if ((obj2 is CLSVData) && (((CLSVData) obj2).ValueRaw is CLSHashtable))
            {
                valueRaw = (CLSHashtable) ((CLSVData) obj2).ValueRaw;
            }
            if (obj2 is CLSHashtable)
            {
                valueRaw = (CLSHashtable) obj2;
            }
            return valueRaw;
        }

        public static implicit operator CLSArrayList(Array value)
        {
            CLSArrayList list = new CLSArrayList();
            foreach (object obj2 in value)
            {
                list.Add(obj2);
            }
            return list;
        }

        public override void RemoveAt(int index)
        {
            base.RemoveAt(index);
        }

        public bool SetByArray<T>(T[] array)
        {
            switch (typeof(T).Name.ToLower())
            {
                case "byte[]":
                case "datetime":
                case "decimal":
                case "int32":
                case "string":
                case "boolean":
                    this.Clear();
                    foreach (T local in array)
                    {
                        this.Add(local);
                    }
                    return true;
            }
            return false;
        }

        public override object[] ToArray()
        {
            return base.ToArray();
        }

        public string ToJson()
        {
            return this.GetJsonValueStr_Array(this);
        }

        public T[] ToTArray<T>()
        {
            switch (typeof(T).Name.ToLower())
            {
                case "byte[]":
                case "datetime":
                case "decimal":
                case "int32":
                case "string":
                case "boolean":
                {
                    T[] localArray = new T[this.Count];
                    for (int i = 0; i < this.Count; i++)
                    {
                        CLSVData data = (CLSVData) this[i];
                        localArray[i] = (T) Convert.ChangeType(data.ValueRaw, typeof(T));
                    }
                    return localArray;
                }
            }
            return new T[0];
        }

        public override int Count
        {
            get
            {
                return base.Count;
            }
        }
    }
}

