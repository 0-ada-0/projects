using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DC2016.Model {

	public partial class UrsuserextInfo {
		#region fields
		private int? _ExtNumber;
		private string _ExtCHTInfo;
		#endregion

		public UrsuserextInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Ursuserext(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_ExtNumber == null ? "null" : _ExtNumber.ToString(), "|",
				_ExtCHTInfo == null ? "null" : _ExtCHTInfo.Replace("|", StringifySplit));
		}
		public UrsuserextInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，UrsuserextInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _ExtNumber = int.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _ExtCHTInfo = ret[1].Replace(StringifySplit, "|");
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("ExtNumber") ? string.Empty : string.Format(", ExtNumber : {0}", ExtNumber == null ? "null" : ExtNumber.ToString()), 
				__jsonIgnore.ContainsKey("ExtCHTInfo") ? string.Empty : string.Format(", ExtCHTInfo : {0}", ExtCHTInfo == null ? "null" : string.Format("'{0}'", ExtCHTInfo.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("ExtNumber")) ht["ExtNumber"] = ExtNumber;
			if (!__jsonIgnore.ContainsKey("ExtCHTInfo")) ht["ExtCHTInfo"] = ExtCHTInfo;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(UrsuserextInfo).GetField("JsonIgnore");
						__jsonIgnore = new Dictionary<string, bool>();
						if (field != null) {
							string[] fs = string.Concat(field.GetValue(null)).Split(',');
							foreach (string f in fs) if (!string.IsNullOrEmpty(f)) __jsonIgnore[f] = true;
						}
					}
				}
			}
		}
		public override bool Equals(object obj) {
			UrsuserextInfo item = obj as UrsuserextInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(UrsuserextInfo op1, UrsuserextInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(UrsuserextInfo op1, UrsuserextInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public int? ExtNumber {
			get { return _ExtNumber; }
			set { _ExtNumber = value; }
		}
		public string ExtCHTInfo {
			get { return _ExtCHTInfo; }
			set { _ExtCHTInfo = value; }
		}
		#endregion

		public DC2016.DAL.Ursuserext.SqlUpdateBuild UpdateDiy {
			get { return DC2016.BLL.Ursuserext.UpdateDiy(this, _ExtNumber); }
		}
	}
}

