using System.Linq.Expressions;

namespace Logix.Application.Helpers
{
    public class ExpressionMapperHelper
    {
        public static Expression<Func<TDestination, TResult>> MapExpression<TSource, TDestination, TResult>(
            Expression<Func<TSource, TResult>> sourceExpression)
        {
            var parameter = Expression.Parameter(typeof(TDestination), sourceExpression.Parameters[0].Name);
            var body = new CustomExpressionVisitor<TSource, TDestination>(parameter).Visit(sourceExpression.Body);
            return Expression.Lambda<Func<TDestination, TResult>>(body, parameter);
        }

        private class CustomExpressionVisitor<TSource, TDestination> : System.Linq.Expressions.ExpressionVisitor
        {
            private readonly ParameterExpression _parameter;

            public CustomExpressionVisitor(ParameterExpression parameter)
            {
                _parameter = parameter;
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.Member.DeclaringType == typeof(TSource))
                {
                    var newMember = typeof(TDestination).GetMember(node.Member.Name).FirstOrDefault();
                    if (newMember == null)
                    {
                        // Skip members that do not exist in TDestination
                        return Expression.Constant(null, node.Type);
                    }
                    return Expression.MakeMemberAccess(_parameter, newMember);
                }
                return base.VisitMember(node);
            }

            protected override Expression VisitUnary(UnaryExpression node)
            {
                // Handle cases where the operand type is TSource or nullable TSource
                if (node.NodeType == ExpressionType.Convert || node.NodeType == ExpressionType.ConvertChecked)
                {
                    var memberExpression = node.Operand as MemberExpression;
                    if (memberExpression != null && memberExpression.Member.DeclaringType == typeof(TSource))
                    {
                        var newMember = typeof(TDestination).GetMember(memberExpression.Member.Name).FirstOrDefault();
                        if (newMember != null)
                        {
                            var memberAccess = Expression.MakeMemberAccess(_parameter, newMember);
                            return Expression.MakeUnary(node.NodeType, memberAccess, node.Type);
                        }
                        else
                        {
                            // Skip members that do not exist in TDestination
                            return Expression.Constant(null, memberExpression.Type);
                        }
                    }
                }
                return base.VisitUnary(node);
            }
        }
    }

    //public class DateCondition
    //{
    //    public string DatePropertyName { get; set; }
    //    public string ComparisonOperator { get; set; } // "GreaterThanOrEqual", "LessThanOrEqual", "Between", "Equal"
    //    public string StartDateString { get; set; }
    //    public string EndDateString { get; set; } // Only needed for "Between"
    //}
    public class DateCondition
    {
        public string DatePropertyName { get; set; }
        public ComparisonOperator ComparisonOperator { get; set; } // "GreaterThanOrEqual", "LessThanOrEqual", "Between", "Equal"
        public string StartDateString { get; set; }
        public string EndDateString { get; set; } // Only needed for "Between"
    }
}
