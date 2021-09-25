using System;
using System.Linq.Expressions;

namespace GoodToCode.Analytics.Matching.Domain
{
    public interface IFilterExpression<T>
    {
        Expression<Func<T, bool>> Expression { get; }
    }
}