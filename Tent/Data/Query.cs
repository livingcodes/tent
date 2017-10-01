using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Tent.Data
{
    public interface IQuery
    {
        List<T> Select<T>(string sql = null);
        (List<T>, List<U>) Select<T, U>(string sql = null);
        DataTable SelectTable(string sql = null);
        T SelectOne<T>(string sql = null);

        IQuery Sql(string sql);
        IQuery Parameter(string name, object value);
    }

    public class Query : IQuery
    {
        public Query(string connectionString) {
            this.connectionString = connectionString;
        }
        public Query(string connectionString, string sql, params object[] parameters)
        : this(connectionString) {
            Sql(sql);
        }

        string connectionString, sql;
        List<(string name, object value)> parameters = new List<(string name, object value)>();

        public IQuery Sql(string sql) {
            this.sql = sql;
            return this;
        }
        public IQuery Parameter(string name, object value) {
            parameters.Add((name, value));
            return this;
        }
        public List<T> Select<T>(string sql = null) {
            if (sql != null)
                Sql(sql);
            var list = new List<T>();
            var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var connection = new SqlConnection(connectionString);
            IDbCommand command = null;
            try {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = this.sql;
                var reader = command.ExecuteReader();
                var converter = new ReaderToList<T>();
                list = converter.Convert(reader);
            } finally {
                if (command != null)
                    command.Dispose();
                if (connection.State != System.Data.ConnectionState.Closed)
                    connection.Close();
            }
            return list;
        }
        public T SelectOne<T>(string sql = null) {
            if (sql != null)
                Sql(sql);
            T item = default(T);
            var table = typeof(T).Name + "s";
            var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var connection = new SqlConnection(connectionString);
            IDbCommand command = null;
            try {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = this.sql;
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
        public DataTable SelectTable(string sql = null) {
            if (sql != null)
                Sql(sql);
            var dataTable = new DataTable();
            var connection = new SqlConnection(connectionString);
            SqlCommand command = null;
            try {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = this.sql;
                foreach (var parameter in parameters)
                    command.Parameters.AddWithValue(parameter.name, parameter.value);
                var reader = command.ExecuteReader();
                dataTable = new ReaderToDataTable().Convert(reader);
            } finally {
                if (command != null)
                    command.Dispose();
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }
            return dataTable;
        }
        public (List<T>, List<U>) Select<T, U>(string sql = null) {
            return (new List<T>() { default(T) }, new List<U>() { default(U) });
        }
    }
}
