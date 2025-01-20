namespace Tent.Auth;
using Tent.Common;
using Tent.Data;
public class SignUp
{
   public SignUp(string email, string password) {
      this.email = email;
      this.password = password;
      db = new Pack();
   }

   string email, password;
   Pack db;

   public IResult Execute() {
      var user = db.Sel1<User>("WHERE Email = @Email", email);
      if (user != null)
            return Result.Failure("Email is not available");
         
      var salt = new Salt();
      var passwordHash = new Hash(password, salt.AsByteArray).AsString;
      var id = db.Ins(new User() {
            Email = email,
            PasswordHash = passwordHash,
            Salt = salt.AsString
      });
      return Result.Success;
   }
}