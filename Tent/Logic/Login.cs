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

        public IResult<User> Execute() {
            var user = db.SelectOne<User>("where email = @email", email);
            
            if (user == null)
                return Result<User>.Failure(null, "Email is not registered");

            var inputHash = new Hash(password, user.Salt).AsString;

            if (user.PasswordHash != inputHash)
                return Result<User>.Failure(user, "Password incorrect");
            
            return Result<User>.Success(user);
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
    }

    public interface IResult
    {
        bool Failed { get; }
        string ErrorMessage { get; }
    }

    public interface IResult<T> : IResult
    {
        T Value { get; }
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
        public static Result Failure(string errorMessage) =>
            new Result(errorMessage);
    }

    public class Result<T> : Result, IResult<T>
    {
        public Result(T value, string errorMessage = null) 
        : base(errorMessage) {
            Value = value;
        }
        public T Value { get; }

        public new static Result<T> Success(T value) => new Result<T>(value);
        public static Result<T> Failure(T value, string errorMessage) =>
            new Result<T>(value, errorMessage);
    }
}
