namespace Tent.Auth.Password;
using Tent.Data;
public record ResetPassword(int userId)
{
   public Result<string> Execute() {
      var password = new PasswordGenerator().Generate();
      var db = new Pack();
      var user = db.SelectById<User>(userId);
      if (user == null)
         return Result<string>.Failure(null, "User ID not found");

      var hash = new Hash(password, user.Salt).AsString;
      db.UpdatePasswordHash(hash, user.Id);

      return Result<string>.Success(password);
   }
}