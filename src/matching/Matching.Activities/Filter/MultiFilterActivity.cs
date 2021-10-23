using GoodToCode.Analytics.Matching.Domain;
using System.Collections.Generic;

namespace GoodToCode.Analytics.Matching.Activities
{
    public class MultiFilterActivity<T>
    {
        public MultiFilterHandler<T> Handler { get; }
        public List<IEnumerable<T>> Results { get; private set; } = new List<IEnumerable<T>>();

        public MultiFilterActivity(IEnumerable<FilterExpression<T>> filters)
        {
            Handler = new MultiFilterHandler<T>(filters);
        }

        public List<IEnumerable<T>> Execute(IEnumerable<T> listToFilter)
        {            
            var filteredList = Handler.ApplyFilter(listToFilter);            
            Results.Add(filteredList);            
            return Results;
        }        
    }
}
