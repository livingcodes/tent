using Tent.Auth;
using Tent.Data;

namespace Tent.Logic
{
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
            var user = db.SelectOne<User>("WHERE Email = @Email", email);
            if (user != null)
                return Result.Failure("Email is not available");
            
            var salt = new Salt();
            var passwordHash = new Hash(password, salt.AsByteArray).AsString;
            var id = db.Insert(new User() {
                Email = email,
                PasswordHash = passwordHash,
                Salt = salt.AsString
            });
            return Result.Success;
        }
    }
}
