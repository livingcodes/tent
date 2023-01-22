using System.Data;
using System.Data.SqlClient;

namespace Tent.Data
{
    public class SqlConnectionFactory : Basketcase.IConnectionFactory
    {
        public IDbConnection Create() {
            return new SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Tent; trusted_connection=true;");
        }
    }
}