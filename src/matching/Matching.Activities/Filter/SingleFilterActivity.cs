using GoodToCode.Analytics.Matching.Domain;
using System.Collections.Generic;
using System.Linq;

namespace GoodToCode.Analytics.Matching.Activities
{
    public class SingleFilterActivity<T>
    {
        public SingleFilterHandler<T> Handler { get; }
        public List<IEnumerable<T>> Results { get; private set; } = new List<IEnumerable<T>>();

        public SingleFilterActivity(FilterExpression<T> filter)
        {
            Handler = filter.Expression;
        }

        public List<IEnumerable<T>> Execute(IEnumerable<T> listToFilter)
        {
            Handler.ApplyFilter(listToFilter);
            
            Results.Add(filter.FilteredList);
            
            return Results;
        }        
    }
}
