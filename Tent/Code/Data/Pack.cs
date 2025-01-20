namespace Tent.Data;
using Microsoft.Extensions.Caching.Memory;
public class Pack:Basketcase.Db
{
  public Pack(Basketcase.ICache cache = null)
  : base(
    new SqlConFct(),
    new Basketcase.Reader(),
    // todo: switch default to serialized cache
    cache ?? new InMemCache(new MemoryCache(new MemoryCacheOptions()))
  ) { }

  int hr = 60 * 60;
  public Basketcase.IDb Cache(str key) => Cache(key, 1*hr);
}