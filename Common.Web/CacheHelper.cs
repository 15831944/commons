using System.Web.Caching;

namespace System.Web
{
    /// <summary>
    /// Defines the <see cref="CacheHelper" />
    /// </summary>
    public static class CacheHelper
    {
        #region 设置缓存

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetCache(string key, object value)
        {
            var objCache = HttpRuntime.Cache;
            objCache.Insert(key, value);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string key, object value, TimeSpan timeOut)
        {
            var objCache = HttpRuntime.Cache;
            objCache.Insert(key, value, null, DateTime.MaxValue, timeOut, CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string key, object value, double minutes)
        {
            SetCache(key, value, null, DateTime.Now.AddMinutes(minutes), TimeSpan.Zero);
        }

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetCache(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            var objCache = HttpRuntime.Cache;
            objCache.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration);
        }

        #endregion 设置缓存

        #region 获取缓存

        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetCache(string key)
        {
            var objCache = HttpRuntime.Cache;
            return objCache[key];
        }

        /// <summary>
        /// 得到缓存中缓存的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public static T GetCache<T>(string key)
        {
            var objCache = HttpRuntime.Cache;
            var obj = objCache.Get(key);
            if (obj != null)
            {
                return (T)obj;
            }
            return default(T);
        }

        #endregion 获取缓存

        #region 移除缓存

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
        public static void RemoveCache(string key)
        {
            var _cache = HttpRuntime.Cache;
            _cache.Remove(key);
        }

        #endregion 移除缓存
    }
}