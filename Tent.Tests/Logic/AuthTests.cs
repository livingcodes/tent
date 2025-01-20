namespace Tent.Tests.Logic;
using System.Text;
using Tent.Auth;
using Tent.Logic;
using Basketcase;
using static Basketcase.Table;
[tc]public class AuthTests : BaseTests
{
  [tm]public void SaltAs() {
    var salt = new Salt();
    str saltAsStr = salt.AsString;
    var saltAsBytArr = Encoding.UTF8.GetBytes(saltAsStr, 0, saltAsStr.Length);
    str saltAsStr2 = Encoding.UTF8.GetString(saltAsBytArr, 0, saltAsBytArr.Length);
    assert(saltAsStr == saltAsStr2);
  }

  [tm]public void SignUpAndLogin() {
    crtUsrTbl();

    str eml = "case@sparkle.stream";
    str pw = "abcd1234";
    var signUpResult = new SignUp(eml, pw).Exe();
    assert(!signUpResult.Failed);
      
    var loginResult = new Login(eml, pw).Exe();
    assert(!loginResult.Failed);
  }

  [tm]public void LoginFail() {
    str eml = "case@sparkle.stream";
    str pw = "oops1ts3rong";
    var result = new Login(eml, pw).Exe();
    assert(result.Failed);
  }

  // todo: have duplicate .sql file that could be deleted now that table is created in code
  void crtUsrTbl() {
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