using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static class EnumerableExtension
    {
        public static Dictionary<TKey, List<T>> Group<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            IEnumerable<IGrouping<TKey, T>> enumerable = source.GroupBy(keySelector);
            Dictionary<TKey, List<T>> dictionary = new Dictionary<TKey, List<T>>(enumerable.Count<IGrouping<TKey, T>>());
            foreach (IGrouping<TKey, T> current in enumerable)
            {
                List<T> value = current.ToList<T>();
                dictionary.Add(current.Key, value);
            }
            return dictionary;
        }

        public static List<TResult> CastList<TResult>(this IEnumerable source)
        {
            return source.Cast<TResult>().ToList<TResult>();
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException();
            if (keySelector == null)
                throw new ArgumentNullException();
            Dictionary<TKey, T> dictionary = new Dictionary<TKey, T>();
            foreach (T current in source)
            {
                TKey tKey = keySelector(current);
                if (tKey == null)
                {
                    throw new ArgumentException("source 集合存在 key 为 null 的元素");
                }
                if (!dictionary.ContainsKey(tKey))
                {
                    dictionary.Add(tKey, current);
                }
            }
            return dictionary.Values;
        }

        /// <summary>
        ///     Combines with.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns></returns>
        public static IEnumerable<Tuple<T, T>> CombineWith<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            using (var firstEnumerator = first.GetEnumerator())
            using (var secondEnumerator = second.GetEnumerator())
            {
                var hasFirst = true;
                var hasSecond = true;

                while ((hasFirst && (hasFirst = firstEnumerator.MoveNext())) |
                       (hasSecond && (hasSecond = secondEnumerator.MoveNext())))
                    yield return Tuple.Create(
                        hasFirst ? firstEnumerator.Current : default(T),
                        hasSecond ? secondEnumerator.Current : default(T)
                    );
            }
        }

        /// <summary>
        ///     Concats the specified delimiter.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        public static string Concat(this IEnumerable<long> source, string delimiter)
        {
            IEnumerable<long> enumerable = source as long[] ?? source.ToArray();
            return !enumerable.Any()
                ? string.Empty
                : enumerable.Select(a => a.ToString()).Aggregate((i, j) => $"{i}{delimiter}{j}");
        }

        /// <summary>
        ///     Pages the specified page.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}