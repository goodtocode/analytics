﻿using GoodToCode.Analytics.Matching.Domain;
using System.Collections.Generic;

namespace GoodToCode.Analytics.Matching.Activities
{
    public class MultiFilterActivity<T> : IMultiFilterActivity<T>
    {
        public MultiFilterHandler<T> Handler { get; }
        public IEnumerable<T> Results { get; private set; }

        public MultiFilterActivity(IEnumerable<FilterExpression<T>> filters)
        {
            Handler = new MultiFilterHandler<T>(filters);
        }

        public IEnumerable<T> Execute(IEnumerable<T> listToFilter)
        {
            var filteredList = Handler.ApplyFilter(listToFilter);
            Results = filteredList;
            return Results;
        }
    }
}
