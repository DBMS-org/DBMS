using Microsoft.Extensions.Caching.Memory;

namespace Application.Interfaces.Infrastructure
{
    /// <summary>
    /// Caching service with TTL, invalidation, and performance monitoring
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Gets a cached value by key
        /// </summary>
        /// <typeparam name="T">Type of cached value</typeparam>
        /// <param name="key">Cache key</param>
        /// <returns>Cached value or null if not found</returns>
        Task<T?> GetAsync<T>(string key) where T : class;

        /// <summary>
        /// Sets a cached value with optional TTL
        /// </summary>
        /// <typeparam name="T">Type of value to cache</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to cache</param>
        /// <param name="timeToLive">Time to live (defaults to 15 minutes)</param>
        /// <returns>True if set successfully</returns>
        Task<bool> SetAsync<T>(string key, T value, TimeSpan? timeToLive = null) where T : class;

        /// <summary>
        /// Gets a cached value or creates it using the factory function
        /// </summary>
        /// <typeparam name="T">Type of cached value</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="factory">Factory function to create the value if not cached</param>
        /// <param name="timeToLive">Time to live (defaults to 15 minutes)</param>
        /// <returns>Cached or newly created value</returns>
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? timeToLive = null) where T : class;

        /// <summary>
        /// Removes a cached value by key
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>True if removed successfully</returns>
        Task<bool> RemoveAsync(string key);

        /// <summary>
        /// Removes all cached values matching a pattern
        /// </summary>
        /// <param name="pattern">Pattern to match (supports wildcards)</param>
        /// <returns>Number of items removed</returns>
        Task<int> RemoveByPatternAsync(string pattern);

        /// <summary>
        /// Clears all cached values
        /// </summary>
        /// <returns>Number of items cleared</returns>
        Task<int> ClearAllAsync();

        /// <summary>
        /// Checks if a key exists in cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>True if key exists</returns>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// Gets cache statistics
        /// </summary>
        /// <returns>Cache statistics</returns>
        Task<CacheStatistics> GetStatisticsAsync();
    }

    /// <summary>
    /// Cache statistics for monitoring
    /// </summary>
    public class CacheStatistics
    {
        public long TotalKeys { get; set; }
        public long HitCount { get; set; }
        public long MissCount { get; set; }
        public double HitRatio => TotalRequests > 0 ? (double)HitCount / TotalRequests : 0;
        public long TotalRequests => HitCount + MissCount;
        public long MemoryUsageBytes { get; set; }
        public DateTime LastResetTime { get; set; }
    }
} 