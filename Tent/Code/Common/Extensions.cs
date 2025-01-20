namespace Tent.Common;
using System;
public static class Extensions
{
  public static string ToStringOr(this object value, string ifNull) =>
    value == null
      ? ifNull
      : value.ToString();

  public static int ToInt(this string text, int @default = 0) =>
    int.TryParse(text, out var result)
      ? result
      : @default;

  public static DateTime ToDate(this string text, DateTime? @default = null) {
    if (!@default.HasValue)
      @default = DateTime.MinValue;
    var succeeded = DateTime.TryParse(text, out var result);
    if (!succeeded)
      result = @default.Value;
    return result;
  }

  public static bool NotSet(this object obj) =>
    obj == null
    || obj == DBNull.Value
    || obj.ToString().Trim() == "";

  public static bool IsSet(this object obj) =>
    !obj.NotSet();

  // ex. 2._(x => x + 2);
  // ex. 2._(add2); // int add2(int x) => x + 2;
  /// <summary>Pipe x into f(x).</summary>
  public static O _<X, O>(this X x, Func<X, O> f) =>
    f(x);

  // 2._(add, y); // int add(int x, int y) => x + y;
  /// <summary>Pipe x into f(x,y). y is pass as parameter.</summary>
  public static O _<X, Y, O>(this X x, Func<X, Y, O> f, Y y) =>
    f(x, y);

  /// <summary>End pipe. Pipes x into void f(x).</summary>
  public static void _<X>(this X x, Action<X> f) =>
    f(x);
}