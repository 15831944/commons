namespace System.Web.Caching
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
        public static object GetCache(string CacheKey)
        {
            var objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            var _cache = HttpRuntime.Cache;
            var CacheEnum = _cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                _cache.Remove(CacheEnum.Key.ToString());
            }
        }

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        public static void RemoveCache(string CacheKey)
        {
            var _cache = HttpRuntime.Cache;
            _cache.Remove(CacheKey);
        }

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        public static void SetCache(string CacheKey, object objObject)
        {
            var objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject);
        }

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            var objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string CacheKey, object objObject, TimeSpan Timeout)
        {
            var objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, DateTime.MaxValue, Timeout, System.Web.Caching.CacheItemPriority.NotRemovable, null);
        }

        #endregion Methods
    }
}