using System;
using System.Collections.Generic;
using System.Linq;

namespace GoodToCode.Analytics.Matching.Domain
{
    public class ExpressionFilterHandler<T> : ChainableFilterHandler<T>
    {
        public IEnumerable<T> FilteredList;

        public ExpressionFilterHandler(IFilterExpression<T> filter) : base(filter)
        {
        }

        public override IEnumerable<T> ApplyFilter(IEnumerable<T> filterableList)
        {
            FilteredList = filterableList.Where(Filter.Expression.Compile());
            return FilteredList;
        }
    }
}
