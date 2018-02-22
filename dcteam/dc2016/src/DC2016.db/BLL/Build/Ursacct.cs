using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using DC2016.Model;

namespace DC2016.BLL {

	public partial class Ursacct {

		protected static readonly DC2016.DAL.Ursacct dal = new DC2016.DAL.Ursacct();
		protected static readonly int itemCacheTimeout;

		static Ursacct() {
			var ini = IniHelper.LoadIni(@"../web.config");
			if (ini.ContainsKey("appSettings") && !int.TryParse(ini["appSettings"]["DC2016_ITEM_CACHE_TIMEOUT_Ursacct"], out itemCacheTimeout))
				int.TryParse(ini["appSettings"]["DC2016_ITEM_CACHE_TIMEOUT"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(string AcctEMail) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(AcctEMail));
			return dal.Delete(AcctEMail);
		}

		public static int Update(UrsacctInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static DC2016.DAL.Ursacct.SqlUpdateBuild UpdateDiy(string AcctEMail) {
			return UpdateDiy(null, AcctEMail);
		}
		public static DC2016.DAL.Ursacct.SqlUpdateBuild UpdateDiy(UrsacctInfo item, string AcctEMail) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(AcctEMail));
			return new DC2016.DAL.Ursacct.SqlUpdateBuild(item, AcctEMail);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static DC2016.DAL.Ursacct.SqlUpdateBuild UpdateDiyDangerous {
			get { return new DC2016.DAL.Ursacct.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Ursacct.Insert(UrsacctInfo item)
		/// </summary>
		[Obsolete]
		public static UrsacctInfo Insert(string AcctEMail, int? AcctNumber) {
			return Insert(new UrsacctInfo {
				AcctEMail = AcctEMail, 
				AcctNumber = AcctNumber});
		}
		public static UrsacctInfo Insert(UrsacctInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(UrsacctInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("DC2016_BLL_Ursacct_", item.AcctEMail));
		}
		#endregion

		public static UrsacctInfo GetItem(string AcctEMail) {
			if (AcctEMail == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(AcctEMail);
			string key = string.Concat("DC2016_BLL_Ursacct_", AcctEMail);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new UrsacctInfo(value); } catch { }
			UrsacctInfo item = dal.GetItem(AcctEMail);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<UrsacctInfo> GetItems() {
			return Select.ToList();
		}
		public static UrsacctSelectBuild Select {
			get { return new UrsacctSelectBuild(dal); }
		}
	}
	public partial class UrsacctSelectBuild : SelectBuild<UrsacctInfo, UrsacctSelectBuild> {
		public UrsacctSelectBuild WhereAcctEMail(params string[] AcctEMail) {
			return this.Where1Or("a.`AcctEMail` = {0}", AcctEMail);
		}
		public UrsacctSelectBuild WhereAcctNumber(params int?[] AcctNumber) {
			return this.Where1Or("a.`AcctNumber` = {0}", AcctNumber);
		}
		protected new UrsacctSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as UrsacctSelectBuild;
		}
		public UrsacctSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}