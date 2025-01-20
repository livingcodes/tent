namespace Tent.Auth.Password;
using Tent.Logic;
public class SendCode(str eml, ISend send):Cmd
{
  public Result Exe() {
    if (eml.NotSet())
      return Result.Fail("Email is required");

    eml = eml.Trim();
    var usr = db.GetUsrByEml(eml);
    if (usr == null)
      return Result.Fail("Email not found");
    
    var x = new VerificationCode(usr.Id);
    (_, x.Id) = db.Ins(x);

    send.Send($"Reset Password Verification Code: {x.Code}");

    return Result.Suc;
  }
}