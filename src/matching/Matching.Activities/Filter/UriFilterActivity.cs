using GoodToCode.Analytics.Matching.Domain;
using System;
using System.Collections.Generic;

namespace GoodToCode.Analytics.Matching.Activities
{
    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        ListFilterActivity filterChain = new ListFilterActivity();
    //        filterChain.ApplyFilter(4600);
    //        filterChain.ApplyFilter(1900);
    //        filterChain.ApplyFilter(600);
    //    }
    //}

    public class UriFilterActivity
    {
        // By colum type:
        // Uri column: ContainsPath(), BeginsWithPath, EndsWithPath, Equals
        // String column: Contains, BeginsWith, EndsWith, Equals

        private FilterExpression<object> filter = new FilterExpression<object>(x => x.ToString().Length > 0);
        private ExpressionFilterHandler<object> firstHandler;
        private ExpressionFilterHandler<object> secondHandler;
        private ExpressionFilterHandler<object> thirdHandler;

        public UriFilterActivity()
        {
            firstHandler = new ExpressionFilterHandler<object>(filter);
            secondHandler = new ExpressionFilterHandler<object>(filter);
            thirdHandler = new ExpressionFilterHandler<object>(filter);
            
            // ToDo: Move to Parallel https://timdeschryver.dev/blog/process-your-list-in-parallel-to-make-it-faster-in-dotnet#parallel-linq-plinq
            firstHandler.SetNextHandler(secondHandler);
            secondHandler.SetNextHandler(thirdHandler);
        }

        public void Execute(IEnumerable<object> listToFilter)
        {
            // Start chain filters
            firstHandler.ApplyFilter(listToFilter);

            // Read results from each
            var firstResults = firstHandler.FilteredList;
            var secondResults = secondHandler.FilteredList;
            var thirdResults = thirdHandler.FilteredList;

        }
    }
}
