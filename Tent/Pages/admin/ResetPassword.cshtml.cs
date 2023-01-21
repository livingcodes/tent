namespace Tent.Pages.Admin;
using Tent.Logic;
public class ResetPasswordModel : AuthenticatedPage
{
   public ResetPasswordModel(ICrypto crypto) : base(crypto) {}

   public string NewPassword, ErrorMessage; // output

   public void OnPost() {
      var email = Request.Form["email"].FirstOrDefault();
      if (email.NotSet()) {
         ErrorMessage = "Email is required."; return; }

      // todo: verify email (send link)

      var result = new ResetPassword(email).Execute();
      if (result.Failed) {
         ErrorMessage = result.ErrorMessage; return; }

      NewPassword = result.Value;
   }
}