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
                throw new ArgumentException("filterableList must not be empty.", filterableList.GetType().Name);

            IEnumerable<T> filteredList = null;
            foreach (var filter in Filters)
            {
                if (filteredList == null)
                    filteredList = filterableList;

                filteredList = filteredList.Where(filter.Expression.Compile());
            }
                
            FilteredList = filteredList.ToList();
            return FilteredList;
        }
    }
}
