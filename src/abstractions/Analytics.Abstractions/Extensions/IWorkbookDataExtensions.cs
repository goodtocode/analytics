using GoodToCode.Shared.Blob.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoodToCode.Analytics.Abstractions
{
    public static class IWorkbookDataExtensions
    {
        public static IEnumerable<MatchingRuleEntity> ToMatchingRule(this IWorkbookData workbook)
        {
            var returnData = new List<MatchingRuleEntity>();

            if (!workbook.Sheets.Any())
                throw new ArgumentException("Argument list is empty.", workbook.Sheets.GetType().Name);

            foreach (var sheet in workbook.Sheets)
                returnData.AddRange(sheet.ToMatchingRule());

            return returnData;
        }
    }
}