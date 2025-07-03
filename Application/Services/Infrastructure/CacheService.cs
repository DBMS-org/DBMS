using Application.Interfaces.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Application.Services.Infrastructure
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CacheService> _logger;
        private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(5);

        public CacheService(IMemoryCache cache, ILogger<CacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            if (_cache.TryGetValue(key, out T value))
            {
                _logger.LogInformation("Cache hit for key: {CacheKey}", key);
                return value;
            }

            _logger.LogInformation("Cache miss for key: {CacheKey}. Fetching from source.", key);
            
            var result = await factory();
            
            if (result != null)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration ?? DefaultExpiration
                };
                
                _cache.Set(key, result, cacheEntryOptions);
                _logger.LogInformation("Cached new value for key: {CacheKey}", key);
            }

            return result;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
            _logger.LogInformation("Removed key from cache: {CacheKey}", key);
        }
    }
} 