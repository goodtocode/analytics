using GoodToCode.Shared.Persistence.Abstractions;

namespace GoodToCode.Analytics.Abstractions
{
    public interface IMatchResultEntity<TDataSource> : IMatchingRuleEntity where TDataSource : IEntity
    {
        TDataSource MatchedData { get; }
        string MatchRuleRowKey { get; }
        string DataSourceRowKey { get; }
        string DataSourceString { get; }
    }
}