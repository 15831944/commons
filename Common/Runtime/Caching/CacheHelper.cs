namespace System.Runtime.Caching
{
    /// <summary>
    /// Defines the <see cref="CacheHelper" />
    /// </summary>
    public static class CacheHelper
    {
        #region Methods

        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public static object GetCache(string key)
        {
            var cache = MemoryCache.Default;
            if (cache.Contains(key))
            {
                var value = cache.Get(key);
                return value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// The Remove
        /// </summary>
        /// <param name="key">The <see cref="string"/></param>
        /// <returns>The <see cref="object"/></returns>
        public static object Remove(string key)
        {
            var cache = MemoryCache.Default;
            return cache.Remove(key);
        }

        /// <summary>
        /// The RemoveAll
        /// </summary>
        public static void RemoveAll()
        {
            var cache = MemoryCache.Default;
            foreach (var item in cache)
            {
                cache.Remove(item.Key);
            }
        }

        /// <summary>
        /// The SetCache
        /// </summary>
        /// <param name="key">The <see cref="string"/></param>
        /// <param name="value">The <see cref="object"/></param>
        public static void SetCache(string key, object value)
        {
            var cache = MemoryCache.Default;
            cache.Set(key, value, DateTimeOffset.MaxValue);
        }

        #endregion Methods
    }
}