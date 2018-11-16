using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// 类说明：CacheHttpRuntimeHelper
/// 编 码 人：苏飞
/// 联系方式：361983679  
/// 更新网站：http://www.sufeinet.com/thread-655-1-1.html
/// </summary>
using System.Text;
using System.Web;
using System.Web.Caching;

namespace SufeiUtil
{
    /// <summary>
    /// 当前应用程序System.Web.Caching.Cache帮助类
    /// </summary>
    public class CacheHttpRuntimeHelper
    {
        /// <summary>
        /// 添加到缓存中
        /// </summary>
        /// <param name="key">用于引用对象的缓存键。</param>
        /// <param name="value">要插入到缓存中的对象</param>
        /// <param name="minutes">所插入对象将到期并被从缓存中移除的时间</param>
        public static void Insert(string key, object value, double minutes)
        {
            Insert(key, value, null, DateTime.Now.AddMinutes(minutes), TimeSpan.Zero);
        }

        public static void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            HttpRuntime.Cache.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration);
        }
        /// <summary>
        /// 得到缓存中缓存的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public static T GetCache<T>(string key)
        {
            object obj = HttpRuntime.Cache.Get(key);
            if (obj != null)
            {
                return (T)obj;
            }
            return default(T);
        }
    }
}
