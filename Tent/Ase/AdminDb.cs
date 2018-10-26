using System.Collections.Generic;
using System.Linq;

namespace Ase
{
    public class AdminDb : IAdminDb
    {
        public AdminDb(IDb db) {
            this.db = db;
        }
        IDb db;

        public void DropTable(string tableName) =>
            db.Select<int>($"drop table {tableName}");

        public void Truncate(string tableName) =>
            db.Execute($"truncate table {tableName}");

        public void CreateProcedure(string name, string sql) =>
            CreateProcedure(name, new List<string>(), sql);

        public void CreateProcedure(string name, IEnumerable<string> parameters, string sql) {
            db.Execute($"DROP PROCEDURE IF EXISTS {name}");

            string _sql = $"CREATE PROCEDURE {name} ";
            if (parameters.Count() > 0) {
                _sql += "(";
                foreach (var parameter in parameters)
                    _sql += parameter + ", ";
                _sql = _sql.Remove(_sql.Length - 2);
                _sql += ") ";
            }
            _sql += $"AS BEGIN {sql} END";

            db.Execute(_sql);
        }
    }
}
