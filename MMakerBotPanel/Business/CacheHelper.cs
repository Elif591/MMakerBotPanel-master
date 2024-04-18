namespace MMakerBotPanel.Business
{
    using System;
    using System.Runtime.Caching;

    public static class CacheHelper
    {
        private static readonly MemoryCache cache = MemoryCache.Default;

        public static T Get<T>(string key)
        {
            object cachedObject = cache.Get(key);

            return cachedObject == null ? default : (T)cachedObject;
        }

        public static void Add<T>(string key, T value, DateTimeOffset absoluteExpiration)
        {
            cache.Set(key, value, absoluteExpiration);
        }

        public static void Remove(string key)
        {
            if (cache.Contains(key))
            {
                 cache.Remove(key);
            }
        }
    }
}