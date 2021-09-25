using System;
using System.Linq.Expressions;

namespace GoodToCode.Analytics.Matching.Domain
{
    public class FilterExpression<T> : IFilterExpression<T>
    {
        public Expression<Func<T, bool>> Expression { get; }

        public FilterExpression() { }

        public FilterExpression(Expression<Func<T, bool>> expression) { Expression = expression; }
    }
}