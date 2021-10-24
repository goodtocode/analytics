using System;
using System.Linq.Expressions;

namespace GoodToCode.Analytics.Abstractions
{
    public interface IFilterExpression<T>
    {
        Expression<Func<T, bool>> Expression { get; }
    }
}