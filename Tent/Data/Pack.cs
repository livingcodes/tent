using Microsoft.Extensions.Caching.Memory;
using Ase;

namespace Tent.Data
{
    public class Pack : Db
    {
        public Pack(ICache cache = null)
        : base(
            new SqlConnectionFactory(),
            new Reader(),
            // todo: switch default to serialized cache
            cache ?? new InMemoryCache(new MemoryCache(new MemoryCacheOptions()))
        ) { }

        int hr = 60 * 60;

        public IDb Cache(string key) => Cache(key, 1*hr);
    }
}
