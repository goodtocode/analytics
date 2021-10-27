using GoodToCode.Analytics.Matching.Domain;
using GoodToCode.Shared.Blob.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoodToCode.Analytics.Abstractions
{
    public class MatchingRuleEntity : IMatchingRuleEntity
    {
        public struct Columns
        {
            public const string MatchColumn = "MatchColumn";
            public const string MatchType = "MatchType";
            public const string MatchValue = "MatchValue";
            public const string MatchResult = "MatchResult";
        }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string MatchColumn { get; set; }
        public string MatchResult { get; set; }
        public string MatchType { get; set; }
        public string MatchValue { get; set; }

        public MatchingRuleEntity() { }

        public MatchingRuleEntity(IRowData row)
        {
            var cells = row.Cells;
            if (!cells.Any())
                throw new ArgumentException("Argument list is empty.", cells.GetType().Name);

            PartitionKey = cells.FirstOrDefault().SheetName;
            RowKey = Guid.NewGuid().ToString();
            MatchColumn = cells.Where(c => c.ColumnName == Columns.MatchColumn).FirstOrDefault().CellValue;
            MatchType = cells.Where(c => c.ColumnName == Columns.MatchType).FirstOrDefault().CellValue;
            MatchValue = cells.Where(c => c.ColumnName == Columns.MatchValue).FirstOrDefault().CellValue;
            MatchResult = cells.Where(c => c.ColumnName == Columns.MatchResult).FirstOrDefault().CellValue;
        }
    }
}