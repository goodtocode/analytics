using GoodToCode.Analytics.Abstractions;
using GoodToCode.Analytics.Matching.Domain;
using System.Collections.Generic;

namespace GoodToCode.Analytics.Matching.Activities
{
    public class LinkDataSourceToRuleActivity<TDataSource>
    {
        public List<ChainableFilterHandler<IMatchingRuleEntity>> Handlers { get; } = new List<ChainableFilterHandler<IMatchingRuleEntity>>();
        public IDataSourceMatch<TDataSource> Results;

        public LinkDataSourceToRuleActivity()
        {
        }

        public IDataSourceMatch<TDataSource> Execute(IEnumerable<FilterExpression<TDataSource>> filterRules, IEnumerable<TDataSource> dataSource)
        {
            var workflow = new MultiFilterActivity<TDataSource>(filterRules);
            var results = workflow.Execute(dataSource);
            Results = new DataSourceMatch<TDataSource>(filterRules, results);

            return Results;
        }
    }
}
