using System.Collections.Generic;

namespace Tent.Data
{
    public interface IDatabase
    {
        List<T> Select<T>(string sql, params object[] parameters);
        T SelectOne<T>(string sql, params object[] parameters);
        T Select<T>(int id);
        int Execute(string sql, params object[] parameters);
        int Insert<T>(T instance);
        int Update<T>(T instance);
        int Delete<T>(int id);
    }
}
