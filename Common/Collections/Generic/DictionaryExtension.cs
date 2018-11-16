namespace System.Collections.Generic
{
    public static class DictionaryExtension
    {
        public static TValue TryGetValue<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        {
            if (!dic.TryGetValue(key, out TValue result))
            {
                result = default(TValue);
            }
            return result;
        }

        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, Func<TValue> valueGenerator, object syncRoot = null)
        {
            if (!dic.TryGetValue(key, out TValue tValue))
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
    }
}