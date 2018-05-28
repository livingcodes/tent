using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Tent.Data
{
    public interface IDatabase
    {
        T Select<T>(int id);
        List<T> Select<T>(string sql, params object[] parameters);
        //IDatabase Sproc(string name);
        int Insert<T>(T obj);
        int Update<T>(T obj);
        int Delete<T>(int id);
    }
    
    public partial class Database : IDatabase
    {
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
    }
    //public static class DatabaseExtensions
    //{
    //    public static void Truncate<T>(this Database db) {
    //        var tableName = db.TableName.Get<T>();
    //        db.Execute($"truncate table {tableName}");
    //    }
    //}
}