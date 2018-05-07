using ConfigurationManagement.Business.Contract;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManagement.Business.Manager
{
    public class FakeCacheManager : ICacheManager
    {
        MemoryCache _InnerCache = null;

        public FakeCacheManager()
        {

        }

        public void Add<T>(string key, T value, int durationMinutes)
        {
            _InnerCache.Set(key, value, DateTime.Now.Add(new TimeSpan(0, durationMinutes, 0)));
        }

        public T Get<T>(string key)
        {
            var cache = _InnerCache.Get(key);

            if (cache != null)
                return (T)cache;

            return default(T);
        }

        public bool Contains(string key)
        {
            return _InnerCache.Get(key) != null;
        }

        public void Remove(string key)
        {
            _InnerCache.Remove(key);        
        }

    }

}
 