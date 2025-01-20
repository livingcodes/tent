namespace Tent.Auth.Password;
using Tent.Data;
public record ResetPw(int uid)
{
  public Result<str> Exe() {
    var pw = new PwGen().Gen();
    var db = new Pack();
    var usr = db.SelById<User>(uid);
    if (usr == null)
      return Result<string>.Fail(null, "User ID not found");

    var hash = new Hash(pw, usr.Salt).AsString;
    db.UpdPwHash(hash, usr.Id);

    return Result<str>.Suc(pw);
  }
}