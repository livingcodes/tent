﻿namespace Tent.Auth.Password;
using Tent.Logic;
public class SendCode : Command
{
   public SendCode(string email, ISend send) {
      this.email = email;
      this.send = send;
   }
   string email; ISend send;
   
   public Result Execute() {
      if (email.NotSet())
         return Result.Failure("Email is required");

      email = email.Trim();
      var user = db.GetUserByEmail(email);
      if (user == null)
         return Result.Failure("Email not found");
      
      var x = new VerificationCode(user.Id);
      (_, x.Id) = db.Ins(x);

      send.Send($"Reset Password Verification Code: {x.Code}");

      return Result.Success;
   }
}