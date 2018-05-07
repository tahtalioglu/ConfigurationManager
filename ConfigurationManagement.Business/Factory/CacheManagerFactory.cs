using ConfigurationManagement.Business.Contract;
using ConfigurationManagement.Business.Manager;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConfigurationManagement.Business
{
    public class CacheManagerFactory
    {
        public virtual ICacheManager GetCacheProvider(int duration, string cache)
        {
            switch (cache)
            {
                case "Redis":
                    return new RedisCacheManager(duration);
                case "Fake":
                    return new FakeCacheManager();
                default:
                    throw new Exception("Invalid Cache Provider !");
            }
        }
    }
}
