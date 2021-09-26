using GoodToCode.Analytics.Matching.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoodToCode.Analytics.Matching.Activities
{
    public class ListFilterActivity<T>
    {
        public List<ExpressionFilterHandler<T>> Filters { get; }
        public List<IEnumerable<T>> Results;

        public ListFilterActivity(IEnumerable<FilterExpression<T>> filters)
        {
            ExpressionFilterHandler<T> last = null;
            foreach (var filter in filters)
            {                
                var next = new ExpressionFilterHandler<T>(filter);
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
