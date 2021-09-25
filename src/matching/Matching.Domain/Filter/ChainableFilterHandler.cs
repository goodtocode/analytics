using System;
using System.Collections.Generic;

namespace GoodToCode.Analytics.Matching.Domain
{
    public abstract class ChainableFilterHandler<T> : IFilterHandler<T>
    {
        public ChainableFilterHandler<T> NextHandler;
        public IFilterExpression<T> Filter { get; }

        public ChainableFilterHandler(IFilterExpression<T> filter) { Filter = filter; }

        public void SetNextHandler(ChainableFilterHandler<T> handler)
        {
            NextHandler = handler;
        }

        public abstract IEnumerable<T> ApplyFilter(IEnumerable<T> filterableList);
    }
}
