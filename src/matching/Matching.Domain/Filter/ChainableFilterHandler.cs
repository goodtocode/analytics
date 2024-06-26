﻿using GoodToCode.Analytics.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoodToCode.Analytics.Matching.Domain
{
    public class ChainableFilterHandler<T> : IFilterHandler<T>
    {
        public IEnumerable<T> FilteredList { get; private set; }
        public ChainableFilterHandler<T> NextHandler { get; private set; }
        public IFilterExpression<T> Filter { get; }

        public ChainableFilterHandler(IFilterExpression<T> filter) { Filter = filter; }

        public void SetNextHandler(ChainableFilterHandler<T> handler)
        {
            NextHandler = handler;
        }        

        public IEnumerable<T> ApplyFilter(IEnumerable<T> filterableList)
        {
            var filteredList = filterableList.Where(Filter.Expression.Compile());
            FilteredList = filteredList?.ToList() ?? new List<T>();
            return FilteredList;
        }
    }
}
