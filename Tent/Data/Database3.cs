using System;

namespace Tent.Data
{
    public partial class Database
    {
        public Database Sql(string sql) {
            query.Sql(sql);
            return this;
        }

        public Database Parameter(string name, object value) {
            query.Parameter(name, value);
            return this;
        }

        public Database Sproc(string name) {
            query.Sproc(name);
            return this;
        }

        public Database Cache(string key = null, DateTime? expirationDate = null, int? seconds = null) {
            cacheKey = key;

            if (expirationDate.HasValue) {
                var duration = expirationDate.Value.Subtract(DateTime.Now);
                cacheSeconds = (int)duration.TotalSeconds;
            } else if (seconds.HasValue) {
                cacheSeconds = seconds.Value;
            } else {
                cacheSeconds = 60; // default, if not set
            }

            return this;
        }
        string cacheKey;
        int cacheSeconds;

        void setCache(object obj) {
            if (cacheKey != null) {
                cache.Set(cacheKey, obj, cacheSeconds);
                cacheKey = null;
                cacheSeconds = 60;
            }
        }
    }
}
