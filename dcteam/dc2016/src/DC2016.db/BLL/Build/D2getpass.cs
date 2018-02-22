using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using DC2016.Model;
using static DC2016.DAL.SqlHelper;

namespace DC2016.BLL {

	public partial class D2getpass {

		protected static readonly DC2016.DAL.D2getpass dal = new DC2016.DAL.D2getpass();
		protected static readonly int itemCacheTimeout;

		static D2getpass() {
			var ini = IniHelper.LoadIni(@"../web.config");
			if (ini.ContainsKey("appSettings") && !int.TryParse(ini["appSettings"]["DC2016_ITEM_CACHE_TIMEOUT_D2getpass"], out itemCacheTimeout))
				int.TryParse(ini["appSettings"]["DC2016_ITEM_CACHE_TIMEOUT"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(string GtpsGUID) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(GtpsGUID));
			return dal.Delete(GtpsGUID);
		}

		public static int Update(D2getpassInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static DC2016.DAL.D2getpass.SqlUpdateBuild UpdateDiy(string GtpsGUID) {
			return UpdateDiy(null, GtpsGUID);
		}
		public static DC2016.DAL.D2getpass.SqlUpdateBuild UpdateDiy(D2getpassInfo item, string GtpsGUID) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(GtpsGUID));
			return new DC2016.DAL.D2getpass.SqlUpdateBuild(item, GtpsGUID);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static DC2016.DAL.D2getpass.SqlUpdateBuild UpdateDiyDangerous {
			get { return new DC2016.DAL.D2getpass.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 D2getpass.Insert(D2getpassInfo item)
		/// </summary>
		[Obsolete]
		public static D2getpassInfo Insert(string GtpsGUID, string GtpsEMail, string GtpsGate, int? GtpsIP, int? GtpsNumber, int? GtpsState, DateTime? GtpsTime1, DateTime? GtpsTime2, int? GtpsType) {
			return Insert(new D2getpassInfo {
				GtpsGUID = GtpsGUID, 
				GtpsEMail = GtpsEMail, 
				GtpsGate = GtpsGate, 
				GtpsIP = GtpsIP, 
				GtpsNumber = GtpsNumber, 
				GtpsState = GtpsState, 
				GtpsTime1 = GtpsTime1, 
				GtpsTime2 = GtpsTime2, 
				GtpsType = GtpsType});
		}
		public static D2getpassInfo Insert(D2getpassInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(D2getpassInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("DC2016_BLL_D2getpass_", item.GtpsGUID));
		}
		#endregion

		public static D2getpassInfo GetItem(string GtpsGUID) {
			if (GtpsGUID == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(GtpsGUID);
			string key = string.Concat("DC2016_BLL_D2getpass_", GtpsGUID);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new D2getpassInfo(value); } catch { }
			D2getpassInfo item = dal.GetItem(GtpsGUID);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<D2getpassInfo> GetItems() {
			return Select.ToList();
		}
		public static D2getpassSelectBuild Select {
			get { return new D2getpassSelectBuild(dal); }
		}
	}
	public partial class D2getpassSelectBuild : SelectBuild<D2getpassInfo, D2getpassSelectBuild> {
		public D2getpassSelectBuild WhereGtpsGUID(params string[] GtpsGUID) {
			return this.Where1Or("a.`GtpsGUID` = {0}", GtpsGUID);
		}
		public D2getpassSelectBuild WhereGtpsEMail(params string[] GtpsEMail) {
			return this.Where1Or("a.`GtpsEMail` = {0}", GtpsEMail);
		}
		public D2getpassSelectBuild WhereGtpsGate(params string[] GtpsGate) {
			return this.Where1Or("a.`GtpsGate` = {0}", GtpsGate);
		}
		public D2getpassSelectBuild WhereGtpsIP(params int?[] GtpsIP) {
			return this.Where1Or("a.`GtpsIP` = {0}", GtpsIP);
		}
		public D2getpassSelectBuild WhereGtpsNumber(params int?[] GtpsNumber) {
			return this.Where1Or("a.`GtpsNumber` = {0}", GtpsNumber);
		}
		public D2getpassSelectBuild WhereGtpsState(params int?[] GtpsState) {
			return this.Where1Or("a.`GtpsState` = {0}", GtpsState);
		}
		public D2getpassSelectBuild WhereGtpsTime1Range(DateTime? begin) {
			return base.Where("a.`GtpsTime1` >= {0}", begin) as D2getpassSelectBuild;
		}
		public D2getpassSelectBuild WhereGtpsTime1Range(DateTime? begin, DateTime? end) {
			if (end == null) return WhereGtpsTime1Range(begin);
			return base.Where("a.`GtpsTime1` between {0} and {1}", begin, end) as D2getpassSelectBuild;
		}
		public D2getpassSelectBuild WhereGtpsTime2Range(DateTime? begin) {
			return base.Where("a.`GtpsTime2` >= {0}", begin) as D2getpassSelectBuild;
		}
		public D2getpassSelectBuild WhereGtpsTime2Range(DateTime? begin, DateTime? end) {
			if (end == null) return WhereGtpsTime2Range(begin);
			return base.Where("a.`GtpsTime2` between {0} and {1}", begin, end) as D2getpassSelectBuild;
		}
		public D2getpassSelectBuild WhereGtpsType(params int?[] GtpsType) {
			return this.Where1Or("a.`GtpsType` = {0}", GtpsType);
		}
		protected new D2getpassSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as D2getpassSelectBuild;
		}
		public D2getpassSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}