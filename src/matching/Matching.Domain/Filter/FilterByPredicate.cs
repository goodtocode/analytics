using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GoodToCode.Analytics.Matching.Domain
{
    public class FilterByPredicate<T> : IFilterable
    {
        public Expression<Func<T, bool>> Expression { get; }

        public FilterByPredicate() { }

        public FilterByPredicate(Expression<Func<T, bool>> expression) { Expression = expression; }
    }
}