using System.Text;

namespace System.Collections.Generic
{
    public static class DictionaryExtension
    {
        public static TValue TryGetValue<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        {
            if (!dic.TryGetValue(key, out var result))
            {
                result = default(TValue);
            }
            return result;
        }

        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, Func<TValue> valueGenerator, object syncRoot = null)
        {
            if (!dic.TryGetValue(key, out var tValue))
            {
                if (syncRoot != null)
                {
                    lock (syncRoot)
                    {
                        if (!dic.TryGetValue(key, out tValue))
                        {
                            tValue = valueGenerator();
                            dic[key] = tValue;
                        }
                        return tValue;
                    }
                }
                tValue = valueGenerator();
                dic[key] = tValue;
            }
            return tValue;
        }

        public static string JoinToString<T, K>(this Dictionary<T, K> dic)
        {
            var sb = new StringBuilder();
            foreach (var item in dic)
            {
                sb.Append(item.Key);
                sb.Append(":");
                sb.Append(item.Value);
                sb.Append(";");
            }
            return sb.ToString(0, sb.Length - 1);
        }
    }
}