using System.Collections.Generic;

namespace GoodToCode.Analytics.Abstractions
{
    public interface IDataSourceMatch<TDataSource>
    {
        IEnumerable<TDataSource> MatchedData { get; }
        IEnumerable<FilterExpression<TDataSource>> MatchedRules { get; }
    }
}