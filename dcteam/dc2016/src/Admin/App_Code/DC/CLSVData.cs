using System;
using System.Collections;
using System.Reflection;

namespace DC2016.Admin.DC
{
    public class CLSVData
    {
        private CLSVData m_attributes;
        private CLSHashtable m_container;
        private CLSVData m_parent;
        private object m_value;
        private object m_valueraw;

        public CLSVData(object value) : this(null, value, true)
        {
        }

        public CLSVData(CLSHashtable cht, object value) : this(cht, value, true)
        {
            this.Parent = cht;
        }

        public CLSVData(CLSVData clsvd, object value) : this(clsvd.Container, value, true)
        {
            this.Parent = clsvd;
        }

        public CLSVData(CLSHashtable cht, object value, bool convert)
        {
            if (Helper_CCDecimal(value))
            {
                this.Value = Convert.ToDecimal(value);
                this.ValueRaw = value;
            }
            else
            {
                this.Value = value;
            }
            this.Container = cht;
            if (value is CLSHashtable)
            {
                ((CLSHashtable)value).Parent = this;
            }
            if ((convert && ((value is ArrayList) || (value is Hashtable))) && (this.Container != null))
            {
                this.Container.Convert_X2H_ForCollection(value);
            }
        }

        public static bool Helper_CCDecimal(object value)
        {
            return ((((((value is decimal) || (value is sbyte)) || ((value is byte) || (value is short))) || (((value is ushort) || (value is int)) || ((value is uint) || (value is long)))) || (((value is ulong) || (value is char)) || (value is float))) || (value is double));
        }

        public static implicit operator CLSVData(CLSHashtable value)
        {
            return new CLSVData(value);
        }

        public static implicit operator bool(CLSVData value)
        {
            return value.ValueBool;
        }

        public static implicit operator DateTime(CLSVData value)
        {
            return value.ValueDateTime;
        }

        public static implicit operator decimal(CLSVData value)
        {
            return value.ValueDecimal;
        }

        public static implicit operator int(CLSVData value)
        {
            return value.ValueInt;
        }

        public static implicit operator string(CLSVData value)
        {
            return value.ValueString;
        }

        public static implicit operator CLSVData(Array value)
        {
            ArrayList list = new ArrayList();
            foreach (object obj2 in value)
            {
                list.Add(obj2);
            }
            return new CLSVData(list);
        }

        public static implicit operator CLSVData(bool value)
        {
            return new CLSVData(value);
        }

        public static implicit operator CLSVData(byte value)
        {
            return new CLSVData((decimal)value);
        }

        public static implicit operator CLSVData(char value)
        {
            return new CLSVData((decimal)value);
        }

        public static implicit operator CLSVData(ArrayList value)
        {
            return new CLSVData(null, value, false);
        }

        public static implicit operator CLSVData(DateTime value)
        {
            return new CLSVData(value);
        }

        public static implicit operator CLSVData(decimal value)
        {
            return new CLSVData(value);
        }

        public static implicit operator CLSVData(double value)
        {
            return new CLSVData((decimal)value);
        }

        public static implicit operator CLSVData(short value)
        {
            return new CLSVData((decimal)value);
        }

        public static implicit operator CLSVData(int value)
        {
            return new CLSVData(value);
        }

        public static implicit operator CLSVData(long value)
        {
            return new CLSVData((decimal)value);
        }

        public static implicit operator CLSVData(sbyte value)
        {
            return new CLSVData((decimal)value);
        }

        public static implicit operator CLSVData(float value)
        {
            return new CLSVData((decimal)value);
        }

        public static implicit operator CLSVData(string value)
        {
            return new CLSVData(value);
        }

        public static implicit operator CLSVData(ushort value)
        {
            return new CLSVData((decimal)value);
        }

        public static implicit operator CLSVData(uint value)
        {
            return new CLSVData((decimal)value);
        }

        public static implicit operator CLSVData(ulong value)
        {
            return new CLSVData((decimal)value);
        }

        public static implicit operator CLSVData(byte[] value)
        {
            return new CLSVData(null, value, false);
        }

        public static implicit operator CLSVData(object[] value)
        {
            ArrayList list = new ArrayList();
            foreach (object obj2 in value)
            {
                list.Add(obj2);
            }
            return new CLSVData(list);
        }

        public void Set(object key, object value)
        {
            this[key] = new CLSVData(value);
        }

        public override string ToString()
        {
            if (this.Value == null)
            {
                return "null";
            }
            return this.Value.ToString();
        }

