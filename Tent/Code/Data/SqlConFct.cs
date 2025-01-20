namespace Tent.Data;
using System.Data;
public class SqlConFct : Basketcase.IConFct
{
  public IDbConnection Crt() => new Microsoft.Data.SqlClient
    .SqlConnection("server=(LocalDb)\\MSSQLLocalDB;"
    + "database=Tent;"
    + "trusted_connection=true;");
}