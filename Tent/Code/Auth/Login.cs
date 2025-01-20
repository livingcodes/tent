namespace Tent.Auth;
using Tent.Data;
public class Login(str eml, str pw)
{
  Pack db = new();

  public IResult<User> Exe() {
    var usr = db.Sel1<User>("where email = @email", eml);
    if (usr == null)
      return Result<User>.Fail(null, "Email is not registered");

    var inputHash = new Hash(pw, usr.Salt).AsString;

    if (usr.PasswordHash != inputHash)
      return Result<User>.Fail(usr, "Password incorrect");
    
    return Result<User>.Suc(usr);
  }
}

public class User
{
  public int Id { get; set; }
  public string Email { get; set; }
  public string PasswordHash { get; set; }
  public string Salt { get; set; }
}