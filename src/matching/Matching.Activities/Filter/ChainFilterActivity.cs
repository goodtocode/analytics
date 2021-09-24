using GoodToCode.Analytics.Matching.Domain;
using System;

namespace GoodToCode.Analytics.Matching.Activities
{
    class Program
    {
        static void Main(string[] args)
        {
            ChainFilterActivity atm = new ChainFilterActivity();
            atm.ApplyFilter(4600);
            atm.ApplyFilter(1900);
            atm.ApplyFilter(600);
        }
    }

    public class ChainFilterActivity
    {
        private PredicateFilterHandler firstHandler = new PredicateFilterHandler();
        private PropertyFilterHandler secondHandler = new PropertyFilterHandler();
        private LongFilterHandler thirdHandler = new LongFilterHandler();

        public ChainFilterActivity()
        {
            // Prepare the chain of Handlers
            firstHandler.NextHandler(secondHandler);
            secondHandler.NextHandler(thirdHandler);
        }

        public void ApplyFilter(long requestedAmount)
        {
            firstHandler.ApplyFilter(requestedAmount);
        }
    }
}
