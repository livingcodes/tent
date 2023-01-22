namespace Tent.Pages.Admin;
using Tent.Auth.Password;
using Tent.Logic;
public class ForgotPasswordModel : BasePage
{
   public string Message;

   public void OnPost() {
      var email = Form("email");
      var send = new DebugSend();
      var result = new SendCode(email, send).Execute();
      Message = (result.Failed)
         ? result.ErrorMessage
         : "Check your email";
   }
}