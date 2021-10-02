using GoodToCode.Analytics.Matching.Domain;
using System.Collections.Generic;
using System.Linq;

namespace GoodToCode.Analytics.Matching.Activities
{
    public class SingleFilterActivity<T>
    {
        public ExpressionFilterHandler<T> Filter { get; }
        public List<IEnumerable<T>> Results;

        public SingleFilterActivity(FilterExpression<T> filter)
        {
            Filter = filter;
        }

        public List<IEnumerable<T>> Execute(IEnumerable<T> listToFilter)
        {
            Filters.First().ApplyFilter(listToFilter);
            
            Results = new List<IEnumerable<T>>();
            foreach (var filter in Filters)
                Results.Add(filter.FilteredList);
            
            return Results;
        }        
    }
}
