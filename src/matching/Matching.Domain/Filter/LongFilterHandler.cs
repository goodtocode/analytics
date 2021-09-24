using System;

namespace GoodToCode.Analytics.Matching.Domain
{
    public class LongFilterHandler : ChainableFilterHandler<long>
    {
        public override void ApplyFilter(long requestedAmount)
        {
            long numberofNotesToBeDispatched = requestedAmount / 2000;
            if (numberofNotesToBeDispatched > 0)
            {
                if (numberofNotesToBeDispatched > 1)
                {
                    Console.WriteLine(numberofNotesToBeDispatched + " Two Thousand notes are dispatched by TwoThousandHandler");
                }
                else
                {
                    Console.WriteLine(numberofNotesToBeDispatched + " Two Thousand note is dispatched by TwoThousandHandler");
                }
            }

            long pendingAmountToBeProcessed = requestedAmount % 2000;

            if (pendingAmountToBeProcessed > 0)
            {
                nextHandler.ApplyFilter(pendingAmountToBeProcessed);
            }
        }
    }
}
