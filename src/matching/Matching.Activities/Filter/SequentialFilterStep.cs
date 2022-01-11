using GoodToCode.Analytics.Abstractions;
using GoodToCode.Analytics.Matching.Domain;
using System.Collections.Generic;

namespace GoodToCode.Analytics.Matching.Activities
{
    public class SequentialFilterStep<T> : ISequentialFilterStep<T>
    {
        public List<ChainableFilterHandler<T>> Handlers { get; } = new List<ChainableFilterHandler<T>>();
        public IEnumerable<T> Results;

        public SequentialFilterStep(IEnumerable<FilterExpression<T>> filters)
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

        public IEnumerable<T> Execute(IEnumerable<T> listToFilter)
        {
            var localResults = new List<T>();

            foreach (var handler in Handlers)
            {
                handler.ApplyFilter(listToFilter);
                localResults.AddRange(handler.FilteredList);
            }

            Results = localResults;
            return Results;
        }
    }
}
