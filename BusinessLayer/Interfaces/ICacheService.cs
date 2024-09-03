namespace BusinessLayer.Interfaces
{
    public interface ICacheService
    {
        public T Get<T>(string cacheKey);
        public bool Set<T>(string cacheKey,T value,DateTimeOffset expirationTime);
        public object Remove(string cacheKey);

    }
}
