namespace Tent.Tests.Logic;
using System.Text;
using Tent.Auth;
using Tent.Logic;
using Basketcase;
using static Basketcase.Table;
[TestClass]
public class AuthTests : BaseTests
{
  [TestMethod]
  public void SaltAs() {
    var salt = new Salt();
    var saltAsString = salt.AsString;
    var saltAsByteArray = Encoding.UTF8.GetBytes(saltAsString, 0, saltAsString.Length);
    var saltAsString2 = Encoding.UTF8.GetString(saltAsByteArray, 0, saltAsByteArray.Length);
    assert(saltAsString == saltAsString2);
  }

  [TestMethod]
  public void SignUpAndLogin() {
    createUserTable();

    string email = "case@sparkle.stream";
    string password = "abcd1234";
    var signUpResult = new SignUp(email, password).Execute();
    assert(!signUpResult.Failed);
      
    var loginResult = new Login(email, password).Execute();
    assert(!loginResult.Failed);
  }

  [TestMethod]
  public void LoginFail() {
    string email = "case@sparkle.stream";
    string password = "oops1ts3rong";
    var result = new Login(email, password).Execute();
    assert(result.Failed);
  }

  // todo: have duplicate .sql file that could be deleted now that table is created in code
  void createUserTable() {
    var sql = new Table("User")
      .AddCol("Id", SqlType.Int, Syntax.PrimaryKey + Syntax.Identity(1, 1))
      .AddCol("Email", SqlType.VarChar(100), Syntax.NotNull)
      .AddCol("PasswordHash", SqlType.VarChar(255), Syntax.NotNull)
      .AddCol("Salt", SqlType.VarChar(255), Syntax.NotNull)
      .AddCol("DateCreated", SqlType.DateTime, Syntax.NotNull + Syntax.DefaultGetDate)
      .End()
      .Sql;
    db.Exe(sql);
  }
}