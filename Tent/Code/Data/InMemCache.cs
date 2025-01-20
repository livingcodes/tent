namespace Tent.Data;
using Microsoft.Extensions.Caching.Memory;
/// <summary>Stores reference to cached object.
/// If object changes outside cache, it changes in cache too.</summary>
public class InMemCache(IMemoryCache c) : Basketcase.ICache
{
  public T Get<T>(str key) => c.Get<T>(key);

  public void Set(str key, obj val, int sec) {
    c.Set(key, val, new TimeSpan(0, 0, sec));
  }
}