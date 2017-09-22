using System;
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
        public int Insert<T>(T obj) => throw new System.NotImplementedException();
        public int Update<T>(T obj) => throw new System.NotImplementedException();
    }
}
