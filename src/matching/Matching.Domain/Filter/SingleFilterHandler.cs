using System;
using System.Collections.Generic;

namespace GoodToCode.Analytics.Matching.Domain
{
    public abstract class SingleFilterHandler<T> : IFilterHandler<T>
    {
        public SingleFilterHandler<T> NextHandler;
        public IFilterExpression<T> Filter { get; }

        public SingleFilterHandler(IFilterExpression<T> filter) { Filter = filter; }

        public abstract IEnumerable<T> ApplyFilter(IEnumerable<T> filterableList);
    }
}
