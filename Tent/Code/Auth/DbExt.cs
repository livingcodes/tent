namespace Tent.Auth;
using Tent.Data;
public static class DbExt
{
  public static User GetUsrByEml(this Pack db, str eml) =>
    db.Sel1<User>("WHERE Email = @Email", eml);
   
  public static int UpdPwHash(this Pack db, str pwHash, int uid) =>
    db.Exe(@"
      UPDATE [User] SET PasswordHash = @Hash
      WHERE Id = @Id
      ", pwHash, uid);
}