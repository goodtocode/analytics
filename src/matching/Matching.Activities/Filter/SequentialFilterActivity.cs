using GoodToCode.Analytics.Matching.Domain;
using System.Collections.Generic;
using System.Linq;

namespace GoodToCode.Analytics.Matching.Activities
{
    public class SequentialFilterActivity<T>
    {
        public List<ChainableFilterHandler<T>> Filters { get; } = new List<ChainableFilterHandler<T>>();
        public List<IEnumerable<T>> Results;

        public SequentialFilterActivity(IEnumerable<FilterExpression<T>> filters)
        {
            ChainableFilterHandler<T> last = null;
            foreach (var filter in filters)
            {                
                var next = new ChainableFilterHandler<T>(filter);
                Filters.Add(next);
                if (last != null)
                    last.SetNextHandler(next);
                last = next;
            }
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
