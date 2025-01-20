namespace Tent.Auth.Password;
using Tent.Data;
public class VerifyCode(int uid, str code):Cmd
{
  public Result<VerificationCode> Exe() {
    var x = db.GetVerificationCode(uid, code);
    if (x == null)
        return Result<VerificationCode>.Fail(null, "Reset password ID not found");
    if (x.DateExpires <= Now)
        return Result<VerificationCode>.Fail(null, "Verification code expired");
    x.IsReset = true;
    x.DateReset = Now;
    db.Upd(x);
    return Result<VerificationCode>.Suc(x);
  }
}

file static class DbExt {
  public static VerificationCode GetVerificationCode(
    this Pack db, int uid, str code
  ) =>
    db.Sel1<VerificationCode>(
      "WHERE UserId = @UserId AND Code = @Code", 
      uid, code);
}