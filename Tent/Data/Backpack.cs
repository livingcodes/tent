using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Tent.Data
{
    public interface IRead
    {
        T Read<T>(IDataReader reader);
        List<T> ReadList<T>(IDataReader reader);
    }

    public interface IConnectionFactory
    {
        IDbConnection Create();
    }

    public class SqlConnectionFactory : IConnectionFactory
    {
        public IDbConnection Create() {
            return new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Tent; trusted_connection=true;");
        }
    }
    public class Reader : IRead
    {
        public T Read<T>(IDataReader reader) {
            return new ReaderToClass<T>().Convert(reader);
        }
        public List<T> ReadList<T>(IDataReader reader) {
            return new ReaderToList<T>().Convert(reader);
        }
    }

    public class Pack : Backpack
    {
        public Pack() 
        : base(new SqlConnectionFactory(), new Reader()) 
        {
            
        }
    }

    public class Backpack
    {
        public Backpack(
            IConnectionFactory connectionFactory, 
            IRead reader
        ) {
            this.reader = reader;
            this.connectionFactory = connectionFactory;
        }
        string sql;
        IConnectionFactory connectionFactory;
        IRead reader;

        IQuery query { get {
            if (_query == null)
                _query = new Query(connectionFactory, reader);
            return _query;
        } }
        IQuery _query;

        void setQueryToNull() {
            _query = null;
        }

        public Backpack Sql(string sql) {
            this.sql = sql;
            return this;
        }

        public Backpack Parameter(string name, object value) {
            query.Parameter(name, value);
            return this;
        }

        public List<T> Select<T>(string sql = null, params object[] parameters) {
            if (sql != null)
                query.Sql(sql);
            var parameterNames = getParameterNamesFromSql(sql);
            if (parameterNames.Count != parameters.Length)
                throw new System.Exception("Parameter name and value counts are not equal");
            for (var i = 0; i < parameters.Length; i++)
                query.Parameter(parameterNames[i], parameters[i]);
            var list = query.Select<T>();
            setQueryToNull();
            return list;
        }

        List<string> getParameterNamesFromSql(string sql) {
            var parameters = new Dictionary<string, int>();
            var index = sql.IndexOf('@');
            while (index > -1) {
                var endIndex = sql.IndexOfAny(new char[] {' ', ','}, index);
                if (endIndex == -1)
                    endIndex = sql.Length - 1;
                var parameterName = sql.Substring(index, endIndex - index + 1);
                if (!parameters.ContainsKey(parameterName))
                    parameters.Add(parameterName, index);
                index = sql.IndexOf('@', index + 1);
            };
            var keys = new List<string>();
            foreach (var key in parameters.Keys)
                keys.Add(key);
            return keys;
        }
    }
}