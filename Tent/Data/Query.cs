using System.Collections.Generic;
using System.Data;

namespace Tent.Data
{
    public interface IQuery
    {
        List<T> Select<T>(string sql = null);
        //(List<T>, List<U>) Select<T, U>(string sql = null);
        DataTable SelectTable(string sql = null);
        T SelectOne<T>(string sql = null);
        int Execute(string sql = null);

        IQuery Sql(string sql);
        IQuery Parameter(string name, object value);

        string Sql();
    }

    public class Query : IQuery
    {
        public Query(
            IConnectionFactory connectionFactory, 
            IRead reader
        ) {
            this.connectionFactory = connectionFactory;
            this.reader = reader;
            this.parameters = new List<(string name, object value)>();
        }

        IConnectionFactory connectionFactory;
        IRead reader;
        string sql;
        List<(string name, object value)> parameters;

        public IQuery Sql(string sql) {
            this.sql = sql;
            return this;
        }
        public string Sql() {
            return sql;
        }
        public IQuery Parameter(string name, object value) {
            parameters.Add((name, value));
            return this;
        }

        public List<T> Select<T>(string sql = null) {
            if (sql != null)
                Sql(sql);
            var list = new List<T>();
            var connection = connectionFactory.Create();
            IDbCommand command = null;
            try {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = this.sql;
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
        public T SelectOne<T>(string sql = null) {
            if (sql != null)
                Sql(sql);
            T item = default(T);
            var connection = connectionFactory.Create();
            IDbCommand command = null;
            try {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = this.sql;
                foreach (var parameter in parameters) {
                    var p = command.CreateParameter();
                    p.ParameterName = parameter.name;
                    p.Value = parameter.value;
                    command.Parameters.Add(p);
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
        public DataTable SelectTable(string sql = null) {
            if (sql != null)
                Sql(sql);
            var dataTable = new DataTable();
            var connection = connectionFactory.Create();
            IDbCommand command = null;
            try {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = this.sql;
                foreach (var parameter in parameters) {
                    var p = command.CreateParameter();
                    p.ParameterName = parameter.name;
                    p.Value = parameter.value;
                    command.Parameters.Add(p);
                }
                var reader = command.ExecuteReader();
                dataTable.Load(reader);
            } finally {
                if (command != null)
                    command.Dispose();
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }
            return dataTable;
        }

        public int Execute(string sql = null) {
            int rowsAffected = -1;
            if (sql != null)
                Sql(sql);
            var connection = connectionFactory.Create();
            IDbCommand command = null;
            try {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = this.sql;
                foreach (var parameter in parameters) {
                    var p = command.CreateParameter();
                    p.ParameterName = parameter.name;
                    p.Value = parameter.value;
                    command.Parameters.Add(p);
                }
                rowsAffected = command.ExecuteNonQuery();
            } finally {
                if (command != null)
                    command.Dispose();
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }
            return rowsAffected;
        }
        //public (List<T>, List<U>) Select<T, U>(string sql = null) {
        //    return (new List<T>() { default(T) }, new List<U>() { default(U) });
        //}
    }
}