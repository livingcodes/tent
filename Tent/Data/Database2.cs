using System;
using System.Collections.Generic;

namespace Tent.Data
{
    public partial class Database
    {
        public Database(
            IConnectionFactory connectionFactory, 
            IRead reader,
            ICache cache
        ) {
            this.reader = reader;
            this.connectionFactory = connectionFactory;
            this.cache = cache;
        }
        string sql;
        IConnectionFactory connectionFactory;
        IRead reader;
        ICache cache;

        IQuery query { get {
            if (_query == null)
                _query = new Query(connectionFactory, reader);
            return _query;
        } }
        IQuery _query;

        void setQueryToNull() {
            _query = null;
        }

        public Database Sql(string sql) {
            query.Sql(sql);
            return this;
        }

        public Database Parameter(string name, object value) {
            query.Parameter(name, value);
            return this;
        }

        public Database Cache(string key = null, System.DateTime? expirationDate = null, int? seconds = null) {
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

        public List<T> Select<T>(string sql = null, params object[] parameters) {
        // get from cache
            if (cacheKey != null) {
                var cacheValue = cache.Get<List<T>>(cacheKey);
                if (cacheValue != null)
                    return (List<T>)cacheValue;
            }

        // get from database
            if (sql != null)
                query.Sql(sql);
            
            var parameterNames = getParameterNamesFromSql(query.Sql());
            if (parameterNames.Count > 0) {
                if (parameters.Length > 0) {
                    if (parameters.Length != parameterNames.Count)
                        throw new System.Exception($"Parameter name and value counts are not equal. Parameter name count: {parameterNames.Count}, Parameter value count: {parameters.Length}");
                    for (var i = 0; i < parameters.Length; i++)
                        query.Parameter(parameterNames[i], parameters[i]);
                }
            }

            var list = query.Select<T>();

        // set cache
            setCache(list);
            setQueryToNull();
            return list;
        }

        void setCache(object obj) {
            if (cacheKey != null) {
                cache.Set(cacheKey, obj, cacheSeconds);
                cacheKey = null;
                cacheSeconds = 60;
            }
        }

        public T SelectOne<T>(string sql = null, params object[] parameters) {
        // get from cache
            if (cacheKey != null) {
                var cacheValue = cache.Get<T>(cacheKey);
                if (cacheValue != null)
                    return (T)cacheValue;
            }

        // get from database
            if (sql != null)
                query.Sql(sql);
            var parameterNames = getParameterNamesFromSql(query.Sql());
            if (parameterNames.Count > 0)
                for (var i = 0; i < parameters.Length; i++)
                    query.Parameter(parameterNames[i], parameters[i]);
            var item = query.SelectOne<T>();

        // set cache
            setCache(item);

            setQueryToNull();

            return item;
        }

        public T Select<T>(int id) {
            return SelectOne<T>($"select * from {typeof(T).Name}s where id = @id", id);
        }

        public int Execute(string sql = null, params object[] parameters) {
            if (sql != null)
                query.Sql(sql);
            var parameterNames = getParameterNamesFromSql(query.Sql());
            if (parameterNames.Count > 0)
                for (var i = 0; i < parameters.Length; i++)
                    query.Parameter(parameterNames[i], parameters[i]);
            int affectedRows = query.Execute(query.Sql());
            setQueryToNull();
            return affectedRows;
        }

        public void DropTable(string tableName) {
            Select<int>($"drop table {tableName}");
        }

        List<string> getParameterNamesFromSql(string sql) {
            var parameters = new Dictionary<string, int>();
            var index = sql.IndexOf('@');
            while (index > -1) {
                var endIndex = sql.IndexOfAny(new char[] {' ', ',', ')'}, index);
                if (endIndex == -1)
                    endIndex = sql.Length - 1;
                else
                    endIndex -= 1;
                
                var parameterName = sql.Substring(index, endIndex - index + 1);
                if (!parameters.ContainsKey(parameterName))
                    parameters.Add(parameterName, index);
                index = sql.IndexOf('@', index + 1);
            };
            var keys = new List<string>();
            foreach (var key in parameters.Keys)
                keys.Add(key);
            return keys;
        }
    }

    public class Admin
    {
        public Admin(Database db) {
            this.db = db;
        }
        Database db;
        public void DropTable(string tableName) {
            db.Select<int>($"drop table {tableName}");
        }
    }
}