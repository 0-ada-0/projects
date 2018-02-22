using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using DC2016.Model;

namespace DC2016.BLL {

	public partial class Validinfo {

		protected static readonly DC2016.DAL.Validinfo dal = new DC2016.DAL.Validinfo();
		protected static readonly int itemCacheTimeout;

		static Validinfo() {
			var ini = IniHelper.LoadIni(@"../web.config");
			if (ini.ContainsKey("appSettings") && !int.TryParse(ini["appSettings"]["DC2016_ITEM_CACHE_TIMEOUT_Validinfo"], out itemCacheTimeout))
				int.TryParse(ini["appSettings"]["DC2016_ITEM_CACHE_TIMEOUT"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(int? Pkid) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Pkid));
			return dal.Delete(Pkid);
		}

		public static int Update(ValidinfoInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static DC2016.DAL.Validinfo.SqlUpdateBuild UpdateDiy(int? Pkid) {
			return UpdateDiy(null, Pkid);
		}
		public static DC2016.DAL.Validinfo.SqlUpdateBuild UpdateDiy(ValidinfoInfo item, int? Pkid) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Pkid));
			return new DC2016.DAL.Validinfo.SqlUpdateBuild(item, Pkid);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static DC2016.DAL.Validinfo.SqlUpdateBuild UpdateDiyDangerous {
			get { return new DC2016.DAL.Validinfo.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Validinfo.Insert(ValidinfoInfo item)
		/// </summary>
		[Obsolete]
		public static ValidinfoInfo Insert(DateTime? Addtime, string Code, string Gate, string Mobile, int? Number, string Param, int? Server, int? State, int? Type, DateTime? Updatetime, int? Userid) {
			return Insert(new ValidinfoInfo {
				Addtime = Addtime, 
				Code = Code, 
				Gate = Gate, 
				Mobile = Mobile, 
				Number = Number, 
				Param = Param, 
				Server = Server, 
				State = State, 
				Type = Type, 
				Updatetime = Updatetime, 
				Userid = Userid});
		}
		public static ValidinfoInfo Insert(ValidinfoInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(ValidinfoInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("DC2016_BLL_Validinfo_", item.Pkid));
		}
		#endregion

		public static ValidinfoInfo GetItem(int? Pkid) {
			if (Pkid == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Pkid);
			string key = string.Concat("DC2016_BLL_Validinfo_", Pkid);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new ValidinfoInfo(value); } catch { }
			ValidinfoInfo item = dal.GetItem(Pkid);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<ValidinfoInfo> GetItems() {
			return Select.ToList();
		}
		public static ValidinfoSelectBuild Select {
			get { return new ValidinfoSelectBuild(dal); }
		}
	}
	public partial class ValidinfoSelectBuild : SelectBuild<ValidinfoInfo, ValidinfoSelectBuild> {
		public ValidinfoSelectBuild WherePkid(params int?[] Pkid) {
			return this.Where1Or("a.`Pkid` = {0}", Pkid);
		}
		public ValidinfoSelectBuild WhereAddtimeRange(DateTime? begin) {
			return base.Where("a.`Addtime` >= {0}", begin) as ValidinfoSelectBuild;
		}
		public ValidinfoSelectBuild WhereAddtimeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereAddtimeRange(begin);
			return base.Where("a.`Addtime` between {0} and {1}", begin, end) as ValidinfoSelectBuild;
		}
		public ValidinfoSelectBuild WhereCode(params string[] Code) {
			return this.Where1Or("a.`Code` = {0}", Code);
		}
		public ValidinfoSelectBuild WhereGate(params string[] Gate) {
			return this.Where1Or("a.`Gate` = {0}", Gate);
		}
		public ValidinfoSelectBuild WhereMobile(params string[] Mobile) {
			return this.Where1Or("a.`Mobile` = {0}", Mobile);
		}
		public ValidinfoSelectBuild WhereNumber(params int?[] Number) {
			return this.Where1Or("a.`Number` = {0}", Number);
		}
		public ValidinfoSelectBuild WhereParam(params string[] Param) {
			return this.Where1Or("a.`Param` = {0}", Param);
		}
		public ValidinfoSelectBuild WhereServer(params int?[] Server) {
			return this.Where1Or("a.`Server` = {0}", Server);
		}
		public ValidinfoSelectBuild WhereState(params int?[] State) {
			return this.Where1Or("a.`State` = {0}", State);
		}
		public ValidinfoSelectBuild WhereType(params int?[] Type) {
			return this.Where1Or("a.`Type` = {0}", Type);
		}
		public ValidinfoSelectBuild WhereUpdatetimeRange(DateTime? begin) {
			return base.Where("a.`Updatetime` >= {0}", begin) as ValidinfoSelectBuild;
		}
		public ValidinfoSelectBuild WhereUpdatetimeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereUpdatetimeRange(begin);
			return base.Where("a.`Updatetime` between {0} and {1}", begin, end) as ValidinfoSelectBuild;
		}
		public ValidinfoSelectBuild WhereUserid(params int?[] Userid) {
			return this.Where1Or("a.`Userid` = {0}", Userid);
		}
		protected new ValidinfoSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as ValidinfoSelectBuild;
		}
		public ValidinfoSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}