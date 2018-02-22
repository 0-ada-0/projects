using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DC2016.Model {

	public partial class UrsuserInfo {
		#region fields
		private int? _UrsNumber;
		private int? _UrsBirthDay;
		private string _UrsIDCard;
		private string _UrsMobile;
		private int? _UrsQQ;
		private DateTime? _UrsTime;
		#endregion

		public UrsuserInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Ursuser(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_UrsNumber == null ? "null" : _UrsNumber.ToString(), "|",
				_UrsBirthDay == null ? "null" : _UrsBirthDay.ToString(), "|",
				_UrsIDCard == null ? "null" : _UrsIDCard.Replace("|", StringifySplit), "|",
				_UrsMobile == null ? "null" : _UrsMobile.Replace("|", StringifySplit), "|",
				_UrsQQ == null ? "null" : _UrsQQ.ToString(), "|",
				_UrsTime == null ? "null" : _UrsTime.Value.Ticks.ToString());
		}
		public UrsuserInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 6, StringSplitOptions.None);
			if (ret.Length != 6) throw new Exception("格式不正确，UrsuserInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _UrsNumber = int.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _UrsBirthDay = int.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) _UrsIDCard = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) _UrsMobile = ret[3].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[4]) != 0) _UrsQQ = int.Parse(ret[4]);
			if (string.Compare("null", ret[5]) != 0) _UrsTime = new DateTime(long.Parse(ret[5]));
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("UrsNumber") ? string.Empty : string.Format(", UrsNumber : {0}", UrsNumber == null ? "null" : UrsNumber.ToString()), 
				__jsonIgnore.ContainsKey("UrsBirthDay") ? string.Empty : string.Format(", UrsBirthDay : {0}", UrsBirthDay == null ? "null" : UrsBirthDay.ToString()), 
				__jsonIgnore.ContainsKey("UrsIDCard") ? string.Empty : string.Format(", UrsIDCard : {0}", UrsIDCard == null ? "null" : string.Format("'{0}'", UrsIDCard.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("UrsMobile") ? string.Empty : string.Format(", UrsMobile : {0}", UrsMobile == null ? "null" : string.Format("'{0}'", UrsMobile.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("UrsQQ") ? string.Empty : string.Format(", UrsQQ : {0}", UrsQQ == null ? "null" : UrsQQ.ToString()), 
				__jsonIgnore.ContainsKey("UrsTime") ? string.Empty : string.Format(", UrsTime : {0}", UrsTime == null ? "null" : UrsTime.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("UrsNumber")) ht["UrsNumber"] = UrsNumber;
			if (!__jsonIgnore.ContainsKey("UrsBirthDay")) ht["UrsBirthDay"] = UrsBirthDay;
			if (!__jsonIgnore.ContainsKey("UrsIDCard")) ht["UrsIDCard"] = UrsIDCard;
			if (!__jsonIgnore.ContainsKey("UrsMobile")) ht["UrsMobile"] = UrsMobile;
			if (!__jsonIgnore.ContainsKey("UrsQQ")) ht["UrsQQ"] = UrsQQ;
			if (!__jsonIgnore.ContainsKey("UrsTime")) ht["UrsTime"] = UrsTime;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(UrsuserInfo).GetField("JsonIgnore");
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
			UrsuserInfo item = obj as UrsuserInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(UrsuserInfo op1, UrsuserInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(UrsuserInfo op1, UrsuserInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public int? UrsNumber {
			get { return _UrsNumber; }
			set { _UrsNumber = value; }
		}
		public int? UrsBirthDay {
			get { return _UrsBirthDay; }
			set { _UrsBirthDay = value; }
		}
		public string UrsIDCard {
			get { return _UrsIDCard; }
			set { _UrsIDCard = value; }
		}
		public string UrsMobile {
			get { return _UrsMobile; }
			set { _UrsMobile = value; }
		}
		public int? UrsQQ {
			get { return _UrsQQ; }
			set { _UrsQQ = value; }
		}
		public DateTime? UrsTime {
			get { return _UrsTime; }
			set { _UrsTime = value; }
		}
		#endregion

		public DC2016.DAL.Ursuser.SqlUpdateBuild UpdateDiy {
			get { return DC2016.BLL.Ursuser.UpdateDiy(this, _UrsNumber); }
		}
	}
}

