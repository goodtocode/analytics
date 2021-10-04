﻿using GoodToCode.Analytics.Matching.Domain;
using System.Collections.Generic;
using System.Linq;

namespace GoodToCode.Analytics.Matching.Activities
{
    public class SequentialFilterActivity<T>
    {
        public List<ChainableFilterHandler<T>> Handlers { get; } = new List<ChainableFilterHandler<T>>();
        public List<IEnumerable<T>> Results;

        public SequentialFilterActivity(IEnumerable<FilterExpression<T>> filters)
        {
            ChainableFilterHandler<T> last = null;
            foreach (var filter in filters)
            {                
                var next = new ChainableFilterHandler<T>(filter);
                Handlers.Add(next);
                if (last != null)
                    last.SetNextHandler(next);
                last = next;
            }
        }

        public List<IEnumerable<T>> Execute(IEnumerable<T> listToFilter)
        {
            Handlers.First().ApplyFilter(listToFilter);
            
            Results = new List<IEnumerable<T>>();
            foreach (var filter in Handlers)
                Results.Add(filter.FilteredList);
            
            return Results;
        }        
    }
}
