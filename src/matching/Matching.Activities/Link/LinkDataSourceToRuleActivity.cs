using GoodToCode.Analytics.Abstractions;
using GoodToCode.Analytics.Matching.Domain;
using GoodToCode.Shared.Persistence.Abstractions;
using System.Collections.Generic;

namespace GoodToCode.Analytics.Matching.Activities
{
    public class LinkDataSourceToRuleActivity<TDataSource> where TDataSource : IEntity
    {
        public List<ChainableFilterHandler<IMatchingRuleEntity>> Handlers { get; } = new List<ChainableFilterHandler<IMatchingRuleEntity>>();
        public IEnumerable<IMatchResultEntity<TDataSource>> Results;

        public LinkDataSourceToRuleActivity()
        {
        }

        public IEnumerable<IMatchResultEntity<TDataSource>> Execute(IEnumerable<MatchingRuleEntity> filterRules, IEnumerable<TDataSource> dataSource)
        {
            var currResults = new List<IMatchResultEntity<TDataSource>>();
            foreach (var rule in filterRules)
            {
                var expression = rule.ToFilterExpression<TDataSource>();
                var workflow = new SingleFilterActivity<TDataSource>(expression);
                var results = workflow.Execute(dataSource);
                foreach (var result in results)
                    currResults.Add(new MatchResultEntity<TDataSource>(rule, result));
            }

            Results = currResults;
            return Results;
        }
    }
}
