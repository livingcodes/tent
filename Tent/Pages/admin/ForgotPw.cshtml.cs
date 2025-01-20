namespace Tent.Pages.Admin;
using Tent.Auth.Password;
using Tent.Logic;
public class ForgotPwModel:BasePg
{
  public str Msg;

  public void OnPost() {
    var eml = Frm("email");
    var send = new DebugSend();
    var result = new SendCode(eml, send).Exe();
    Msg = (result.Failed)
      ? result.ErrMsg
      : "Check your email";
  }
}