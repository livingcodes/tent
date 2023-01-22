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
   public bool IsReset;
   public string Code;
   public DateTime DateCreated, DateExpires;
   public DateTime? DateReset;
}