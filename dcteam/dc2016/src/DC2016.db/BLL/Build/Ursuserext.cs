using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using DC2016.Model;

namespace DC2016.BLL {

	public partial class Ursuserext {

		protected static readonly DC2016.DAL.Ursuserext dal = new DC2016.DAL.Ursuserext();
		protected static readonly int itemCacheTimeout;

		static Ursuserext() {
			var ini = IniHelper.LoadIni(@"../web.config");
			if (ini.ContainsKey("appSettings") && !int.TryParse(ini["appSettings"]["DC2016_ITEM_CACHE_TIMEOUT_Ursuserext"], out itemCacheTimeout))
				int.TryParse(ini["appSettings"]["DC2016_ITEM_CACHE_TIMEOUT"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(int? ExtNumber) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(ExtNumber));
			return dal.Delete(ExtNumber);
		}

		public static int Update(UrsuserextInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static DC2016.DAL.Ursuserext.SqlUpdateBuild UpdateDiy(int? ExtNumber) {
			return UpdateDiy(null, ExtNumber);
		}
		public static DC2016.DAL.Ursuserext.SqlUpdateBuild UpdateDiy(UrsuserextInfo item, int? ExtNumber) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(ExtNumber));
			return new DC2016.DAL.Ursuserext.SqlUpdateBuild(item, ExtNumber);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static DC2016.DAL.Ursuserext.SqlUpdateBuild UpdateDiyDangerous {
			get { return new DC2016.DAL.Ursuserext.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Ursuserext.Insert(UrsuserextInfo item)
		/// </summary>
		[Obsolete]
		public static UrsuserextInfo Insert(int? ExtNumber, string ExtCHTInfo) {
			return Insert(new UrsuserextInfo {
				ExtNumber = ExtNumber, 
				ExtCHTInfo = ExtCHTInfo});
		}
		public static UrsuserextInfo Insert(UrsuserextInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(UrsuserextInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("DC2016_BLL_Ursuserext_", item.ExtNumber));
		}
		#endregion

		public static UrsuserextInfo GetItem(int? ExtNumber) {
			if (ExtNumber == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(ExtNumber);
			string key = string.Concat("DC2016_BLL_Ursuserext_", ExtNumber);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new UrsuserextInfo(value); } catch { }
			UrsuserextInfo item = dal.GetItem(ExtNumber);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<UrsuserextInfo> GetItems() {
			return Select.ToList();
		}
		public static UrsuserextSelectBuild Select {
			get { return new UrsuserextSelectBuild(dal); }
		}
	}
	public partial class UrsuserextSelectBuild : SelectBuild<UrsuserextInfo, UrsuserextSelectBuild> {
		public UrsuserextSelectBuild WhereExtNumber(params int?[] ExtNumber) {
			return this.Where1Or("a.`ExtNumber` = {0}", ExtNumber);
		}
		protected new UrsuserextSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as UrsuserextSelectBuild;
		}
		public UrsuserextSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}