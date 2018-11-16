using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Helper
{
    public static class EnumHelper
    {
        /// <summary>
        ///     Gets the key values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, int>> GetKeyValues<T>()
        {
            var enumValues = GetValues<T>();
            return enumValues
                .Select(enumValue => new KeyValuePair<string, int>(enumValue.ToString(), Convert.ToInt32(enumValue))).ToList();
        }

        /// <summary>
        ///     Gets the values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetValues<T>()
        {
            yield return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}