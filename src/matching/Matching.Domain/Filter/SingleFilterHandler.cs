using GoodToCode.Analytics.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoodToCode.Analytics.Matching.Domain
{
    public class SingleFilterHandler<T> : IFilterHandler<T>
    {
        public SingleFilterHandler<T> NextHandler;
        public IFilterExpression<T> Filter { get; private set; }

        public IEnumerable<T> FilteredList { get; private set; }

        public SingleFilterHandler(IFilterExpression<T> filter) { Filter = filter; }

        public IEnumerable<T> ApplyFilter(IEnumerable<T> filterableList)
        {
            if (filterableList.Any())
                throw new ArgumentException("filterableList must not be empty. SingleFilterHandler:ApplyFilter()", filterableList.GetType().Name);

            FilteredList = filterableList.Where(Filter.Expression.Compile());
            
            return FilteredList;
        }
    }
}