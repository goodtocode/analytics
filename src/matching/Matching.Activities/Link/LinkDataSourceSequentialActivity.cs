using GoodToCode.Analytics.Abstractions;
using GoodToCode.Analytics.Matching.Domain;
using GoodToCode.Shared.Persistence.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace GoodToCode.Analytics.Matching.Activities
{
    public class LinkDataSourceSequentialActivity<TDataSource> where TDataSource : IEntity
    {
        public List<ChainableFilterHandler<IMatchingRuleEntity>> Handlers { get; } = new List<ChainableFilterHandler<IMatchingRuleEntity>>();
        public IEnumerable<IMatchResultEntity<TDataSource>> Results;

        public LinkDataSourceSequentialActivity()
        {
        }

        public IEnumerable<IMatchResultEntity<TDataSource>> Execute(IEnumerable<MatchingRuleEntity> filterRules, IEnumerable<TDataSource> dataSource)
        {
            var currResults = new List<IMatchResultEntity<TDataSource>>();
            IEnumerable<TDataSource> filteredResults;
            IEnumerable<TDataSource> remainingDataSource = dataSource;

            var rulePartitions = filterRules.GroupBy(r => r.PartitionKey);
            foreach (var partition in rulePartitions)
            {
                var ruleGroups = partition.GroupBy(r => r.MatchResult);
                foreach (var group in ruleGroups)
                {
                    var expression = group.ToFilterExpression<TDataSource>();
                    if (group.Count() > 1)
                        filteredResults = new SequentialFilterActivity<TDataSource>(expression).Execute(remainingDataSource);
                    else
                        filteredResults = new SingleFilterActivity<TDataSource>(expression.FirstOrDefault()).Execute(remainingDataSource);
                    foreach (var result in filteredResults)
                        currResults.Add(new MatchResultEntity<TDataSource>(group.FirstOrDefault(), result));
                    remainingDataSource = remainingDataSource.Where(c => filteredResults.Select(r => r.RowKey).Contains(c.RowKey) == false);
                }
            }

            Results = currResults;
            return Results;
        }
    }
}
