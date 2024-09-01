using LMSApi.App.Interfaces;
using System.Runtime.Caching;

namespace LMSApi.App.Services
{
    public class CacheService(ILogger<CacheService> logger) : ICacheService
    {
        private MemoryCache _memoryCache = MemoryCache.Default;
        private readonly ILogger<CacheService> _logger;

        public T Get<T>(string cacheKey)
        {
            try
            {
                T item = (T)_memoryCache.Get(cacheKey);
                return item;
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                _logger.LogError(ex, "Error getting item from cache :{ex}", ex);
                throw;
            }
        }

        public object Remove(string cacheKey)
        {
            var res = true;
            try
            {
                if (!string.IsNullOrEmpty(cacheKey))
                    _memoryCache.Remove(cacheKey);
                else
                    res = false;

                return res;
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                _logger.LogError(ex, "Error removing item from cache :{ex}", ex);
                throw;
            }
        }

        public bool Set<T>(string cacheKey, T value, DateTimeOffset expirationTime)
        {
            var res = true;
            try
            {
                if (!string.IsNullOrEmpty(cacheKey))
                    _memoryCache.Set(cacheKey, value, expirationTime);
                else
                    res = false;

                return res;

            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                _logger.LogError(ex, "Error setting item to cache :{ex}", ex);
                throw;
            }
        }
    }
}
