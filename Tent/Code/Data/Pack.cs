using Microsoft.Extensions.Caching.Memory;
namespace Tent.Data
{
    public class Pack : Basketcase.Db
    {
        public Pack(Basketcase.ICache cache = null)
        : base(
            new SqlConnectionFactory(),
            new Basketcase.Reader(),
            // todo: switch default to serialized cache
            cache ?? new InMemoryCache(new MemoryCache(new MemoryCacheOptions()))
        ) { }

        int hr = 60 * 60;

        public Basketcase.IDb Cache(string key) => Cache(key, 1*hr);
    }
}
