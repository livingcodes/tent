using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Tent.Data
{
    public interface ISproc
    {
        ISproc Name(string name);
        ISproc Parameter(string name, object value);
        List<T> Select<T>();
        T SelectOne<T>();
    }

    public class Sproc : ISproc
    {
        public Sproc(string connectionString, string name) {
            this.connectionString = connectionString;
            this.name = name;
            parameters = new List<(string name, object value)>();
            query = new Query(connectionString);
        }
        string connectionString, name;
        List<(string name, object value)> parameters;
        IQuery query;

        ISproc ISproc.Name(string name) {
            this.name = name;
            return this;
        }

        public ISproc Parameter(string name, object value) {
            parameters.Add((name, value));
            return this;
        }

        public List<T> Select<T>() {
            var list = new List<T>();
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var connection = new SqlConnection(connectionString);
            SqlCommand command = null;
            try {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = name;
                command.CommandType = CommandType.StoredProcedure;
                foreach (var parameter in parameters)
                    command.Parameters.AddWithValue(parameter.name, parameter.value);
                var reader = command.ExecuteReader();
                var converter = new ReaderToClassList<T>();
                list = converter.Convert(reader);
            } finally {
                if (command != null)
                    command.Dispose();
                if (connection.State != System.Data.ConnectionState.Closed)
                    connection.Close();
            }
            return list;
        }

        public T SelectOne<T>() {
            T item = default(T);
            var connection = new SqlConnection(connectionString);
            SqlCommand command = null;
            try {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = name;
                command.CommandType = CommandType.StoredProcedure;
                foreach (var parameter in parameters)
                    command.Parameters.AddWithValue(parameter.name, parameter.value);
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
    }
}
