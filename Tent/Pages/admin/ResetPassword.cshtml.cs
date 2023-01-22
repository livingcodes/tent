namespace Tent.Pages.Admin;
using Tent.Auth.Password;
using Tent.Logic;
public class ResetPasswordModel : AuthenticatedPage
{
   public ResetPasswordModel(ICrypto crypto) : base(crypto) {}
   public string Message;

   public void OnGet() {
      var userId = QueryString("user-id").ToInt();
      if (userId == 0) {
         Message = "User ID required"; return; }
      var resetPasswordId = QueryString("code");
      if (resetPasswordId.NotSet()) {
         Message = "Confirmation code required"; return; }

      var verify = new VerifyCode(userId, resetPasswordId).Execute();
      if (verify.Failed) {
         Message = verify.ErrorMessage; return; }

      var reset = new ResetPassword(verify.Value.UserId).Execute();
      if (reset.Failed) {
         Message = reset.ErrorMessage; return; }

      Message = $"New password: {reset.Value}";
   }
}