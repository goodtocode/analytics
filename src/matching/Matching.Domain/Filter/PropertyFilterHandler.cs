using System;

namespace GoodToCode.Analytics.Matching.Domain
{

    public class PropertyFilterHandler : ChainableFilterHandler<long>
    {
        public override void ApplyFilter(long requestedAmount)
        {
            long numberofNotesToBeDispatched = requestedAmount / 100;
            if (numberofNotesToBeDispatched > 0)
            {
                if (numberofNotesToBeDispatched > 1)
                {
                    Console.WriteLine(numberofNotesToBeDispatched + " Hundred notes are dispatched by HundredHandler");
                }
                else
                {
                    Console.WriteLine(numberofNotesToBeDispatched + " Hundred note is dispatched by HundredHandler");
                }
            }

            long pendingAmountToBeProcessed = requestedAmount % 100;

            if (pendingAmountToBeProcessed > 0)
            {
                nextHandler.ApplyFilter(pendingAmountToBeProcessed);
            }
        }
    }
}
