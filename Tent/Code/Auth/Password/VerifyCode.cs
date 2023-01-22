namespace Tent.Auth.Password;
using Tent.Data;

public class VerifyCode : Command {
   public VerifyCode(int userId, string code) {
      this.userId = userId;
      this.code = code;
   }
   int userId; string code;

   public Result<VerificationCode> Execute() {
      var x = db.GetVerificationCode(userId, code);
      if (x == null)
         return Result<VerificationCode>.Failure(null, "Reset password ID not found");
      if (x.DateExpires <= Now)
         return Result<VerificationCode>.Failure(null, "Verification code expired");
      x.IsReset = true;
      x.DateReset = Now;
      db.Update(x);
      return Result<VerificationCode>.Success(x);
   }
}

file static class DbExt {
   public static VerificationCode GetVerificationCode(
      this Pack db, int userId, string code
   ) =>
      db.SelectOne<VerificationCode>(
         "WHERE UserId = @UserId AND Code = @Code", 
         userId, code);
}