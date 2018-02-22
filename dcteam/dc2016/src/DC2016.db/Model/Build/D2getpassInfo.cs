using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DC2016.Model {

	public partial class D2getpassInfo {
		#region fields
		private string _GtpsGUID;
		private string _GtpsEMail;
		private string _GtpsGate;
		private int? _GtpsIP;
		private int? _GtpsNumber;
		private int? _GtpsState;
		private DateTime? _GtpsTime1;
		private DateTime? _GtpsTime2;
		private int? _GtpsType;
		#endregion

		public D2getpassInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<D2getpass(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_GtpsGUID == null ? "null" : _GtpsGUID.Replace("|", StringifySplit), "|",
				_GtpsEMail == null ? "null" : _GtpsEMail.Replace("|", StringifySplit), "|",
				_GtpsGate == null ? "null" : _GtpsGate.Replace("|", StringifySplit), "|",
				_GtpsIP == null ? "null" : _GtpsIP.ToString(), "|",
				_GtpsNumber == null ? "null" : _GtpsNumber.ToString(), "|",
				_GtpsState == null ? "null" : _GtpsState.ToString(), "|",
				_GtpsTime1 == null ? "null" : _GtpsTime1.Value.Ticks.ToString(), "|",
				_GtpsTime2 == null ? "null" : _GtpsTime2.Value.Ticks.ToString(), "|",
				_GtpsType == null ? "null" : _GtpsType.ToString());
		}
		public D2getpassInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 9, StringSplitOptions.None);
			if (ret.Length != 9) throw new Exception("格式不正确，D2getpassInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _GtpsGUID = ret[0].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[1]) != 0) _GtpsEMail = ret[1].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[2]) != 0) _GtpsGate = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) _GtpsIP = int.Parse(ret[3]);
			if (string.Compare("null", ret[4]) != 0) _GtpsNumber = int.Parse(ret[4]);
			if (string.Compare("null", ret[5]) != 0) _GtpsState = int.Parse(ret[5]);
			if (string.Compare("null", ret[6]) != 0) _GtpsTime1 = new DateTime(long.Parse(ret[6]));
			if (string.Compare("null", ret[7]) != 0) _GtpsTime2 = new DateTime(long.Parse(ret[7]));
			if (string.Compare("null", ret[8]) != 0) _GtpsType = int.Parse(ret[8]);
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("GtpsGUID") ? string.Empty : string.Format(", GtpsGUID : {0}", GtpsGUID == null ? "null" : string.Format("'{0}'", GtpsGUID.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("GtpsEMail") ? string.Empty : string.Format(", GtpsEMail : {0}", GtpsEMail == null ? "null" : string.Format("'{0}'", GtpsEMail.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("GtpsGate") ? string.Empty : string.Format(", GtpsGate : {0}", GtpsGate == null ? "null" : string.Format("'{0}'", GtpsGate.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("GtpsIP") ? string.Empty : string.Format(", GtpsIP : {0}", GtpsIP == null ? "null" : GtpsIP.ToString()), 
				__jsonIgnore.ContainsKey("GtpsNumber") ? string.Empty : string.Format(", GtpsNumber : {0}", GtpsNumber == null ? "null" : GtpsNumber.ToString()), 
				__jsonIgnore.ContainsKey("GtpsState") ? string.Empty : string.Format(", GtpsState : {0}", GtpsState == null ? "null" : GtpsState.ToString()), 
				__jsonIgnore.ContainsKey("GtpsTime1") ? string.Empty : string.Format(", GtpsTime1 : {0}", GtpsTime1 == null ? "null" : GtpsTime1.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("GtpsTime2") ? string.Empty : string.Format(", GtpsTime2 : {0}", GtpsTime2 == null ? "null" : GtpsTime2.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("GtpsType") ? string.Empty : string.Format(", GtpsType : {0}", GtpsType == null ? "null" : GtpsType.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("GtpsGUID")) ht["GtpsGUID"] = GtpsGUID;
			if (!__jsonIgnore.ContainsKey("GtpsEMail")) ht["GtpsEMail"] = GtpsEMail;
			if (!__jsonIgnore.ContainsKey("GtpsGate")) ht["GtpsGate"] = GtpsGate;
			if (!__jsonIgnore.ContainsKey("GtpsIP")) ht["GtpsIP"] = GtpsIP;
			if (!__jsonIgnore.ContainsKey("GtpsNumber")) ht["GtpsNumber"] = GtpsNumber;
			if (!__jsonIgnore.ContainsKey("GtpsState")) ht["GtpsState"] = GtpsState;
			if (!__jsonIgnore.ContainsKey("GtpsTime1")) ht["GtpsTime1"] = GtpsTime1;
			if (!__jsonIgnore.ContainsKey("GtpsTime2")) ht["GtpsTime2"] = GtpsTime2;
			if (!__jsonIgnore.ContainsKey("GtpsType")) ht["GtpsType"] = GtpsType;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(D2getpassInfo).GetField("JsonIgnore");
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
			D2getpassInfo item = obj as D2getpassInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(D2getpassInfo op1, D2getpassInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(D2getpassInfo op1, D2getpassInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public string GtpsGUID {
			get { return _GtpsGUID; }
			set { _GtpsGUID = value; }
		}
		public string GtpsEMail {
			get { return _GtpsEMail; }
			set { _GtpsEMail = value; }
		}
		public string GtpsGate {
			get { return _GtpsGate; }
			set { _GtpsGate = value; }
		}
		public int? GtpsIP {
			get { return _GtpsIP; }
			set { _GtpsIP = value; }
		}
		public int? GtpsNumber {
			get { return _GtpsNumber; }
			set { _GtpsNumber = value; }
		}
		public int? GtpsState {
			get { return _GtpsState; }
			set { _GtpsState = value; }
		}
		public DateTime? GtpsTime1 {
			get { return _GtpsTime1; }
			set { _GtpsTime1 = value; }
		}
		public DateTime? GtpsTime2 {
			get { return _GtpsTime2; }
			set { _GtpsTime2 = value; }
		}
		public int? GtpsType {
			get { return _GtpsType; }
			set { _GtpsType = value; }
		}
		#endregion

		public DC2016.DAL.D2getpass.SqlUpdateBuild UpdateDiy {
			get { return DC2016.BLL.D2getpass.UpdateDiy(this, _GtpsGUID); }
		}
	}
}

