using Application.Interfaces.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Application.Services.Infrastructure
{
    /// <summary>
    /// Memory-based cache service with performance monitoring and TTL support
    /// </summary>
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CacheService> _logger;
        private readonly ConcurrentDictionary<string, DateTime> _keyTracker;
        private readonly CacheStatistics _statistics;

        // Default TTL values for different data types
        private static readonly TimeSpan DefaultTtl = TimeSpan.FromMinutes(15);
        private static readonly TimeSpan UserDataTtl = TimeSpan.FromMinutes(30);
        private static readonly TimeSpan RegionDataTtl = TimeSpan.FromHours(4); // Regions change infrequently
        private static readonly TimeSpan ProjectDataTtl = TimeSpan.FromMinutes(20);

        public CacheService(IMemoryCache cache, ILogger<CacheService> logger)
        {
            _cache = cache;
            _logger = logger;
            _keyTracker = new ConcurrentDictionary<string, DateTime>();
            _statistics = new CacheStatistics
            {
                LastResetTime = DateTime.UtcNow
            };
        }

        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            try
            {
                if (_cache.TryGetValue(key, out var cachedValue))
                {
                    Interlocked.Increment(ref _statistics.HitCount);
                    
                    if (cachedValue is string jsonValue)
                    {
                        return JsonSerializer.Deserialize<T>(jsonValue);
                    }
                    
                    return cachedValue as T;
                }

                Interlocked.Increment(ref _statistics.MissCount);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cached value for key {Key}", key);
                Interlocked.Increment(ref _statistics.MissCount);
                return null;
            }
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? timeToLive = null) where T : class
        {
            try
            {
                var ttl = timeToLive ?? GetDefaultTtl<T>();
                var options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = ttl,
                    Priority = CacheItemPriority.Normal
                };

                // Add eviction callback to track key removal
                options.PostEvictionCallbacks.Add(new PostEvictionCallbackRegistration
                {
                    EvictionCallback = OnCacheItemEvicted
                });

                // Serialize complex objects to JSON for consistent caching
                var cacheValue = ShouldSerialize<T>() ? JsonSerializer.Serialize(value) : value;
                
                _cache.Set(key, cacheValue, options);
                _keyTracker.TryAdd(key, DateTime.UtcNow);

                _logger.LogDebug("Cached value for key {Key} with TTL {TTL}", key, ttl);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting cached value for key {Key}", key);
                return false;
            }
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? timeToLive = null) where T : class
        {
            // Try to get from cache first
            var cachedValue = await GetAsync<T>(key);
            if (cachedValue != null)
            {
                return cachedValue;
            }

            // Cache miss - create the value
            try
            {
                var value = await factory();
                if (value != null)
                {
                    await SetAsync(key, value, timeToLive);
                }
                return value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating value for cache key {Key}", key);
                throw;
            }
        }

        public async Task<bool> RemoveAsync(string key)
        {
            try
            {
                _cache.Remove(key);
                _keyTracker.TryRemove(key, out _);
                _logger.LogDebug("Removed cached value for key {Key}", key);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cached value for key {Key}", key);
                return false;
            }
        }

        public async Task<int> RemoveByPatternAsync(string pattern)
        {
            try
            {
                var regex = new Regex(pattern.Replace("*", ".*"), RegexOptions.IgnoreCase);
                var keysToRemove = _keyTracker.Keys.Where(key => regex.IsMatch(key)).ToList();
                
                foreach (var key in keysToRemove)
                {
                    await RemoveAsync(key);
                }

                _logger.LogDebug("Removed {Count} cached values matching pattern {Pattern}", keysToRemove.Count, pattern);
                return keysToRemove.Count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cached values by pattern {Pattern}", pattern);
                return 0;
            }
        }

        public async Task<int> ClearAllAsync()
        {
            try
            {
                var keyCount = _keyTracker.Count;
                
                // Clear all tracked keys
                foreach (var key in _keyTracker.Keys.ToList())
                {
                    _cache.Remove(key);
                }
                
                _keyTracker.Clear();
                
                // Reset statistics
                _statistics.HitCount = 0;
                _statistics.MissCount = 0;
                _statistics.LastResetTime = DateTime.UtcNow;
                
                _logger.LogInformation("Cleared all cached values ({Count} items)", keyCount);
                return keyCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing all cached values");
                return 0;
            }
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return _cache.TryGetValue(key, out _);
        }

        public async Task<CacheStatistics> GetStatisticsAsync()
        {
            return new CacheStatistics
            {
                TotalKeys = _keyTracker.Count,
                HitCount = _statistics.HitCount,
                MissCount = _statistics.MissCount,
                LastResetTime = _statistics.LastResetTime,
                MemoryUsageBytes = EstimateMemoryUsage()
            };
        }

        /// <summary>
        /// Gets the default TTL for a specific type
        /// </summary>
        private static TimeSpan GetDefaultTtl<T>()
        {
            var typeName = typeof(T).Name;
            
            return typeName switch
            {
                var name when name.Contains("User") => UserDataTtl,
                var name when name.Contains("Region") => RegionDataTtl,
                var name when name.Contains("Project") => ProjectDataTtl,
                _ => DefaultTtl
            };
        }

        /// <summary>
        /// Determines if a type should be serialized to JSON
        /// </summary>
        private static bool ShouldSerialize<T>()
        {
            var type = typeof(T);
            return !type.IsPrimitive && type != typeof(string) && !type.IsEnum;
        }

        /// <summary>
        /// Callback when cache item is evicted
        /// </summary>
        private void OnCacheItemEvicted(object key, object value, EvictionReason reason, object state)
        {
            if (key is string keyString)
            {
                _keyTracker.TryRemove(keyString, out _);
                
                if (reason == EvictionReason.Expired)
                {
                    _logger.LogDebug("Cache key {Key} expired and was evicted", keyString);
                }
            }
        }

        /// <summary>
        /// Estimates memory usage (rough approximation)
        /// </summary>
        private long EstimateMemoryUsage()
        {
            // This is a rough estimate - in production, you might want to use
            // more sophisticated memory profiling tools
            return _keyTracker.Count * 1024; // Assume 1KB per cached item on average
        }
    }

    /// <summary>
    /// Extension methods for cache key generation
    /// </summary>
    public static class CacheKeyExtensions
    {
        public static string UserKey(int userId) => $"user:{userId}";
        public static string UserByEmailKey(string email) => $"user:email:{email}";
        public static string UserByNameKey(string name) => $"user:name:{name}";
        public static string AllUsersKey() => "users:all";
        
        public static string RegionKey(int regionId) => $"region:{regionId}";
        public static string RegionByNameKey(string name) => $"region:name:{name}";
        public static string AllRegionsKey() => "regions:all";
        
        public static string ProjectKey(int projectId) => $"project:{projectId}";
        public static string ProjectWithDetailsKey(int projectId) => $"project:details:{projectId}";
        public static string ProjectByOperatorKey(int operatorId) => $"project:operator:{operatorId}";
        public static string AllProjectsKey() => "projects:all";
        
        public static string ProjectSiteKey(int siteId) => $"project-site:{siteId}";
        public static string ProjectSitesByProjectKey(int projectId) => $"project-sites:project:{projectId}";
        
        public static string DrillHoleKey(string drillHoleId) => $"drill-hole:{drillHoleId}";
        public static string DrillHolesByProjectKey(int projectId) => $"drill-holes:project:{projectId}";
        public static string DrillHolesBySiteKey(int projectId, int siteId) => $"drill-holes:site:{projectId}:{siteId}";
    }
} 