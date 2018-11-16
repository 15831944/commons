namespace System.Configuration
{
    using System.Linq;
    using System.Runtime.Caching;

    /// <summary>
    /// Defines the <see cref="ConfigHelper" />
    /// </summary>
    public static class ConfigHelper
    {
        #region Methods

        /// <summary>
        /// 添加新的Key ，Value键值对
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public static void Add(string key, string value)
        {
            ConfigurationManager.AppSettings.Add(key, value);
            CacheHelper.SetCache("AppSettings-" + key, value);
        }

        /// <summary>
        /// The GetConnectString
        /// </summary>
        /// <param name="key">The <see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string GetConnectString(string key)
        {
            var CacheKey = "ConnectionStrings-" + key;
            var value = CacheHelper.GetCache(CacheKey);
            if (value == null)
            {
                value = ConfigurationManager.ConnectionStrings[key].ConnectionString;
                if (value != null)
                {
                    CacheHelper.SetCache(CacheKey, value);
                }
            }
            return value.ToString();
        }

        /// <summary>
        /// 根据Key取Value值
        /// </summary>
        /// <param name="key"></param>
        public static string GetValue(string key)
        {
            var CacheKey = "AppSettings-" + key;
            var value = CacheHelper.GetCache(CacheKey);
            if (value == null)
            {
                if (ConfigurationManager.AppSettings.HasKeys())
                {
                    if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
                    {
                        value = ConfigurationManager.AppSettings[key].ToString().Trim();
                        if (value != null)
                        {
                            CacheHelper.SetCache(CacheKey, value);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            return value.ToString();
        }

        /// <summary>
        /// 根据Key删除项
        /// </summary>
        /// <param name="key">Key</param>
        public static void Remove(string key)
        {
            ConfigurationManager.AppSettings.Remove(key);
            CacheHelper.Remove("AppSettings-" + key);
        }

        /// <summary>
        /// 根据Key修改Value
        /// </summary>
        /// <param name="key">要修改的Key</param>
        /// <param name="value">要修改为的值</param>
        public static void SetValue(string key, string value)
        {
            ConfigurationManager.AppSettings.Set(key, value);
            CacheHelper.SetCache("AppSettings-" + key, value);
        }

        #endregion Methods
    }
}