namespace System.Linq.Expressions
{
    public static class ExpressionExtension
    {
        public static Expression<Func<TSource, bool>> And<TSource>(this Expression<Func<TSource, bool>> a, Expression<Func<TSource, bool>> b)
        {
            Type typeFromHandle = typeof(TSource);
            ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "root");
            Expression left = ParameterExpressionReplacer.Replace(a.Body, parameterExpression);
            Expression right = ParameterExpressionReplacer.Replace(b.Body, parameterExpression);
            BinaryExpression body = Expression.And(left, right);
            return Expression.Lambda<Func<TSource, bool>>(body, new ParameterExpression[]
            {
                parameterExpression
            });
        }

        public static Expression<Func<TSource, bool>> Or<TSource>(this Expression<Func<TSource, bool>> a, Expression<Func<TSource, bool>> b)
        {
            Type typeFromHandle = typeof(TSource);
            ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "root");
            Expression left = ParameterExpressionReplacer.Replace(a.Body, parameterExpression);
            Expression right = ParameterExpressionReplacer.Replace(b.Body, parameterExpression);
            BinaryExpression body = Expression.Or(left, right);
            return Expression.Lambda<Func<TSource, bool>>(body, new ParameterExpression[]
            {
                parameterExpression
            });
        }
    }
}