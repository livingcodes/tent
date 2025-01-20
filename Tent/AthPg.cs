namespace Tent;
using Tent.Auth;
using Tent.Logic;
using static Shorthand;
public class AthPg : BasePg
{
  public AthPg(ICrypto crypto) =>
    this.crypto = crypto;

  public UserCookie GetUsrCkie() => Try(
    () => usrCkie._(decrypt)._(dslz),
    () => new UserCookie()
  );
     
  public void SetUsrCkie(UserCookie ckie) =>
    setCkie( "user", ckie._(toJson)._(encrypt), Now.AddYears(1) );

  public void SetUsrCkie(User usr) =>
    usr._(bldCkie)._(SetUsrCkie);

  #region private
  ICrypto crypto;
  str usrCkie => Request.Cookies["user"];
  string toJson(obj x) =>
    Newtonsoft.Json.JsonConvert.SerializeObject(x);
  string encrypt(str jsn) =>
    crypto.Encrypt(jsn);
  UserCookie dslz(str jsn) =>
    Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(jsn);
  str decrypt(str ckie) =>
    crypto.Decrypt(ckie); // exception if null (i.e. cookie missing)
  void setCkie(str key, str val, dte exp) =>
    Response.Cookies.Append(key, val,
      options: new CookieOptions {
        Expires = exp
      }
    );
  UserCookie bldCkie(User usr) =>
    usr == null
      ? new UserCookie()
      : new UserCookie() {
        Authenticated = true,
        Id = usr.Id,
        Email = usr.Email
      };
  #endregion
}

public class UserCookie {
  public int Id { get; set; }
  public str Email { get; set; }
  public bln Authenticated { get; set; }
}