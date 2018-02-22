using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using DC2016.Model;

namespace DC2016.BLL {

	public partial class D2unactive {

		protected static readonly DC2016.DAL.D2unactive dal = new DC2016.DAL.D2unactive();
		protected static readonly int itemCacheTimeout;

		static D2unactive() {
			var ini = IniHelper.LoadIni(@"../web.config");
			if (ini.ContainsKey("appSettings") && !int.TryParse(ini["appSettings"]["DC2016_ITEM_CACHE_TIMEOUT_D2unactive"], out itemCacheTimeout))
				int.TryParse(ini["appSettings"]["DC2016_ITEM_CACHE_TIMEOUT"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(string UavGUID) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(UavGUID));
			return dal.Delete(UavGUID);
		}

		public static int Update(D2unactiveInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static DC2016.DAL.D2unactive.SqlUpdateBuild UpdateDiy(string UavGUID) {
			return UpdateDiy(null, UavGUID);
		}
		public static DC2016.DAL.D2unactive.SqlUpdateBuild UpdateDiy(D2unactiveInfo item, string UavGUID) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(UavGUID));
			return new DC2016.DAL.D2unactive.SqlUpdateBuild(item, UavGUID);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static DC2016.DAL.D2unactive.SqlUpdateBuild UpdateDiyDangerous {
			get { return new DC2016.DAL.D2unactive.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 D2unactive.Insert(D2unactiveInfo item)
		/// </summary>
		[Obsolete]
		public static D2unactiveInfo Insert(string UavGUID, string UavEMail, int? UavFlag, string UavGate, string UavGateSrc, int? UavNumber, int? UavState, DateTime? UavTime1, DateTime? UavTime2) {
			return Insert(new D2unactiveInfo {
				UavGUID = UavGUID, 
				UavEMail = UavEMail, 
				UavFlag = UavFlag, 
				UavGate = UavGate, 
				UavGateSrc = UavGateSrc, 
				UavNumber = UavNumber, 
				UavState = UavState, 
				UavTime1 = UavTime1, 
				UavTime2 = UavTime2});
		}
		public static D2unactiveInfo Insert(D2unactiveInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(D2unactiveInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("DC2016_BLL_D2unactive_", item.UavGUID));
		}
		#endregion

		public static D2unactiveInfo GetItem(string UavGUID) {
			if (UavGUID == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(UavGUID);
			string key = string.Concat("DC2016_BLL_D2unactive_", UavGUID);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new D2unactiveInfo(value); } catch { }
			D2unactiveInfo item = dal.GetItem(UavGUID);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<D2unactiveInfo> GetItems() {
			return Select.ToList();
		}
		public static D2unactiveSelectBuild Select {
			get { return new D2unactiveSelectBuild(dal); }
		}
	}
	public partial class D2unactiveSelectBuild : SelectBuild<D2unactiveInfo, D2unactiveSelectBuild> {
		public D2unactiveSelectBuild WhereUavGUID(params string[] UavGUID) {
			return this.Where1Or("a.`UavGUID` = {0}", UavGUID);
		}
		public D2unactiveSelectBuild WhereUavEMail(params string[] UavEMail) {
			return this.Where1Or("a.`UavEMail` = {0}", UavEMail);
		}
		public D2unactiveSelectBuild WhereUavFlag(params int?[] UavFlag) {
			return this.Where1Or("a.`UavFlag` = {0}", UavFlag);
		}
		public D2unactiveSelectBuild WhereUavGate(params string[] UavGate) {
			return this.Where1Or("a.`UavGate` = {0}", UavGate);
		}
		public D2unactiveSelectBuild WhereUavGateSrc(params string[] UavGateSrc) {
			return this.Where1Or("a.`UavGateSrc` = {0}", UavGateSrc);
		}
		public D2unactiveSelectBuild WhereUavNumber(params int?[] UavNumber) {
			return this.Where1Or("a.`UavNumber` = {0}", UavNumber);
		}
		public D2unactiveSelectBuild WhereUavState(params int?[] UavState) {
			return this.Where1Or("a.`UavState` = {0}", UavState);
		}
		public D2unactiveSelectBuild WhereUavTime1Range(DateTime? begin) {
			return base.Where("a.`UavTime1` >= {0}", begin) as D2unactiveSelectBuild;
		}
		public D2unactiveSelectBuild WhereUavTime1Range(DateTime? begin, DateTime? end) {
			if (end == null) return WhereUavTime1Range(begin);
			return base.Where("a.`UavTime1` between {0} and {1}", begin, end) as D2unactiveSelectBuild;
		}
		public D2unactiveSelectBuild WhereUavTime2Range(DateTime? begin) {
			return base.Where("a.`UavTime2` >= {0}", begin) as D2unactiveSelectBuild;
		}
		public D2unactiveSelectBuild WhereUavTime2Range(DateTime? begin, DateTime? end) {
			if (end == null) return WhereUavTime2Range(begin);
			return base.Where("a.`UavTime2` between {0} and {1}", begin, end) as D2unactiveSelectBuild;
		}
		protected new D2unactiveSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as D2unactiveSelectBuild;
		}
		public D2unactiveSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}