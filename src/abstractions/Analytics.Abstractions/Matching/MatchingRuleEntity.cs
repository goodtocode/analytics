using GoodToCode.Shared.Blob.Abstractions;
using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace GoodToCode.Analytics.Abstractions
{
    public class MatchingRuleEntity : IMatchingRuleEntity, IEquatable<MatchingRuleEntity> , IComparable<MatchingRuleEntity>
    {
        public struct Columns
        {
            public const string MatchColumn = "MatchColumn";
            public const string MatchType = "MatchType";
            public const string MatchValue = "MatchValue";
            public const string MatchResult = "MatchResult";
        }
        
        [JsonInclude]
        public string PartitionKey { get; set; }
        [JsonInclude]
        public string RowKey { get; set; }
        [JsonInclude]
        public DateTimeOffset? Timestamp { get; set; }
        [JsonInclude]
        public string MatchColumn { get; set; }
        [JsonInclude]
        public string MatchResult { get; set; }
        [JsonInclude]
        public string MatchType { get; set; }
        [JsonInclude]
        public string MatchValue { get; set; }
        [JsonInclude]
        public int Order { get; set; }

        public MatchingRuleEntity() { }

        public MatchingRuleEntity(IRowData row, int index)
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
            Order = index;
        }

        public override string ToString()
        {
            return RowKey;
        }

        public override bool Equals(object item)
        {
            if (item == null || !(item is MatchingRuleEntity itemStrong)) return false;
            else return Equals(itemStrong);
        }

        public int CompareTo(MatchingRuleEntity compareItem)
        {
            if (compareItem == null || Timestamp > compareItem.Timestamp)
                return 1;
            else if (Timestamp < compareItem.Timestamp)
                return -1;
            else
                return 0;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (MatchColumn == null ? 1 : MatchColumn.GetHashCode());
                hash = hash * 23 + (MatchValue == null ? 1 : MatchColumn.GetHashCode());
                hash = hash * 23 + (MatchResult == null ? 1 : MatchResult.GetHashCode());
                return hash;
            }
        }

        public bool Equals(MatchingRuleEntity other)
        {
            return other != null && this.Timestamp.Equals(other.Timestamp);
        }
    }
}