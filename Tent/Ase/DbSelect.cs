﻿using System;
using System.Collections.Generic;
using System.Data;

namespace Ase
{
    public partial class Db
    {
        public IDb Sproc(string name) {
            sprocName = name;
            return this;
        }
        string sprocName;

        public bool IsSproc => sprocName != null;

        public List<T> Select<T>(string sql = null, params object[] parameters) {
            // get from cache
            if (cacheKey != null) {
                var cacheValue = cache.Get<List<T>>(cacheKey);
                if (cacheValue != null)
                    return (List<T>)cacheValue;
            }

            // sql syntax
            if (sql == null)
                sql = this.sql;

            if (!IsSproc && (
               sql.ToUpper().StartsWith("WHERE ")
            || sql.ToUpper().StartsWith("GROUP BY ")
            || sql.ToUpper().StartsWith("ORDER BY ")
            )) {
                var tableName = this.tableName.Get<T>();
                //sql = sql ?? query.Sql();
                var sqlStart = $"SELECT * FROM [{tableName}] ";
                sql = sqlStart + sql;
            }

            //query.Sql(sql);

            // parameters
            if (!IsSproc) {
                var parameterNames = getParameterNamesFromSql.Execute(sql);
                // todo: are count and length checks necessary
                if (parameterNames.Count > 0) {
                    if (parameters.Length > 0) {
                        if (parameters.Length != parameterNames.Count)
                            throw new System.Exception($"Parameter name and value counts are not equal. Parameter name count: {parameterNames.Count}, Parameter value count: {parameters.Length}");
                        for (var i = 0; i < parameters.Length; i++)
                            Parameter(parameterNames[i], parameters[i]);
                    }
                }
            }

            var list = select<T>(sql);

            // set cache
            setCache(list);
            setQueryToNull();
            return list;
        }

        string sql;

        List<T> select<T>(string sql) {
            var list = new List<T>();
            var connection = connectionFactory.Create();
            IDbCommand command = null;
            try {
                connection.Open();
                command = connection.CreateCommand();

                if (sprocName != null) {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = sprocName;
                } else {
                    command.CommandText = sql;
                }
                foreach (var parameter in parameters) {
                    var p = command.CreateParameter();
                    p.ParameterName = parameter.name;
                    p.Value = parameter.value;
                    command.Parameters.Add(p);
                }
                var reader = command.ExecuteReader();
                list = this.reader.ReadList<T>(reader);
            } finally {
                if (command != null)
                    command.Dispose();
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }
            return list;
        }

        public IDb Parameter(string name, object value) {
            parameters.Add((name, value));
            return this;
        }
        List<(string name, object value)> parameters;

        void setQueryToNull() {
            sql = "";
            parameters = new List<(string name, object value)>();
        }

        public IDb Cache(string key = null, DateTime? expirationDate = null, int? seconds = null) {
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
