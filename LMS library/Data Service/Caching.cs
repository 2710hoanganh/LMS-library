
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text.Json;

namespace LMS_library.Data_Service
{
    public class Caching : ICaching
    {
        IDatabase _cacheDb;
        public Caching()
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            _cacheDb = redis.GetDatabase();
        }
        public T GetData<T>(string key)
        {
            var value = _cacheDb.StringGet(key);

            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);    
            }
            return default;
        }

        public object RemoveData(string key)
        {
            var exist = _cacheDb.KeyExists(key);
            if (exist)
            {
                _cacheDb.KeyDelete(key);
            }
            return false;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expirayTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var IsSet = _cacheDb.StringSet(key, JsonConvert.SerializeObject(value), expirayTime);
            return IsSet;

        }
    }
}
