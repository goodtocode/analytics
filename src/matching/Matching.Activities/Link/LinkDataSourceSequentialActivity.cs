using GoodToCode.Analytics.Abstractions;
using GoodToCode.Analytics.Matching.Domain;
using GoodToCode.Shared.Persistence.Abstractions;
using System;
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
            var matchedRowKeys = new List<string>();

            var rulePartitions = filterRules.OrderBy(o => o.Timestamp).GroupBy(r => r.PartitionKey);
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
                    {
                        currResults.Add(new MatchResultEntity<TDataSource>(group.FirstOrDefault(), result));
                        matchedRowKeys.Add(result.RowKey);
                    }
                    remainingDataSource = remainingDataSource.Where(c => matchedRowKeys.Contains(c.RowKey) == false);
                }
            }

            Results = currResults;
            return Results;
        }
    }
}
