using EducationTech.Business.Services.Business.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;

namespace EducationTech.Business.Services.Business
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _redisCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;

        public RedisCacheService(IDistributedCache redisCache, IConnectionMultiplexer connectionMultiplexer)
        {
            _redisCache = redisCache;
            _connectionMultiplexer = connectionMultiplexer;
            _database = _connectionMultiplexer.GetDatabase();
        }
        public async Task<string?> GetAsync(string key)
        {
            var valueCached = await _redisCache.GetStringAsync(key);
            return string.IsNullOrEmpty(valueCached) ? null : valueCached;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var valueCached = await _redisCache.GetStringAsync(key);
            return string.IsNullOrEmpty(valueCached) ? default : JsonConvert.DeserializeObject<T>(valueCached);
        }

        public async Task RemoveAsync(string key, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(key)) return;
            await _redisCache.RemoveAsync(key, token);
           
        }

        public async Task RemoveByPatternAsync(string pattern, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(pattern)) return;
            var keys = GetKeysByPatternAsync(pattern);
            var removeTask = keys.Select(key => _redisCache.RemoveAsync(key, token));
            await Task.WhenAll(removeTask);
        }

        private IEnumerable<string> GetKeysByPatternAsync(string pattern)
        {
            if (string.IsNullOrEmpty(pattern)) yield break;
            foreach (var endpoint in _connectionMultiplexer.GetEndPoints())
            {
                var server = _connectionMultiplexer.GetServer(endpoint);
                foreach (var key in server.Keys(pattern: pattern))
                {
                    yield return key.ToString();
                }
            }
        }

        public async Task SetAsync(string key, object? value, TimeSpan timeSpan, CancellationToken token = default)
        {
            if (value == null) return;

            var valueCached = JsonConvert.SerializeObject(value, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            });

            await _redisCache.SetStringAsync(key, valueCached, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = timeSpan
            }, token);
        }

        public async Task<T?> TryGetAndSetAsync<T>(string key, Func<Task<T?>> func, TimeSpan timeSpan, CancellationToken token = default)
        {
            T? value = await GetAsync<T>(key);
            if (value != null)
            {
                return value;
            }

            value = await func();
            await SetAsync(key, value, timeSpan, token);

            return value;
        }
    }
} 
