using Microsoft.Extensions.Caching.Memory;
using System;

namespace Tent.Data
{
    public interface ICache
    {
        T Get<T>(string key);
        void Set(string key, object value, int seconds);
    }

    public class Cache : ICache
    {
        public Cache(IMemoryCache c) {
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