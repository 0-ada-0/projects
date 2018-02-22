
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace DC2016.Admin.DC
{
    public class CLSHashtable : Hashtable
    {
        private string m_filename;
        private bool m_forceparse;
        private Hashtable m_ht_data;
        private string m_input;
        private string[] m_keyarray;
        private bool m_keychange;
        private int m_keyrule;
        private CLSVData m_parent;
        private decimal m_versiondecimal;
        private string m_versionold;
        private bool m_versionupdate;
        private bool outbysort;
        private static readonly string STA_HEAD_BOOL = @"\BOOL";
        private static readonly string STA_HEAD_BYTE = @"\BYTE";
        private static readonly string STA_HEAD_DATETIME = @"\DT";
        private static readonly string STA_HEAD_STRING = @"\STR";
        private static readonly string STATIC_VERSION = "0.5.0.20110225";
        private static readonly string STATIC_VERSION_DEFAULT = "0.5";
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
        private string version;

        public CLSHashtable() : this(new Hashtable())
        {
            this.Version = STATIC_VERSION;
        }

        public CLSHashtable(Hashtable ht_data)
        {
            this.m_forceparse = false;
            this.outbysort = false;
            this.version = "0.5";
            this.m_versionold = "";
            this.m_versionupdate = false;
            this.m_versiondecimal = new decimal();
            this.m_keyrule = 0;
            this.m_keychange = true;
            this.m_keyarray = new string[0];
            this.m_ht_data = new Hashtable();
            foreach (string str in ht_data.Keys)
            {
                this.Add(str, ht_data[str]);
            }
        }

        public CLSHashtable(params object[] args) : this()
        {
            if ((args == null) || ((args.Length % 2) > 0))
            {
                throw new Exception("initialize failed 1");
            }
            for (int i = 0; i < args.Length; i += 2)
            {
                if (!(args[i] is string) || string.IsNullOrEmpty((string)args[i]))
                {
                    throw new Exception("initialize failed 2");
                }
                string key = (string)args[i];
                if (this.m_ht_data.ContainsKey(key))
                {
                    throw new Exception("initialize failed 3");
                }
                object obj2 = args[i + 1];
                this.Add(key, new CLSVData(this, obj2));
            }
        }

        public CLSVData _(string key)
        {
            return this[key];
        }

        public override void Add(object key, object value)
        {
            this.Add(key, value, false);
        }

        public void Add(object key, object value, bool parse)
        {
            if (!(key is string))
            {
                throw new Exception("添加进列表的KEY必须为字符串");
            }
            this.m_keychange = true;
            if (value is CLSVData)
            {
                this.m_ht_data.Add(key, value);
            }
            else if (parse && (value is string))
            {
                this.m_ht_data.Add(key, (string)value);
            }
            else if ((value is ArrayList) && !(value is CLSArrayList))
            {
                ArrayList list = (ArrayList)value;
                CLSArrayList list2 = new CLSArrayList();
                foreach (object obj2 in list)
                {
                    list2.Add(obj2);
                }
                this.m_ht_data.Add(key, list2);
            }
            else
            {
                this.m_ht_data.Add(key, new CLSVData(this, value));
            }
        }

        private static bool Allow_H2A(object obj)
        {
            return (((((obj is int) || (obj is string)) || ((obj is DateTime) || (obj is decimal))) || ((obj is short) || (obj is long))) || (obj is bool));
        }

        public override void Clear()
        {
            this.m_keychange = true;
            this.m_ht_data.Clear();
        }

        public override bool ContainsKey(object key)
        {
            return this.m_ht_data.ContainsKey(key);
        }

        private void Convert_Bin_H2X_Hashtable(MemoryStream ms, Hashtable htb)
        {
            CLSHashtable hashtable = (htb is CLSHashtable) ? ((CLSHashtable)htb) : new CLSHashtable(htb);
            string[] keyArray = hashtable.KeyArray;
            foreach (string str in keyArray)
            {
                object obj2 = hashtable[str];
            }
        }

        private static ArrayList Convert_H2A(CLSHashtable ht)
        {
            ArrayList list = new ArrayList();
            foreach (object obj2 in ht.Keys)
            {
                object obj3 = ht[obj2];
                if (Allow_H2A(obj3))
                {
                    list.Add(obj2);
                    list.Add(obj3);
                }
            }
            return list;
        }

        public string Convert_H2X(object obj)
        {
            if ((obj == null) || (obj is DBNull))
            {
                return "";
            }
            if (obj is DateTime)
            {
                DateTime time = (DateTime)obj;
                return (STA_HEAD_DATETIME + time.Ticks.ToString());
            }
            if (obj is bool)
            {
                bool flag4 = (bool)obj;
                return (STA_HEAD_BOOL + flag4.ToString());
            }
            if (obj is string)
            {
                return (STA_HEAD_STRING + obj.ToString().Replace(@"\", @"\\"));
            }
            if (obj is byte[])
            {
                return (STA_HEAD_BYTE + Convert.ToBase64String((byte[])obj));
            }
            return obj.ToString();
        }

        private static string Convert_String(MemoryStream ms)
        {
            return Convert_String(ms, Encoding.UTF8);
        }

        private static string Convert_String(MemoryStream ms, Encoding code)
        {
            ms.Position = 0;
            StreamReader reader = new StreamReader(ms, code);
            string str = reader.ReadToEnd();
            reader.Dispose();
            return str;
        }

        public object Convert_X2H(string objstr)
        {
            if ((objstr.StartsWith(@"\") && (objstr.Length >= 2)) && (objstr[1] != '\\'))
            {
                return this.Convert_X2H_T(objstr);
            }
            try
            {
                return decimal.Parse(objstr);
            }
            catch
            {
                return objstr.Replace(@"\\", @"\");
            }
        }

        public void Convert_X2H_ForCollection(object value)
        {
            if (value is Hashtable)
            {
                Hashtable hashtable = (Hashtable)value;
                string[] array = new string[hashtable.Count];
                hashtable.Keys.CopyTo(array, 0);
                foreach (string str in array)
                {
                    object obj2 = hashtable[str];
                    if (obj2 is string)
                    {
                        object obj3 = this.Convert_X2H((string)obj2);
                        CLSVData data = new CLSVData(this, obj3);
                        hashtable[str] = data;
                    }
                    else if (Helper_IsParent(obj2))
                    {
                        this.Convert_X2H_ForCollection(obj2);
                    }
                }
            }
            else if (value is ArrayList)
            {
                ArrayList list = (ArrayList)value;
                for (int i = 0; i < list.Count; i++)
                {
                    object obj4 = list[i];
                    if (obj4 is string)
                    {
                        object obj5 = this.Convert_X2H((string)obj4);
                        CLSVData data2 = new CLSVData(this, obj5);
                        list[i] = data2;
                    }
                    else if (Helper_IsParent(obj4))
                    {
                        this.Convert_X2H_ForCollection(obj4);
                    }
                }
            }
        }

        public object Convert_X2H_T(string objstr)
        {
            if (objstr.StartsWith(STA_HEAD_DATETIME))
            {
                return new DateTime(long.Parse(objstr.Remove(0, STA_HEAD_DATETIME.Length)));
            }
            if (objstr.StartsWith(STA_HEAD_BOOL))
            {
                return bool.Parse(objstr.Remove(0, STA_HEAD_BOOL.Length));
            }
            if (objstr.StartsWith(STA_HEAD_STRING))
            {
                return objstr.Remove(0, STA_HEAD_STRING.Length).Replace(@"\\", @"\");
            }
            if (objstr.StartsWith(STA_HEAD_BYTE))
            {
                return Convert.FromBase64String(objstr.Remove(0, STA_HEAD_BYTE.Length));
            }
            return objstr;
        }

        public void CopyTo(CLSHashtable cht_dst, params string[] keys)
        {
            foreach (string str in keys)
            {
                if (this.Has(str))
                {
                    cht_dst.Set(str, this[str]);
                }
            }
        }

        public static CLSHashtable Create(Hashtable ht_data)
        {
            return new CLSHashtable(ht_data);
        }

        private static void Func_NS_ParseNode(CLSHashtable cht, bool isroot, XmlNode node, CLSVData clsvopt)
        {
            string name = node.Name;
            if (Helper_NS_IsParent(node))
            {
                string str2 = name;
                if (!(str2 == "dict"))
                {
                    if (str2 == "array")
                    {
                        if (isroot)
                        {
                            cht["array"] = new CLSArrayList();
                            clsvopt = cht["array"];
                        }
                        for (int i = 0; i < node.ChildNodes.Count; i++)
                        {
                            XmlNode node4 = node.ChildNodes[i];
                            if (Helper_NS_IsParent(node4))
                            {
                                CLSVData data3 = Helper_NS_GetParentObj(cht, node4);
                                clsvopt.ValueCLSArrayList.Add(data3.ValueRaw);
                                Func_NS_ParseNode(cht, false, node4, data3);
                            }
                            else
                            {
                                CLSVData data4 = Helper_NS_GetChildObj(cht, node4);
                                clsvopt.ValueCLSArrayList.Add(data4.ValueRaw);
                            }
                        }
                    }
                }
                else if ((node.ChildNodes.Count != 0) && ((node.ChildNodes.Count % 2) <= 0))
                {
                    for (int j = 0; j < node.ChildNodes.Count; j += 2)
                    {
                        XmlNode node2 = node.ChildNodes[j];
                        XmlNode node3 = node.ChildNodes[j + 1];
                        if (node2.Name == "key")
                        {
                            if (Helper_NS_IsParent(node3))
                            {
                                CLSVData data = Helper_NS_GetParentObj(cht, node3);
                                if (isroot)
                                {
                                    cht[node2.InnerText] = data;
                                }
                                else
                                {
                                    clsvopt[node2.InnerText] = data;
                                }
                                Func_NS_ParseNode(cht, false, node3, data);
                            }
                            else
                            {
                                CLSVData data2 = Helper_NS_GetChildObj(cht, node3);
                                if (isroot)
                                {
                                    cht[node2.InnerText] = data2;
                                }
                                else
                                {
                                    clsvopt[node2.InnerText] = data2;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void Func_ParseNode(CLSHashtable cht, XmlNodeList Nodes, object opt)
        {
            foreach (XmlNode node in Nodes)
            {
                string str = ((node.Attributes == null) || (node.Attributes["type"] == null)) ? "" : node.Attributes["type"].Value;
                string key = null;
                if (cht.KeyRule == 2)
                {
                    string str3 = ((node.Attributes == null) || (node.Attributes["N"] == null)) ? "" : node.Attributes["N"].Value;
                    if (((str3 == "") && Helper_WishName(node)) && !(opt is ArrayList))
                    {
                        continue;
                    }
                    if (Helper_WishName(node))
                    {
                        key = str3;
                    }
                }
                else
                {
                    if (cht.KeyRule != 1)
                    {
                        throw new Exception("Key Rule Error");
                    }
                    key = node.Name.StartsWith("K_") ? node.Name.Remove(0, 2) : node.Name;
                }
                if (str != "")
                {
                    switch (str)
                    {
                        case "al":
                            {
                                CLSArrayList list = new CLSArrayList();
                                Func_ParseNode(cht, node.ChildNodes, list);
                                if (opt is CLSHashtable)
                                {
                                    ((CLSHashtable)opt).Add(key, list);
                                }
                                else if (opt is Hashtable)
                                {
                                    ((Hashtable)opt).Add(key, list);
                                }
                                else if (opt is ArrayList)
                                {
                                    ((ArrayList)opt).Add(list);
                                }
                                goto Label_02B9;
                            }
                        case "ht":
                            {
                                CLSHashtable hashtable = new CLSHashtable();
                                Func_ParseNode(cht, node.ChildNodes, hashtable);
                                if (opt is CLSHashtable)
                                {
                                    ((CLSHashtable)opt).Add(key, hashtable);
                                    goto Label_02B9;
                                }
                                if (opt is Hashtable)
                                {
                                    ((Hashtable)opt).Add(key, hashtable);
                                }
                                else if (opt is ArrayList)
                                {
                                    ((ArrayList)opt).Add(hashtable);
                                }
                                break;
                            }
                    }
                Label_02B9:;
                }
                else if (opt is ArrayList)
                {
                    ((ArrayList)opt).Add(node.InnerText);
                }
                else if (opt is CLSHashtable)
                {
                    ((CLSHashtable)opt).Add(key, node.InnerText, true);
                }
                else if (opt is Hashtable)
                {
                    ((Hashtable)opt).Add(key, node.InnerText);
                }
            }
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
                foreach (object obj3 in (IEnumerable)value.Value)
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
            return string.Format("{0}{1}{2}", "\"", str.Replace(@"\", @"\\").Replace("\"", "\\\"").Replace("\"", "'").Replace("\r", "").Replace("\n", "<br />").Replace("\t", "    "), "\"");
        }

        public bool Has(string key)
        {
            return this.ContainsKey(key);
        }

        private static bool Helper_Check_Number(string str)
        {
            foreach (char ch in str)
            {
                if ((ch < '0') || (ch > '9'))
                {
                    return false;
                }
            }
            return true;
        }

        private static decimal Helper_GetVersionDecimal(string version)
        {
            char[] separator = new char[] { '.' };
            string[] strArray = version.Split(separator);
            if (strArray.Length != 4)
            {
                return decimal.Zero;
            }
            CLSObject[] objArray = new CLSObject[] { strArray[0], strArray[1], strArray[2], strArray[3] };
            return ((((((objArray[0].ValueDecimal * 100M) * 100M) * 100000000M) + ((objArray[1].ValueDecimal * 100M) * 100000000M)) + (objArray[2].ValueDecimal * 100000000M)) + objArray[3].ValueDecimal);
        }

        private static bool Helper_IsParent(object value)
        {
            string type = "";
            return Helper_IsParent(value, ref type);
        }

        private static bool Helper_IsParent(object value, ref string type)
        {
            if (value is Hashtable)
            {
                type = "ht";
            }
            else if (value is ArrayList)
            {
                type = "al";
            }
            return ((value is Hashtable) || (value is ArrayList));
        }

        private static CLSVData Helper_NS_GetChildObj(CLSHashtable cht, XmlNode Node)
        {
            return Helper_NS_GetChildObj(cht, Node.Name, Node.InnerText);
        }

        private static CLSVData Helper_NS_GetChildObj(CLSHashtable cht, string type, string text)
        {
            if (type == "data")
            {
                return Convert.FromBase64String(text);
            }
            if (type == "date")
            {
                return DateTime.Parse(text);
            }
            if (type == "real")
            {
                return decimal.Parse(text);
            }
            if (type == "integer")
            {
                return int.Parse(text);
            }
            if (type == "string")
            {
                return text;
            }
            if (type == "true")
            {
                return 1;
            }
            if (type != "false")
            {
                throw new Exception("该类别不能创建父对象");
            }
            return 0;
        }

        private static string Helper_NS_GetChildType(object value)
        {
            if (value is CLSVData)
            {
                value = ((CLSVData)value).ValueRaw;
            }
            if (value is byte[])
            {
                return "data";
            }
            if (value is DateTime)
            {
                return "date";
            }
            if (value is decimal)
            {
                return "real";
            }
            if (value is int)
            {
                return "integer";
            }
            if (value is string)
            {
                return "string";
            }
            if (value is bool)
            {
                return (((bool)value) ? "true" : "false");
            }
            return "";
        }

        private static string Helper_NS_GetChildValueStr(string type, object value)
        {
            if (value is CLSVData)
            {
                value = ((CLSVData)value).ValueRaw;
            }
            if (type == "data")
            {
                return Convert.ToBase64String((byte[])value);
            }
            if (type == "date")
            {
                DateTime time = (DateTime)value;
                return (time.ToUniversalTime().ToString("s") + "Z");
            }
            if (type == "real")
            {
                decimal num = (decimal)value;
                return num.ToString();
            }
            if (type == "integer")
            {
                int num2 = (int)value;
                return num2.ToString();
            }
            if (type == "string")
            {
                return (string)value;
            }
            if ((type != "true") && (type != "false"))
            {
                throw new Exception("没有这个类别," + type);
            }
            return "";
        }

        private static CLSVData Helper_NS_GetParentObj(CLSHashtable cht, XmlNode Node)
        {
            if (Node.Name == "dict")
            {
                return new CLSVData(cht, new CLSHashtable());
            }
            if (Node.Name != "array")
            {
                throw new Exception("该类别不能创建父对象");
            }
            return new CLSVData(cht, new CLSArrayList());
        }

        private static bool Helper_NS_IsParent(string type)
        {
            return ((type == "dict") || (type == "array"));
        }

        private static bool Helper_NS_IsParent(XmlNode Node)
        {
            return Helper_NS_IsParent(Node.Name);
        }

        private static bool Helper_NS_IsParent(object value, ref string type)
        {
            if (value is Hashtable)
            {
                type = "dict";
            }
            else if (value is ArrayList)
            {
                type = "array";
            }
            return ((value is Hashtable) || (value is ArrayList));
        }

        private static bool Helper_WishName(XmlNode Node)
        {
            return (Node.Name != "Item");
        }

        public static implicit operator CLSHashtable(object[] value)
        {
            return new CLSHashtable(value);
        }

        public override void Remove(object key)
        {
            this.m_keychange = true;
            this.m_ht_data.Remove(key);
        }

        public void Set(params object[] args)
        {
            if ((args != null) && ((args.Length % 2) <= 0))
            {
                for (int i = 0; i < args.Length; i += 2)
                {
                    this.Set(args[i].ToString(), args[i + 1]);
                }
            }
        }

        public void Set(object key, object value)
        {
            this[key] = new CLSVData(value);
        }

        public byte[] ToBinData()
        {
            MemoryStream ms = new MemoryStream();
            this.Convert_Bin_H2X_Hashtable(ms, this.HT_Data);
            string str = Convert_String(ms, Encoding.UTF8);
            return null;
        }

        public void ToFile()
        {
            this.ToFile(this.FileName);
        }

        public void ToFile(string filename)
        {
            if ((filename == null) || (filename == ""))
            {
                throw new Exception("保存路径不存在，请仔细检查");
            }
            using (StreamWriter writer = System.IO.File.CreateText(filename))
            {
                writer.Write(this.ToString());
            }
        }

        public string ToJson()
        {
            return this.GetJsonValueStr_CLSHashtable(this);
        }

        public override int Count
        {
            get
            {
                return this.m_ht_data.Count;
            }
        }

        public string FileName
        {
            get
            {
                return this.m_filename;
            }
            set
            {
                this.m_filename = value;
            }
        }

        public CLSHashtable ForceParse
        {
            get
            {
                this.m_forceparse = true;
                return this;
            }
        }

        public Hashtable HT_Data
        {
            get
            {
                return this.m_ht_data;
            }
        }

        public string Input
        {
            get
            {
                return this.m_input;
            }
        }

        public CLSVData this[object key]
        {
            get
            {
                CLSVData data = null;
                object obj2 = this.m_ht_data[key];
                if (obj2 == null)
                {
                    CLSVData data2 = new CLSVData(this, new CLSHashtable());
                    this.Add(key, data2);
                    data = data2;
                }
                else if (obj2 is CLSVData)
                {
                    data = (CLSVData)obj2;
                }
                else if (obj2 is CLSHashtable)
                {
                    data = new CLSVData(obj2);
                }
                else if (obj2 is ArrayList)
                {
                    data = new CLSVData(this, obj2, true);
                }
                else if (obj2 is string)
                {
                    string objstr = (string)obj2;
                    object obj3 = this.Convert_X2H(objstr);
                    CLSVData data3 = new CLSVData(this, obj3);
                    this[key] = data3;
                    data = data3;
                }
                else
                {
                    CLSVData data4 = new CLSVData(this, new CLSHashtable());
                    this[key] = data4;
                    data = data4;
                }
                data.Container = this;
                return data;
            }
            set
            {
                if (value == null)
                {
                    this.m_ht_data.Remove(key);
                }
                else
                {
                    value.Container = this;
                    if (!this.m_ht_data.ContainsKey(key))
                    {
                        this.m_keychange = true;
                        this.m_ht_data.Add(key, value);
                    }
                    else
                    {
                        this.m_ht_data[key] = value;
                    }
                }
            }
        }

        public string[] KeyArray
        {
            get
            {
                if (this.m_keychange)
                {
                    Hashtable hashtable = this.HT_Data;
                    string[] array = new string[hashtable.Count];
                    int index = 0;
                    foreach (object obj2 in hashtable.Keys)
                    {
                        array[index] = obj2.ToString();
                        index++;
                    }
                    if (this.OutBySort)
                    {
                        Array.Sort<string>(array);
                    }
                    this.m_keyarray = array;
                    this.m_keychange = false;
                }
                return this.m_keyarray;
            }
        }

        public int KeyRule
        {
            get
            {
                if (this.m_keyrule == 0)
                {
                    this.m_keyrule = (this.VersionDecimal >= 50020100114M) ? 2 : 1;
                }
                return this.m_keyrule;
            }
            set
            {
                this.m_keyrule = value;
            }
        }

        public override ICollection Keys
        {
            get
            {
                return this.m_ht_data.Keys;
            }
        }

        public bool OutBySort
        {
            get
            {
                return this.outbysort;
            }
            set
            {
                this.outbysort = value;
            }
        }

        public CLSVData Parent
        {
            get
            {
                return this.m_parent;
            }
            set
            {
                this.m_parent = value;
            }
        }

        public string Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }

        public decimal VersionDecimal
        {
            get
            {
                if (this.m_versiondecimal == decimal.Zero)
                {
                    this.m_versiondecimal = Helper_GetVersionDecimal(this.Version);
                }
                return this.m_versiondecimal;
            }
        }

        public bool VersionUpdate
        {
            get
            {
                return this.m_versionupdate;
            }
            set
            {
                this.m_versionupdate = value;
                this.m_keyrule = 0;
                if (value)
                {
                    if (this.m_versionold == "")
                    {
                        this.m_versionold = this.Version;
                    }
                    this.Version = STATIC_VERSION;
                    this.m_versiondecimal = Helper_GetVersionDecimal(this.Version);
                }
                else if (this.m_versionold != "")
                {
                    this.Version = this.m_versionold;
                    this.m_versiondecimal = Helper_GetVersionDecimal(this.Version);
                }
            }
        }

        private enum TokenType
        {
            TOKEN_NONE,
            TOKEN_CURLY_OPEN,
            TOKEN_CURLY_CLOSE,
            TOKEN_SQUARED_OPEN,
            TOKEN_SQUARED_CLOSE,
            TOKEN_COLON,
            TOKEN_COMMA,
            TOKEN_STRING,
            TOKEN_NUMBER,
            TOKEN_TRUE,
            TOKEN_FALSE,
            TOKEN_NULL
        }
    }
}

