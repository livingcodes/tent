﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Tent
{
    public interface IDatabase
    {
        List<T> Query<T>(string sql);
        int Insert<T>(T obj);
        int Update<T>(T obj);
    }

    public class Database : IDatabase
    {
        public Database(string connectionString) {
            this.connectionString = connectionString;
        }

        string connectionString;

        public List<T> Query<T>(string sql) {
            var list = new List<T>();
            var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var connection = new SqlConnection(connectionString);
            IDbCommand command = null;
            try {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = sql;
                var reader = command.ExecuteReader();
                while (reader.Read()) {
                    var item = System.Activator.CreateInstance<T>();
                    foreach (var property in properties) {
                        property.SetValue(item, reader[property.Name]);
                    }
                    list.Add(item);
                }
            } finally {
                if (command != null)
                    command.Dispose();
                if (connection.State != System.Data.ConnectionState.Closed)
                    connection.Close();
            }
            return list;
        }

        /// <summary>Insert object into datbase. Returns number of rows affected</summary>
        public int Insert<T>(T instance) {
            var connection = new SqlConnection(connectionString);
            SqlCommand command = null;
            int rowsAffected = 0;
            
            try {
                connection.Open();
                command = connection.CreateCommand();
                ISqlBuilder sqlBuilder = new SqlBuilder<T>(instance, command);
                var sql = sqlBuilder.BuildSql();
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

        public int Update<T>(T obj) => throw new System.NotImplementedException();
    }

    public interface ISqlBuilder
    {
        string BuildSql();
    }
    public class SqlBuilder<T> : ISqlBuilder
    {
        public SqlBuilder(T instance, SqlCommand command) {
            this.instance = instance;
            this.command = command;
        }
        T instance;
        SqlCommand command;
        
        public string BuildSql() {
            var tableName = instance.GetType().Name + "s";
            var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var columnNames = "";
            var values = "";
            foreach (var property in properties) {
                if (property.Name.ToUpper() == "ID")
                    continue;
                
                var value = property.GetValue(instance);
                if (value is null)
                    value = DBNull.Value;
                columnNames += property.Name + ", ";

                command.Parameters.AddWithValue("@" + property.Name, value);
                values += "@" + property.Name + ", ";
            }
            columnNames = columnNames.Substring(0, columnNames.Length - 2);
            values = values.Substring(0, values.Length - 2);
            var sql = $@"INSERT INTO {tableName} ({columnNames}) VALUES ({values})";
            return sql;
        }
    }
}
