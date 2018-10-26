using System.Collections.Generic;

namespace Ase
{
    public interface IDb
    {
        (int id, int numberRowsAffected) Insert<T>(T content);
        int Update<T>(T content);
        int Delete<T>(int id);
        int Execute(string sql, params object[] parameters);
        List<T> Select<T>(string sql = "", params object[] parameters);
        IAdminDb Admin { get; }
    }
}
