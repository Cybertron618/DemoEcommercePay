using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DemoEcommercePay.Api.src.Infrastructure.Redis
{
    public class CacheHelper(IDistributedCache cache) : ICacheHelper
    {
        private readonly IDistributedCache _cache = cache ?? throw new ArgumentNullException(nameof(cache));

        public async Task<T?> GetAsync<T>(string key, Func<Task<T>> dataRetriever, TimeSpan? absoluteExpiration = null)
        {
            var cachedValue = await _cache.GetStringAsync(key);
            if (cachedValue != null)
            {
                return JsonConvert.DeserializeObject<T>(cachedValue);
            }

            var data = await dataRetriever();
            if (data != null)
            {
                await SetAsync(key, data, absoluteExpiration);
                return data; // Return the retrieved data if not null
            }

            // If dataRetriever returned null, handle it appropriately (throw an exception or return a default value)
            throw new InvalidOperationException("Data retriever returned null.");
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null)
        {
            var options = new DistributedCacheEntryOptions();
            if (absoluteExpiration != null)
            {
                options.AbsoluteExpirationRelativeToNow = absoluteExpiration.Value;
            }

            var serializedValue = JsonConvert.SerializeObject(value);
            await _cache.SetStringAsync(key, serializedValue, options);
        }

        public Task RemoveAsync(string key)
        {
            return _cache.RemoveAsync(key);
        }

        public async Task InvalidateCacheAsync(string cacheKeyPrefix)
        {
            var keys = await GetAllCacheKeysAsync();
            if (keys is not null)
            {
                foreach (var key in keys)
                {
                    if (key is not null && key.StartsWith(cacheKeyPrefix))
                    {
                        await RemoveAsync(key);
                    }
                }
            }
        }

        private async Task<string[]?> GetAllCacheKeysAsync()
        {
            if (_cache is null)
            {
                throw new InvalidOperationException("_cache is null");
            }

            var methodInfo = _cache.GetType().GetMethod("GetAllKeys", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new NotSupportedException("Cache provider does not support GetAllKeys method.");
            return (await Task.Run(() => (object[]?)methodInfo.Invoke(_cache, null))) as string[] ?? [];
        }
    }

    public interface ICacheHelper
    {
        Task<T?> GetAsync<T>(string key, Func<Task<T>> dataRetriever, TimeSpan? absoluteExpiration = null);
        Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null);
        Task RemoveAsync(string key);
        Task InvalidateCacheAsync(string cacheKeyPrefix);
    }
}
