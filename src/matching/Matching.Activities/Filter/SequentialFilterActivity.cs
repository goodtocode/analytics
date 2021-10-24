using GoodToCode.Analytics.Matching.Domain;
using System.Collections.Generic;

namespace GoodToCode.Analytics.Matching.Activities
{
    public class SequentialFilterActivity<T> : ISequentialFilterActivity<T>
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
            Results = new List<IEnumerable<T>>();

            foreach (var handler in Handlers)
            {
                handler.ApplyFilter(listToFilter);
                Results.Add(handler.FilteredList);
            }

            return Results;
        }
    }
}
