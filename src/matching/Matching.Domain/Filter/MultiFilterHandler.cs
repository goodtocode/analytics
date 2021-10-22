﻿using System;
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

            foreach (var filter in Filters)
                filterableList.Where(filter.Expression.Compile());

            FilteredList = filterableList.ToList();
            return FilteredList;
        }
    }
}
