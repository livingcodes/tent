using System.Collections.Generic;

namespace Ase
{
    public interface IAdminDb
    {
        void Truncate(string table);
        void DropTable(string table);
        void CreateProcedure(string name, string sql);
        void CreateProcedure(string name, IEnumerable<string> parameters, string sql);
    }
}
