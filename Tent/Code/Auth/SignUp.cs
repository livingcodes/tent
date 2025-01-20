namespace Tent.Auth;
using Tent.Common;
using Tent.Data;
public class SignUp(str eml, str pw)
{
  Pack db = new();

  public IResult Exe() {
    var usr = db.Sel1<User>("WHERE Email = @Email", eml);
    if (usr != null)
      return Result.Fail("Email is not available");
         
    var salt = new Salt();
    var pwHash = new Hash(pw, salt.AsByteArray).AsString;
    var id = db.Ins(new User() {
      Email = eml,
      PasswordHash = pwHash,
      Salt = salt.AsString
    });
    return Result.Suc;
  }
}