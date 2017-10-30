using System.Collections.Generic;
using System.Data;

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
        public Sproc(
            IConnectionFactory connectionFactory,
            IRead reader,
            string name
        ) {
            this.connectionFactory = connectionFactory;
            this.reader = reader;
            this.name = name;
            parameters = new List<(string name, object value)>();
            query = new Query(connectionFactory, reader);
        }
        IConnectionFactory connectionFactory;
        IRead reader;
        string name;
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
            var connection = connectionFactory.Create();
            IDbCommand command = null;
            try {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = name;
                command.CommandType = CommandType.StoredProcedure;
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
                if (connection.State != System.Data.ConnectionState.Closed)
                    connection.Close();
            }
            return list;
        }

        public T SelectOne<T>() {
            T item = default(T);
            var connection = connectionFactory.Create();
            IDbCommand command = null;
            try {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = name;
                command.CommandType = CommandType.StoredProcedure;
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
                if (connection.State != System.Data.ConnectionState.Closed)
                    connection.Close();
            }
            return item;
        }
    }
}
