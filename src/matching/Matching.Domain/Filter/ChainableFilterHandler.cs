using System;

namespace GoodToCode.Analytics.Matching.Domain
{
    public abstract class ChainableFilterHandler<T>
    {
        public ChainableFilterHandler<T> nextHandler;
        
        public void NextHandler(ChainableFilterHandler<T> handler)
        {
            nextHandler = handler;
        }

        public abstract void ApplyFilter(T filter);
    }
}
