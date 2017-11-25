using Microsoft.Extensions.Caching.Memory;

namespace Tent.Data
{
    public class Pack : Database
    {
        public Pack(ICache cache = null)
        : base(
            new SqlConnectionFactory(),
            new Reader(),
            // todo: switch default to serialized cache
            cache ?? new InMemoryCache(new MemoryCache(new MemoryCacheOptions()))
        ) { }
    }
}
