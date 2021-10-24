using GoodToCode.Analytics.Matching.Domain;
using System.Collections.Generic;

namespace GoodToCode.Analytics.Matching.Activities
{
    public class SingleFilterActivity<T> : ISingleFilterActivity<T>
    {
        public SingleFilterHandler<T> Handler { get; }
        public IEnumerable<T> Results { get; private set; }

        public SingleFilterActivity(FilterExpression<T> filter)
        {
            Handler = new SingleFilterHandler<T>(filter);
        }

        public IEnumerable<T> Execute(IEnumerable<T> listToFilter)
        {
            var filteredList = Handler.ApplyFilter(listToFilter);
            Results = filteredList;
            return Results;
        }
    }
}
