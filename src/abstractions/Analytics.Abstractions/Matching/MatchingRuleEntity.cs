using GoodToCode.Shared.Blob.Abstractions;
using System;
using System.Linq;
using System.Text.Json.Serialization;

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
        
        [JsonInclude]
        public string PartitionKey  { get; set; }
        [JsonInclude]
        public string RowKey  { get; set; }
        [JsonInclude]
        public string MatchColumn  { get; set; }
        [JsonInclude]
        public string MatchResult  { get; set; }
        [JsonInclude]
        public string MatchType  { get; set; }
        [JsonInclude]
        public string MatchValue  { get; set; }

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