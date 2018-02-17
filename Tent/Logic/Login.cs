using Tent.Auth;
using Tent.Data;

namespace Tent.Logic
{
    public class Login
    {
        public Login(string email, string password) {
            this.email = email;
            this.password = password;
            db = new Pack();
        }

        string email, password;
        Pack db;

        public IResult Execute() {
            var user = db.Sql("select * from users where email = @email")
                .Parameter("@email", email)
                .SelectOne<User>();
            
            if (user == null)
                return Result.Failure("Email is not registered");

            var inputHash = new Hash(password, user.Salt).Generate();

            if (user.PasswordHash != inputHash)
                return Result.Failure("Password incorrect");
                
            return Result.Success;
        }
    }

    public class User
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
    }

    public interface IResult
    {
        bool Failed { get; }
        string ErrorMessage { get; }
    }

    public class Result : IResult
    {
        public Result(string errorMessage = null) {
            ErrorMessage = errorMessage;
            Failed = ErrorMessage != null;
        }
        public bool Failed { get; }
        public string ErrorMessage { get; }

        public static readonly Result Success = new Result();
        public static Result Failure(string errorMessage) {
            return new Result(errorMessage);
        }
    }
}
