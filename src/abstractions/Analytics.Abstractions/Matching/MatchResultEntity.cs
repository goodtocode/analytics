using GoodToCode.Shared.Persistence.Abstractions;
using System.Text.Json.Serialization;

namespace GoodToCode.Analytics.Abstractions
{
    public class MatchResultEntity<TDataSource> : IMatchResultEntity<TDataSource> where TDataSource : IEntity
    {
        [JsonInclude]
        public string PartitionKey { get; }
        [JsonInclude]
        public string RowKey { get; }
        [JsonInclude]
        public string DataSourceRowKey { get; }
        [JsonInclude]
        public string DataSourceString { get; }
        public TDataSource MatchedData { get; private set; }
        [JsonInclude]
        public string MatchColumn { get; }
        [JsonInclude]
        public string MatchResult { get; }
        [JsonInclude]
        public string MatchType { get; }
        [JsonInclude]
        public string MatchValue { get; }

        private MatchResultEntity(MatchingRuleEntity rule)
        {
            PartitionKey = rule.PartitionKey;
            RowKey = rule.RowKey;
            MatchColumn = rule.MatchColumn;
            MatchResult = rule.MatchResult;
            MatchType = rule.MatchType;
            MatchValue = rule.MatchValue;
        }

        public MatchResultEntity(MatchingRuleEntity rule, TDataSource data) : this(rule)
        {
            MatchedData = data;
            DataSourceRowKey = data.RowKey;
            DataSourceString = data.ToString();
        }
    }
}

