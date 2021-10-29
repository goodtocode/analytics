using GoodToCode.Shared.Persistence.Abstractions;

namespace GoodToCode.Analytics.Abstractions
{
    public class MatchResultEntity<TDataSource> : IMatchResultEntity<TDataSource> where TDataSource : IEntity
    {
        public string DataSourceRowKey { get; set; }
        public string DataSourceString { get; set; }
        public TDataSource MatchedData { get; }
        public string MatchColumn { get; }
        public string MatchResult { get; }
        public string MatchType { get; }
        public string MatchValue { get; }
        public string RowKey { get; }
        public string PartitionKey { get; }

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

