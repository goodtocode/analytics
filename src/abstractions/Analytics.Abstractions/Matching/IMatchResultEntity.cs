using GoodToCode.Shared.Persistence.Abstractions;

namespace GoodToCode.Analytics.Abstractions
{
    public interface IMatchResultEntity<TDataSource> : IMatchingRuleEntity where TDataSource : IEntity
    {
        TDataSource MatchedData { get; }
    }
}