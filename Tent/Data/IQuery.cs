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
}