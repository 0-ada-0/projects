using System;
using System.Collections.Generic;
using StackExchange.Redis;
using MySql.Data.MySqlClient;

namespace DC2016.BLL {

	public static partial class RedisHelper {
		public static List<TReturnInfo> ToList<TReturnInfo>(this SelectBuild<TReturnInfo> select, int expireSeconds) { return select.ToList(expireSeconds); }
		public static List<TReturnInfo> ToList<TReturnInfo>(this SelectBuild<TReturnInfo> select, TimeSpan expire) { return select.ToList(expire); }

		public static string ConnectionString = "";
		public static ConnectionMultiplexer NewConnection() {
			if (string.IsNullOrEmpty(ConnectionString)) {
				string key = "DC2016RedisConnectionString";
				var ini = IniHelper.LoadIni(@"../web.config");
				if (ini.ContainsKey("connectionStrings")) ConnectionString = ini["connectionStrings"][key];
				if (string.IsNullOrEmpty(ConnectionString)) throw new ArgumentNullException(key, string.Format("未定义 ../web.config 里的 ConnectionStrings 键 '{0}' 或值不正确！", key));
			}
			return ConnectionMultiplexer.ConnectAsync(ConnectionString).Result;
		}

		private static ConnectionMultiplexer _connection;
		private static IDatabase _cache;

		public static ConnectionMultiplexer Connection {
			get {
				if (_connection == null || !_connection.IsConnected || _cache == null)
					_connection = NewConnection();
				return _connection;
			}
		}
		public static IDatabase Cache {
			get {
				if (_cache == null || !_cache.IsConnected("test")) {
					_cache = null;
					_cache = Connection.GetDatabase();
				}
				return _cache;
			}
		}

		public static void Set(string name, string value) {
			Set(name, value, 0);
		}
		public static void Set(string name, string value, int expireSeconds) {
			if (expireSeconds > 0)
				Cache.StringSetAsync(name, value, TimeSpan.FromSeconds(expireSeconds)).Wait();
			else
				Cache.StringSetAsync(name, value).Wait();
		}
		public static string Get(string name) {
			return Cache.StringGetAsync(name).Result;
		}
		public static void Remove(string name) {
			Cache.KeyDeleteAsync(name).Wait();
		}
	}
}