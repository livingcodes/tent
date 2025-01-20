namespace Tent.Common;
public class Shorthand
{
  public static T Try<T>(Func<T> @try, Func<T> @catch) {
    try {
      return @try();
    } catch {
      return @catch();
    }
  }

  public static DateTime Now => DateTime.Now;
}