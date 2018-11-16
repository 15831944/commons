namespace System.Linq.Expressions
{
    public class ParameterExpressionReplacer : ExpressionVisitor
    {
        private ParameterExpression replaceWith;

        private ParameterExpressionReplacer(ParameterExpression replaceWith)
        {
            this.replaceWith = replaceWith;
        }

        public static Expression Replace(Expression expression, ParameterExpression replaceWith)
        {
            return new ParameterExpressionReplacer(replaceWith).Visit(expression);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return this.replaceWith;
        }
    }
}