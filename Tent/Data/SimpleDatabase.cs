using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Tent.Data
{   
    public class SimpleDatabase : IDatabase
    {
        // cache used to get column names
        public SimpleDatabase(
            IConnectionFactory connectionFactory,
            IRead reader,
            ITableName tableName,
            ICache cache
        ) {
            this.connectionFactory = connectionFactory;
            this.reader = reader;
            this.tableName = tableName;
            this.cache = cache;
        }

        IConnectionFactory connectionFactory;
        IRead reader;
        ITableName tableName;
        ICache cache;

        /// <summary>Insert object into datbase. Returns id.</summary>
        public int Insert<T>(T instance) {
            var connection = connectionFactory.Create();
            SqlCommand command = null;
            int rowsAffected = 0;
            
            try {
                connection.Open();
                command = (SqlCommand)connection.CreateCommand();
                ISqlBuilder sqlBuilder = new SqlBuilder<T>(instance, command, this, cache, tableName);
                var sql = sqlBuilder.BuildInsertSql();
                command.CommandText = sql;
                rowsAffected = command.ExecuteNonQuery();
            } finally {
                if (command != null)
                    command.Dispose();
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }
            return rowsAffected;
        }

        public int Update<T>(T instance) {
            var connection = connectionFactory.Create();
            SqlCommand command = null;
            int rowsAffected = 0;

            try {
                connection.Open();
                command = (SqlCommand)connection.CreateCommand();
                ISqlBuilder sqlBuilder = new SqlBuilder<T>(instance, command, this, cache, tableName);
                var sql = sqlBuilder.BuildUpdateSql();
                command.CommandText = sql;
                rowsAffected = command.ExecuteNonQuery();
            } finally {
                if (command != null)
                    command.Dispose();
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }
            return rowsAffected;
        }

        public int Delete<T>(int id) {
            var table = tableName.Get<T>();
            int rowsAffected = 0;
            var connection = connectionFactory.Create();
            SqlCommand command = null;
            try {
                connection.Open();
                command = (SqlCommand)connection.CreateCommand();
                command.CommandText = $"delete from {table} where id = {id}";
                rowsAffected = command.ExecuteNonQuery();
            } finally {
                if (command != null)
                    command.Dispose();
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }
            return rowsAffected;
        }

        public int Execute(string sql, params object[] parameters) {
            var connection = connectionFactory.Create();
            int affectedRows = -1;
            IDbCommand command = null;
            try {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = sql;
                var parameterNames = new GetParameterNamesFromSql().Execute(sql);
                if (parameters.Length != parameterNames.Count)
                    throw new Exception($"Parameter name and value counts are not equal. Parameter name count: {parameterNames.Count}, Parameter value count: {parameters.Length}");
                for (var i = 0; i < parameters.Length; i++) {
                    var p = command.CreateParameter();
                    p.ParameterName = parameterNames[i];
                    p.Value = parameters[i];
                    command.Parameters.Add(p);
                }
                affectedRows = command.ExecuteNonQuery();
            } finally {
                if (command != null)
                    command.Dispose();
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }
            return affectedRows;
        }

        // todo: unit test
        public List<T> Select<T>(string sql, params object[] parameters) {
            var list = new List<T>();
            var connection = connectionFactory.Create();
            IDbCommand command = null;
            try {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = sql;
                // parameters
                var parameterNames = new GetParameterNamesFromSql().Execute(sql);
                if (parameters.Length != parameterNames.Count)
                    throw new Exception($"Parameter name and value counts are not equal. Parameter name count: {parameterNames.Count}, Parameter value count: {parameters.Length}");
                for (var i = 0; i < parameters.Length; i++) {
                    var p = command.CreateParameter();
                    p.ParameterName = parameterNames[i];
                    p.Value = parameters[i];
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

        // todo: unit test
        public T SelectOne<T>(string sql, params object[] parameters) {
            T item = default(T);
            var connection = connectionFactory.Create();
            IDbCommand command = null;
            try {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = sql;
                // parameters
                var parameterNames = new GetParameterNamesFromSql().Execute(sql);
                if (parameterNames.Count > 0 && parameters.Length > 0) {
                    if (parameters.Length != parameterNames.Count)
                        throw new System.Exception($"Parameter name and value counts are not equal. Parameter name count: {parameterNames.Count}, Parameter value count: {parameters.Length}");
                    for (var i = 0; i < parameters.Length; i++) {
                        var p = command.CreateParameter();
                        p.ParameterName = parameterNames[i];
                        p.Value = parameters[i];
                        command.Parameters.Add(p);
                    }
                }
                var reader = command.ExecuteReader();
                item = this.reader.Read<T>(reader);
            } finally {
                if (command != null)
                    command.Dispose();
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }
            return item;
        }

        // todo: unit test
        public T Select<T>(int id) {
            var sql = $"select * from {tableName.Get<T>()} where id = " + id;
            return SelectOne<T>(sql);
        }
    }
}