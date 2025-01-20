namespace Tent.Auth;
using Tent.Data;
public class Login
{
   public Login(string email, string password) {
      this.email = email;
      this.password = password;
      db = new Pack();
   }

   string email, password;
   Pack db;

   public IResult<User> Execute() {
      var user = db.Sel1<User>("where email = @email", email);
         
      if (user == null)
         return Result<User>.Failure(null, "Email is not registered");

      var inputHash = new Hash(password, user.Salt).AsString;

      if (user.PasswordHash != inputHash)
         return Result<User>.Failure(user, "Password incorrect");
         
      return Result<User>.Success(user);
   }
}

public class User
{
   public int Id { get; set; }
   public string Email { get; set; }
   public string PasswordHash { get; set; }
   public string Salt { get; set; }
}