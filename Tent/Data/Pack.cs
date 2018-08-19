using Microsoft.Extensions.Caching.Memory;

namespace Tent.Data
{
    // todo: pull out of library. create these for tests and apps
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
