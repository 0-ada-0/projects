using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DC2016.Model {

	public partial class UrsacctInfo {
		#region fields
		private string _AcctEMail;
		private int? _AcctNumber;
		#endregion

		public UrsacctInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Ursacct(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_AcctEMail == null ? "null" : _AcctEMail.Replace("|", StringifySplit), "|",
				_AcctNumber == null ? "null" : _AcctNumber.ToString());
		}
		public UrsacctInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，UrsacctInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _AcctEMail = ret[0].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[1]) != 0) _AcctNumber = int.Parse(ret[1]);
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("AcctEMail") ? string.Empty : string.Format(", AcctEMail : {0}", AcctEMail == null ? "null" : string.Format("'{0}'", AcctEMail.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("AcctNumber") ? string.Empty : string.Format(", AcctNumber : {0}", AcctNumber == null ? "null" : AcctNumber.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("AcctEMail")) ht["AcctEMail"] = AcctEMail;
			if (!__jsonIgnore.ContainsKey("AcctNumber")) ht["AcctNumber"] = AcctNumber;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(UrsacctInfo).GetField("JsonIgnore");
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
			UrsacctInfo item = obj as UrsacctInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(UrsacctInfo op1, UrsacctInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(UrsacctInfo op1, UrsacctInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public string AcctEMail {
			get { return _AcctEMail; }
			set { _AcctEMail = value; }
		}
		public int? AcctNumber {
			get { return _AcctNumber; }
			set { _AcctNumber = value; }
		}
		#endregion

		public DC2016.DAL.Ursacct.SqlUpdateBuild UpdateDiy {
			get { return DC2016.BLL.Ursacct.UpdateDiy(this, _AcctEMail); }
		}
	}
}

