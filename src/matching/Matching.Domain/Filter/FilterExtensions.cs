using System;
using System.Collections.Generic;
using System.Text;

namespace GoodToCode.Analytics.Matching.Filter
{
    public static class FilterExtensions
    {
        public static IEnumerable<product> WhereInStock(this IEnumerable<product> p)
        {
            return p.Where(x => x.Qty > 0);
        }
    }
}
