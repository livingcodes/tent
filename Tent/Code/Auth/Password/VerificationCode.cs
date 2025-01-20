namespace Tent.Auth.Password;
public class VerificationCode {
  public VerificationCode() {}
  public VerificationCode(int userId) {
    UserId = userId;
    Code = Guid.NewGuid().ToString();
    DateCreated = Now;
    DateExpires = DateCreated.AddDays(1);
  }
  public int Id, UserId;
  public bln IsReset;
  public str Code;
  public dte DateCreated, DateExpires;
  public dte? DateReset;
}