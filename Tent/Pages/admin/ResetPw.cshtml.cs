namespace Tent.Pages.Admin;
using Tent.Auth.Password;
using Tent.Logic;
public class ResetPwModel(ICrypto crypto):AthPg(crypto)
{
  public str Msg;

  public void OnGet() {
    var uid = QryStr("user-id").Int();
    if (uid == 0) {
      Msg = "User ID required"; return; }
    var resetPwId = QryStr("code");
    if (resetPwId.NotSet()) {
      Msg = "Confirmation code required"; return; }

    var verify = new VerifyCode(uid, resetPwId).Exe();
    if (verify.Failed) {
      Msg = verify.ErrMsg; return; }

    var reset = new ResetPw(verify.Val.UserId).Exe();
    if (reset.Failed) {
      Msg = reset.ErrMsg; return; }

    Msg = $"New password: {reset.Val}";
  }
}