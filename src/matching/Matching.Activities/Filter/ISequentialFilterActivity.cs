using GoodToCode.Analytics.Matching.Domain;
using System.Collections.Generic;

namespace GoodToCode.Analytics.Matching.Activities
{
    public interface ISequentialFilterActivity<T>
    {
        List<ChainableFilterHandler<T>> Handlers { get; }

        IEnumerable<T> Execute(IEnumerable<T> listToFilter);
    }
}