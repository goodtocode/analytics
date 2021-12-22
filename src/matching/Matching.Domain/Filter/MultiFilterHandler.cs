using GoodToCode.Analytics.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoodToCode.Analytics.Matching.Domain
{
    public class MultiFilterHandler<T> : IFilterHandler<T>
    {
        public MultiFilterHandler<T> NextHandler;
        public IEnumerable<IFilterExpression<T>> Filters { get; private set; }

        public IEnumerable<T> FilteredList { get; private set; }

        public MultiFilterHandler(IEnumerable<IFilterExpression<T>> filterList) { Filters = filterList; }

        public IEnumerable<T> ApplyFilter(IEnumerable<T> filterableList)
        {
            if (!filterableList.Any())
                throw new ArgumentException("filterableList must not be empty. MultiFilterHandler:ApplyFilter()", filterableList.GetType().Name);

            IEnumerable<T> filteredList = null;
            IEnumerable<T> finalList = null;
            foreach (var filter in Filters)
            {
                filteredList = filteredList == null ? filterableList : finalList;
                finalList = filteredList.Where(filter.Expression.Compile());
            }
                
            FilteredList = filteredList?.ToList() ?? new List<T>();
            return FilteredList;
        }
    }
}
