using System;

namespace DC2016.Admin.DC
{
    public class CLSObject
    {
        private object m_value;

        public CLSObject(object value)
        {
            this.Value = value;
        }

        public static implicit operator bool(CLSObject value)
        {
            return value.ValueBool;
        }

        public static implicit operator DateTime(CLSObject value)
        {
            return value.ValueDateTime;
        }

        public static implicit operator decimal(CLSObject value)
        {
            return value.ValueDecimal;
        }

        public static implicit operator int(CLSObject value)
        {
            return value.ValueInt32;
        }

        public static implicit operator long(CLSObject value)
        {
            return value.ValueInt64;
        }

        public static implicit operator string(CLSObject value)
        {
            return value.ValueString;
        }

        public static implicit operator CLSObject(decimal value)
        {
            return new CLSObject(value);
        }

        public static implicit operator CLSObject(int value)
        {
            return new CLSObject(value);
        }

        public static implicit operator CLSObject(string value)
        {
            return new CLSObject(value);
        }

        public static CLSObject[] ParseStringSplit(string str, string Split)
        {
            string[] strArray = str.Split(Split.ToCharArray());
            CLSObject[] objArray = new CLSObject[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                objArray[i] = strArray[i];
            }
            return objArray;
        }

        public override string ToString()
        {
            return this.ValueString;
        }

        public bool IsString
        {
            get
            {
                return ((this.Value is string) && (this.Value != null));
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
                this.m_value = value;
            }
        }

        public bool ValueBool
        {
            get
            {
                return (this.ValueString.ToLower() == "true");
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
                if (this.Value == null)
                {
                    return DateTime.Parse("1970-01-01");
                }
                try
                {
                    return DateTime.Parse(this.Value.ToString());
                }
                catch
                {
                    return DateTime.Parse("1970-01-01");
                }
            }
        }

        public decimal ValueDecimal
        {
            get
            {
                if (this.Value is decimal)
                {
                    return (decimal)this.Value;
                }
                if (this.Value == null)
                {
                    return -1M;
                }
                try
                {
                    return decimal.Parse(this.Value.ToString());
                }
                catch
                {
                    return -1M;
                }
            }
        }

        public int ValueInt32
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

        public long ValueInt64
        {
            get
            {
                return (long)this.ValueDecimal;
            }
        }

        public bool ValueIsNull
        {
            get
            {
                return ((this.Value == null) || ((this.Value is string) && (((string)this.Value) == "")));
            }
        }

        public string ValueString
        {
            get
            {
                if (this.Value == null)
                {
                    return "";
                }
                if (this.Value is string)
                {
                    return (string)this.Value;
                }
                return this.Value.ToString();
            }
        }
    }
}
