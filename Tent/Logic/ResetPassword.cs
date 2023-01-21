namespace Tent.Logic;
using Tent.Data;
public record ResetPassword(string Email)
{
   public Result<string> Execute() {
      var password = "Reset1234";
      var db = new Pack();
      var user = db.SelectOne<User>("WHERE Email = @Email", Email);
      if (user == null)
         return Result<string>.Failure(null, "Email not found");

      var hash = new Auth.Hash(password, user.Salt).AsString;
      db.Execute(@"
         UPDATE [User] SET PasswordHash = @Hash
         WHERE Id = @Id
         ", hash, user.Id);

      return Result<string>.Success(password);
   }
}