using Microsoft.Extensions.Caching.Memory;

namespace Tent.Data
{
    public class Pack : Backpack
    {
        public Pack(ICache cache = null)
        : base(
            new SqlConnectionFactory(),
            new Reader(),
            cache ?? new InMemoryCache(new MemoryCache(new MemoryCacheOptions()))
        ) { }
    }
}
