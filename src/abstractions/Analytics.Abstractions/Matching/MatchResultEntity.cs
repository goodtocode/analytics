using GoodToCode.Shared.Persistence.Abstractions;
using System;
using System.Text.Json.Serialization;

namespace GoodToCode.Analytics.Abstractions
{
    public class MatchResultEntity<TDataSource> : IMatchResultEntity<TDataSource> where TDataSource : IEntity
    {
        [JsonInclude]
        public string PartitionKey  { get; set; }
        [JsonInclude]
        public string RowKey  { get; set; }
        [JsonInclude]
        public DateTimeOffset? Timestamp { get; set; }
        [JsonInclude]
        public string MatchRuleRowKey  { get; set; }
        [JsonInclude]
        public string DataSourceRowKey  { get; set; }
        [JsonInclude]
        public string DataSourceString  { get; set; }
        public TDataSource MatchedData { get; private set; }
        [JsonInclude]
        public string MatchColumn  { get; set; }
        [JsonInclude]
        public string MatchResult  { get; set; }
        [JsonInclude]
        public string MatchType  { get; set; }
        [JsonInclude]
        public string MatchValue  { get; set; }

        public MatchResultEntity() { }

        private MatchResultEntity(MatchingRuleEntity rule)
        {
            PartitionKey = rule.PartitionKey;
            RowKey = Guid.NewGuid().ToString();
            MatchRuleRowKey = rule.RowKey;
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

