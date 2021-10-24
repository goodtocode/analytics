using GoodToCode.Shared.Blob.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoodToCode.Analytics.Abstractions
{
    public static class ISheetDataExtensions
    {
        public static IEnumerable<MatchingRuleEntity> ToMatchingRule(this ISheetData sheet)
        {
            IEnumerable<MatchingRuleEntity> returnData;

            if (!sheet.Rows.Any())
                throw new ArgumentException("Argument list is empty.", sheet.Rows.GetType().Name);

            returnData = sheet.Rows.Select(r => new MatchingRuleEntity(r)).ToList();

            return returnData;
        }
    }
}