        public CLSVData Attributes
        {
            get
            {
                if (this.m_attributes == null)
                {
                    CLSHashtable hashtable = new CLSHashtable();
                    this.m_attributes = new CLSVData(hashtable);
                }
                return this.m_attributes;
            }
        }

        public CLSHashtable Container
        {
            get
            {
                return this.m_container;
            }
            set
            {
                this.m_container = value;
            }
        }

        public bool HasAttributes
        {
            get
            {
                return ((this.m_attributes != null) && (this.m_attributes.Length > 0));
            }
        }

        public CLSVData this[object key]
        {
            get
            {
                if (this.Value is CLSHashtable)
                {
                    CLSHashtable hashtable = (CLSHashtable)this.Value;
                    return hashtable[key];
                }
                return new CLSVData(this, null);
            }
            set
            {
                if (this.Value is CLSHashtable)
                {
                    CLSHashtable hashtable = (CLSHashtable)this.Value;
                    hashtable[key] = value;
                    if (!hashtable.ContainsKey(key) && (value != null))
                    {
                        CLSVData data = value;
                        data.Parent = this;
                        data.Container = this.Container;
                    }
                }
            }
        }

        public string[] Keys
        {
            get
            {
                if (this.Value is CLSHashtable)
                {
                    return ((CLSHashtable)this.Value).KeyArray;
                }
                return new string[0];
            }
        }

        public int Length
        {
            get
            {
                if (this.Value is Hashtable)
                {
                    return ((Hashtable)this.Value).Count;
                }
                return 0;
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

        public object Value
        {
            get
            {
                return this.m_value;
            }
            set
            {
                this.m_valueraw = value;
                this.m_value = value;
            }
        }

        public ArrayList ValueArrayList
        {
            get
            {
                if (this.ValueIsNull)
                {
                    this.Value = new ArrayList();
                }
                if (!(this.Value is ArrayList))
                {
                    throw new Exception("该值不能提供默认");
                }
                return (ArrayList)this.Value;
            }
        }

        public bool ValueBool
        {
            get
            {
                if (this.Value is bool)
                {
                    return (bool)this.Value;
                }
                if (this.Value is string)
                {
                    return (((string)this.Value).Length > 0);
                }
                return ((this.Value is int) && (((int)this.Value) != 0));
            }
        }

        public byte[] ValueByteArray
        {
            get
            {
                if (this.Value is byte[])
                {
                    return (byte[])this.Value;
                }
                return new byte[0];
            }
        }

        public CLSObject ValueCCLSObject
        {
            get
            {
                if (this.Value is string)
                {
                    return (string)this.Value;
                }
                return "";
            }
        }

        public CLSArrayList ValueCLSArrayList
        {
            get
            {
                if (this.ValueIsNull)
                {
                    this.Value = new CLSArrayList();
                }
                if (!(this.Value is CLSArrayList))
                {
                    throw new Exception("该值不能提供默认");
                }
                return (CLSArrayList)this.Value;
            }
        }

        public DateTime ValueDateTime
        {
            get
            {
                if (this.Value is DateTime)
                {
                    return (DateTime)this.Value;
                }
                if (this.Value is string)
                {
                    DateTime minValue = DateTime.MinValue;
                    if (DateTime.TryParse((string)this.Value, out minValue))
                    {
                        return minValue;
                    }
                }
                return new DateTime(0x7b2, 1, 1);
            }
        }

        public decimal ValueDecimal
        {
            get
            {
                decimal num;
                if (this.Value is decimal)
                {
                    return (decimal)this.Value;
                }
                if ((this.Value is string) && decimal.TryParse((string)this.Value, out num))
                {
                    return num;
                }
                return 0M;
            }
            set
            {
                if (this.Value is decimal)
                {
                    this.Value = value;
                }
            }
        }

        public int ValueInt
        {
            get
            {
                try
                {
                    return (int)this.ValueDecimal;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public bool ValueIsCollection
        {
            get
            {
                return ((this.Value is Hashtable) || (this.Value is ArrayList));
            }
        }

        public bool ValueIsNull
        {
            get
            {
                return ((this.Value is Hashtable) && (((Hashtable)this.Value).Count == 0));
            }
        }

        public object ValueRaw
        {
            get
            {
                if (this.m_valueraw == null)
                {
                    return this.Value;
                }
                return this.m_valueraw;
            }
            set
            {
                this.m_valueraw = value;
            }
        }

        public string ValueString
        {
            get
            {
                if (this.Value is string)
                {
                    return (string)this.Value;
                }
                return "";
            }
        }
    }
}
