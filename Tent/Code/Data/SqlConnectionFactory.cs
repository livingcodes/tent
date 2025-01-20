namespace Tent.Data;
using System.Data;
public class SqlConnectionFactory : Basketcase.IConFct
{
    public IDbConnection Crt() {
        return new Microsoft.Data.SqlClient.SqlConnection("server=(LocalDb)\\MSSQLLocalDB; database=Tent; trusted_connection=true;");
    }
}