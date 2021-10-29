using GoodToCode.Shared.Persistence.Abstractions;
using System.Collections.Generic;

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

        public MatchResultEntity(MatchingRuleEntity rule, TDataSource data)
        {
            this.Fill(rule);
            MatchedData = data;
            DataSourceRowKey = data.RowKey;
            DataSourceString = data.ToString();
        }
    }
}

