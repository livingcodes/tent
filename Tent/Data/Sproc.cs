using System.Collections.Generic;

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
            IQuery query = new Query(connectionString);
        }
        string connectionString, name;

        ISproc ISproc.Name(string name) {
            this.name = name;
            return this;
        }

        public ISproc Parameter(string name, object value) {
            return this;
        }

        public List<T> Select<T>() {
            return new List<T>() { default(T) };
        }

        public T SelectOne<T>() {
            return default(T);
        }
    }
}
