﻿namespace Tent.Auth;
using Tent.Data;
public static class DbExt
{
   public static User GetUserByEmail(this Pack db, string email) =>
      db.SelectOne<User>("WHERE Email = @Email", email);
   
   public static int UpdatePasswordHash(this Pack db, string passwordHash, int userId) =>
      db.Execute(@"
         UPDATE [User] SET PasswordHash = @Hash
         WHERE Id = @Id
         ", passwordHash, userId);
}