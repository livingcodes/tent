namespace Tent.Common;
public interface IResult {
  bln Failed { get; }
  str ErrMsg { get; }
}

public interface IResult<T> : IResult {
  T Val { get; }
}

public class Result : IResult {
  public Result(str errMsg = null) {
    ErrMsg = errMsg;
    Failed = ErrMsg != null;
  }
  public bln Failed { get; }
  public str ErrMsg { get; }

  public static readonly Result Suc = new();
  public static Result Fail(str errMsg) =>
    new Result(errMsg);
}

public class Result<T>(T val, str errMsg = null)
:Result(errMsg), IResult<T>
{
  public T Val { get; } = val;

  public new static Result<T> Suc(T val) => new(val);
  public static Result<T> Fail(T val, str errMsg) => new(val, errMsg);
}