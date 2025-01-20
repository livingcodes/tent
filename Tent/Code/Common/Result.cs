namespace Tent.Common;
public interface IResult {
  bool Failed { get; }
  string ErrorMessage { get; }
}

public interface IResult<T> : IResult {
  T Value { get; }
}

public class Result : IResult {
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

public class Result<T> : Result, IResult<T> {
  public Result(T value, string errorMessage = null)
  : base(errorMessage) {
    Value = value;
  }
  public T Value { get; }

  public new static Result<T> Success(T value) => new Result<T>(value);
  public static Result<T> Failure(T value, string errorMessage) =>
    new Result<T>(value, errorMessage);
}