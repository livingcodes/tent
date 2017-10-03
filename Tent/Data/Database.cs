﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace Tent.Data
{
    public interface IDatabase
    {
        T Select<T>(int id);
        List<T> Select<T>(string sql, params object[] parameters);
        int Insert<T>(T obj);
        int Update<T>(T obj);
        int Delete<T>(int id);
    }
    
    public class Database : IDatabase
    {
        public Database(string connectionString) {
            this.connectionString = connectionString;
        }

        string connectionString;

        public T Select<T>(int id) {
            T item = default(T);
            var table = typeof(T).Name + "s";
            var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var connection = new SqlConnection(connectionString);
            IDbCommand command = null;
            try {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = $"select * from {table} where id = {id}";
                var reader = command.ExecuteReader();
                item = new ReaderToClass<T>().Convert(reader);
            } finally {
                if (command != null)
                    command.Dispose();
                if (connection.State != System.Data.ConnectionState.Closed)
                    connection.Close();
            }
            return item;
        }

        public List<T> Select<T>(string sql, params object[] parameters) {
            var list = new List<T>();
            var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var connection = new SqlConnection(connectionString);
            IDbCommand command = null;
            try {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = sql;
                var reader = command.ExecuteReader();
                list = new ReaderToList<T>().Convert(reader);
            } finally {
                if (command != null)
                    command.Dispose();
                if (connection.State != System.Data.ConnectionState.Closed)
                    connection.Close();
            }
            return list;
        }

        /// <summary>Insert object into datbase. Returns id.</summary>
        public int Insert<T>(T instance) {
            var connection = new SqlConnection(connectionString);
            SqlCommand command = null;
            int rowsAffected = 0;
            
            try {
                connection.Open();
                command = connection.CreateCommand();
                ISqlBuilder sqlBuilder = new SqlBuilder<T>(instance, command);
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
            var connection = new SqlConnection(connectionString);
            SqlCommand command = null;
            int rowsAffected = 0;

            try {
                connection.Open();
                command = connection.CreateCommand();
                ISqlBuilder sqlBuilder = new SqlBuilder<T>(instance, command);
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
            var table = typeof(T).Name + "s";
            int rowsAffected = 0;
            var connection = new SqlConnection(connectionString);
            SqlCommand command = null;
            try {
                connection.Open();
                command = connection.CreateCommand();
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
    public static class DatabaseExtensions
    {
        public static void Truncate<T>(this IDatabase db) {
            var tableName = typeof(T).Name + "s";
            db.Select<T>($"truncate table {tableName}");
        }
    }
}