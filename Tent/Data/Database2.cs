using System.Collections.Generic;

namespace Tent.Data
{
    public partial class Database : IDatabase
    {
        public Database(
            IConnectionFactory connectionFactory, 
            IRead reader,
            ICache cache,
            ITableName tableName = null
        ) {
            this.reader = reader;
            this.connectionFactory = connectionFactory;
            this.cache = cache;
            this.tableName = tableName ?? new TableName_ClassName();
            this.getParameterNamesFromSql = new GetParameterNamesFromSql();
        }
        IConnectionFactory connectionFactory;
        IRead reader;
        ICache cache;
        ITableName tableName;
        GetParameterNamesFromSql getParameterNamesFromSql;

        IQuery query { get {
            if (_query == null)
                _query = new Query(connectionFactory, reader);
            return _query;
        } }
        IQuery _query;

        void setQueryToNull() {
            _query = null;
        }

        public List<T> Select<T>(string sql = null, params object[] parameters) {
        // get from cache
            if (cacheKey != null) {
                var cacheValue = cache.Get<List<T>>(cacheKey);
                if (cacheValue != null)
                    return (List<T>)cacheValue;
            }

        // sql syntax
            if (sql == null)
                sql = query.Sql();
            
            if (!query.IsSproc && (
                sql.ToUpper().StartsWith("WHERE ")
            ||  sql.ToUpper().StartsWith("GROUP BY ")
            ||  sql.ToUpper().StartsWith("ORDER BY ")
            ) ) {
                var tableName = this.tableName.Get<T>();
                sql = sql ?? query.Sql();
                var sqlStart = $"SELECT * FROM [{tableName}] ";
                sql = sqlStart + sql;
            }

            query.Sql(sql);

        // parameters
            if (!query.IsSproc) {
                var parameterNames = getParameterNamesFromSql.Execute(query.Sql());
                // todo: are count and length checks necessary
                if (parameterNames.Count > 0) {
                    if (parameters.Length > 0) {
                        if (parameters.Length != parameterNames.Count)
                            throw new System.Exception($"Parameter name and value counts are not equal. Parameter name count: {parameterNames.Count}, Parameter value count: {parameters.Length}");
                        for (var i = 0; i < parameters.Length; i++)
                            query.Parameter(parameterNames[i], parameters[i]);
                    }
                }
            }

            var list = query.Select<T>();

        // set cache
            setCache(list);
            setQueryToNull();
            return list;
        }

        public T SelectOne<T>(string sql = null, params object[] parameters) {
        // get from cache
            if (cacheKey != null) {
                var cacheValue = cache.Get<T>(cacheKey);
                if (cacheValue != null)
                    return (T)cacheValue;
            }

        // sql syntax
            if (sql == null)
                sql = query.Sql();
            
            if (!query.IsSproc && (
                sql.ToUpper().StartsWith("WHERE ")
            ||  sql.ToUpper().StartsWith("GROUP BY ")
            ||  sql.ToUpper().StartsWith("ORDER BY ")
            ) ) {
                var tableName = this.tableName.Get<T>();
                sql = sql ?? query.Sql();
                var sqlStart = $"SELECT * FROM [{tableName}] ";
                sql = sqlStart + sql;
            }

            query.Sql(sql);

        // get from database
            if (!query.IsSproc) {
                var parameterNames = getParameterNamesFromSql.Execute(query.Sql());
                if (parameterNames.Count > 0)
                    for (var i = 0; i < parameters.Length; i++)
                        query.Parameter(parameterNames[i], parameters[i]);
            }
            var item = query.SelectOne<T>();

        // set cache
            setCache(item);
            setQueryToNull();

            return item;
        }

        public T Select<T>(int id) {
            return SelectOne<T>($"select * from {this.tableName.Get<T>()} where id = @id", id);
        }

        public int Execute(string sql = null, params object[] parameters) {
            if (sql != null)
                query.Sql(sql);
            var parameterNames = getParameterNamesFromSql.Execute(query.Sql());
            if (parameterNames.Count > 0)
                for (var i = 0; i < parameters.Length; i++)
                    query.Parameter(parameterNames[i], parameters[i]);
            int affectedRows = query.Execute(query.Sql());
            setQueryToNull();
            return affectedRows;
        }
    }
}