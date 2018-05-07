using ConfigurationManagement.Business.Contract;
using ConfigurationManagement.Business.Helper;
using StackExchange.Redis;
using System;

namespace ConfigurationManagement.Business.Manager
{
    public class RedisCacheManager : ICacheManager
    {

        readonly int _duration;
        public RedisCacheManager(int duration)
        {
            _duration = duration;
        }
        string host = "localhost";
        public void Add<T>(string key, T value, int durationMinutes)
        {
            using (var redis = ConnectionMultiplexer.Connect(host))
            {
                var time = TimeSpan.FromMinutes(durationMinutes);
                var db = redis.GetDatabase();
                db.StringSet(key, value.Serialize(), time);
            }

        }

        public void Remove(string key)
        {
            using (var redis = ConnectionMultiplexer.Connect(host))
            {
                var db = redis.GetDatabase();
                db.KeyDelete(key);
            }
        }

        public T Get<T>(string key)
        {
            using (var redis = ConnectionMultiplexer.Connect("localhost"))
            {
                var db = redis.GetDatabase();
                var value = db.StringGet(key);

                if (!value.HasValue)
                    return default(T);

                return Serializer.Deserialize<T>(value);
            }
        }
    }
}
