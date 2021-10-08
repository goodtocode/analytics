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
            Handler = new SingleFilterHandler<T>(filter);
        }

        public List<IEnumerable<T>> Execute(IEnumerable<T> listToFilter)
        {            
            var filteredList = Handler.ApplyFilter(listToFilter);            
            Results.Add(filteredList);            
            return Results;
        }        
    }
}
