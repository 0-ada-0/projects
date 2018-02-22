using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DC2016.Model {

	public partial class D2unactiveInfo {
		#region fields
		private string _UavGUID;
		private string _UavEMail;
		private int? _UavFlag;
		private string _UavGate;
		private string _UavGateSrc;
		private int? _UavNumber;
		private int? _UavState;
		private DateTime? _UavTime1;
		private DateTime? _UavTime2;
		#endregion

		public D2unactiveInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<D2unactive(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_UavGUID == null ? "null" : _UavGUID.Replace("|", StringifySplit), "|",
				_UavEMail == null ? "null" : _UavEMail.Replace("|", StringifySplit), "|",
				_UavFlag == null ? "null" : _UavFlag.ToString(), "|",
				_UavGate == null ? "null" : _UavGate.Replace("|", StringifySplit), "|",
				_UavGateSrc == null ? "null" : _UavGateSrc.Replace("|", StringifySplit), "|",
				_UavNumber == null ? "null" : _UavNumber.ToString(), "|",
				_UavState == null ? "null" : _UavState.ToString(), "|",
				_UavTime1 == null ? "null" : _UavTime1.Value.Ticks.ToString(), "|",
				_UavTime2 == null ? "null" : _UavTime2.Value.Ticks.ToString());
		}
		public D2unactiveInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 9, StringSplitOptions.None);
			if (ret.Length != 9) throw new Exception("格式不正确，D2unactiveInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _UavGUID = ret[0].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[1]) != 0) _UavEMail = ret[1].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[2]) != 0) _UavFlag = int.Parse(ret[2]);
			if (string.Compare("null", ret[3]) != 0) _UavGate = ret[3].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[4]) != 0) _UavGateSrc = ret[4].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[5]) != 0) _UavNumber = int.Parse(ret[5]);
			if (string.Compare("null", ret[6]) != 0) _UavState = int.Parse(ret[6]);
			if (string.Compare("null", ret[7]) != 0) _UavTime1 = new DateTime(long.Parse(ret[7]));
			if (string.Compare("null", ret[8]) != 0) _UavTime2 = new DateTime(long.Parse(ret[8]));
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("UavGUID") ? string.Empty : string.Format(", UavGUID : {0}", UavGUID == null ? "null" : string.Format("'{0}'", UavGUID.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("UavEMail") ? string.Empty : string.Format(", UavEMail : {0}", UavEMail == null ? "null" : string.Format("'{0}'", UavEMail.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("UavFlag") ? string.Empty : string.Format(", UavFlag : {0}", UavFlag == null ? "null" : UavFlag.ToString()), 
				__jsonIgnore.ContainsKey("UavGate") ? string.Empty : string.Format(", UavGate : {0}", UavGate == null ? "null" : string.Format("'{0}'", UavGate.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("UavGateSrc") ? string.Empty : string.Format(", UavGateSrc : {0}", UavGateSrc == null ? "null" : string.Format("'{0}'", UavGateSrc.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("UavNumber") ? string.Empty : string.Format(", UavNumber : {0}", UavNumber == null ? "null" : UavNumber.ToString()), 
				__jsonIgnore.ContainsKey("UavState") ? string.Empty : string.Format(", UavState : {0}", UavState == null ? "null" : UavState.ToString()), 
				__jsonIgnore.ContainsKey("UavTime1") ? string.Empty : string.Format(", UavTime1 : {0}", UavTime1 == null ? "null" : UavTime1.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("UavTime2") ? string.Empty : string.Format(", UavTime2 : {0}", UavTime2 == null ? "null" : UavTime2.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("UavGUID")) ht["UavGUID"] = UavGUID;
			if (!__jsonIgnore.ContainsKey("UavEMail")) ht["UavEMail"] = UavEMail;
			if (!__jsonIgnore.ContainsKey("UavFlag")) ht["UavFlag"] = UavFlag;
			if (!__jsonIgnore.ContainsKey("UavGate")) ht["UavGate"] = UavGate;
			if (!__jsonIgnore.ContainsKey("UavGateSrc")) ht["UavGateSrc"] = UavGateSrc;
			if (!__jsonIgnore.ContainsKey("UavNumber")) ht["UavNumber"] = UavNumber;
			if (!__jsonIgnore.ContainsKey("UavState")) ht["UavState"] = UavState;
			if (!__jsonIgnore.ContainsKey("UavTime1")) ht["UavTime1"] = UavTime1;
			if (!__jsonIgnore.ContainsKey("UavTime2")) ht["UavTime2"] = UavTime2;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(D2unactiveInfo).GetField("JsonIgnore");
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
			D2unactiveInfo item = obj as D2unactiveInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(D2unactiveInfo op1, D2unactiveInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(D2unactiveInfo op1, D2unactiveInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public string UavGUID {
			get { return _UavGUID; }
			set { _UavGUID = value; }
		}
		public string UavEMail {
			get { return _UavEMail; }
			set { _UavEMail = value; }
		}
		public int? UavFlag {
			get { return _UavFlag; }
			set { _UavFlag = value; }
		}
		public string UavGate {
			get { return _UavGate; }
			set { _UavGate = value; }
		}
		public string UavGateSrc {
			get { return _UavGateSrc; }
			set { _UavGateSrc = value; }
		}
		public int? UavNumber {
			get { return _UavNumber; }
			set { _UavNumber = value; }
		}
		public int? UavState {
			get { return _UavState; }
			set { _UavState = value; }
		}
		public DateTime? UavTime1 {
			get { return _UavTime1; }
			set { _UavTime1 = value; }
		}
		public DateTime? UavTime2 {
			get { return _UavTime2; }
			set { _UavTime2 = value; }
		}
		#endregion

		public DC2016.DAL.D2unactive.SqlUpdateBuild UpdateDiy {
			get { return DC2016.BLL.D2unactive.UpdateDiy(this, _UavGUID); }
		}
	}
}

