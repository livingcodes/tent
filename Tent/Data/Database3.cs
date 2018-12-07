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

        /// <summary>
        /// Allows data to be paged through like a book. Requires ORDER BY in query.
        /// </summary>
        /// <param name="page">Page number to query</param>
        /// <param name="take">How many rows to take for a page</param>
        public Database Paging(int page = 1, int take = 10) {
            hasPaging = true;
            this.page = page;
            this.take = take;
            pagingSql = @"
                OFFSET((@Page - 1) * @Take) ROWS
                FETCH NEXT @Take ROWS ONLY";
            return this;
        }
        int page, take;
        string pagingSql;
        bool hasPaging;

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
