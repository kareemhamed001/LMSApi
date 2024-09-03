



using System.Runtime.Caching;

namespace BusinessLayer.Services
{
    public class CacheService : ICacheService
    {
        private MemoryCache _memoryCache = MemoryCache.Default;

        public T Get<T>(string cacheKey)
        {
            try
            {
                T item = (T)_memoryCache.Get(cacheKey);
                return item;
            }
            catch (Exception ex)
            {
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
                throw;
            }
        }
    }
}
