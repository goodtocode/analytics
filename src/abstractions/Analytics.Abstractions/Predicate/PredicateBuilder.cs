using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GoodToCode.Analytics.Abstractions.Predicate
{
    /// <summary>
    /// Usage: 
    ///     PredicateBuilder.IsTrue<T>().And(... expression1 ...).And(...)
    ///     PredicateBuilder.IsFalse<T>().Or(...)...
    /// </summary>
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> IsTrue<T>()
        {
            return (Expression<Func<T, bool>>)(input => true);
        }

        public static Expression<Func<T, bool>> IsFalse<T>()
        {
            return (Expression<Func<T, bool>>)(input => false);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            InvocationExpression invocationExpression = Expression.Invoke((Expression)expression2, expression1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>((Expression)Expression.OrElse(expression1.Body, (Expression)invocationExpression), (IEnumerable<ParameterExpression>)expression1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            InvocationExpression invocationExpression = Expression.Invoke((Expression)expression2, expression1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>((Expression)Expression.AndAlso(expression1.Body, (Expression)invocationExpression), (IEnumerable<ParameterExpression>)expression1.Parameters);
        }
    }
}
