using System.Collections.Generic;

namespace GoodToCode.Analytics.Matching.Domain
{
    public interface IChainableFilterHandler<T> : IFilterHandler<T>
    {
        void SetNextHandler(IChainableFilterHandler<T> handler);
    }
}