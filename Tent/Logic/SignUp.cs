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
            var salt = new Salt();
            var passwordHash = new Hash(password, salt.AsByteArray).Generate();
            var id = db.Insert(new User() {
                Email = email,
                PasswordHash = passwordHash,
                Salt = salt.AsString
            });
            return Result.Success;
        }
    }
}
