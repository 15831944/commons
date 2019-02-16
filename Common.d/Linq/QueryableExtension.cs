using System.Linq.Expressions;

namespace System.Linq
{
    public static class QueryableExtension
    {
        public static IQueryable<TSource> WhereIfNotNullOrEmpty<TSource>(this IQueryable<TSource> source, string value, Expression<Func<TSource, bool>> predicate)
        {
            return source.WhereIf(!string.IsNullOrEmpty(value), predicate);
        }

        public static IQueryable<TSource> WhereIfNotNull<TSource, TValue>(this IQueryable<TSource> source, TValue? value, Expression<Func<TSource, bool>> predicate) where TValue : struct
        {
            return source.WhereIf(value.HasValue, predicate);
        }

        public static IQueryable<TSource> WhereIfNotNull<TSource>(this IQueryable<TSource> source, object value, Expression<Func<TSource, bool>> predicate)
        {
            return source.WhereIf(value != null, predicate);
        }

        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
            {
                return source.Where(predicate);
            }
            return source;
        }

        public static IQueryable<TSource> TakePage<TSource>(this IQueryable<TSource> source, int pageNumber, int pageSize)
        {
            int count = (pageNumber - 1) * pageSize;
            return source.Skip(count).Take(pageSize);
        }

        /// <summary>
        ///     Pages the specified page.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}