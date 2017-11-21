using Microsoft.Extensions.Caching.Memory;
using System;

namespace Tent.Data
{
    /// <summary>Stores reference to cached object. If object changes outside cache, it changes in cache too.</summary>
    public class InMemoryCache : ICache
    {
        public InMemoryCache(IMemoryCache c) {
            this.c = c;
        }
        IMemoryCache c;

        public T Get<T>(string key) {
            return c.Get<T>(key);
        }

        public void Set(string key, object value, int seconds) {
            c.Set(key, value, new TimeSpan(0, 0, seconds));
        }
    }
}