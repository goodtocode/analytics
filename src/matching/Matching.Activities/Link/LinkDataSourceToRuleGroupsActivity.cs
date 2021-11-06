using GoodToCode.Analytics.Abstractions;
using GoodToCode.Analytics.Matching.Domain;
using GoodToCode.Shared.Persistence.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace GoodToCode.Analytics.Matching.Activities
{
    public class LinkDataSourceToRuleGroupsActivity<TDataSource> where TDataSource : class, IEntity, new()
    {
        public List<ChainableFilterHandler<IMatchingRuleEntity>> Handlers { get; } = new List<ChainableFilterHandler<IMatchingRuleEntity>>();
        public IEnumerable<IMatchResultEntity<TDataSource>> Results;
        public IEnumerable<string> RuleGroupPartitionKeys { get; }


        public LinkDataSourceToRuleGroupsActivity(IEnumerable<string> ruleGroupPartitions)
        {
            RuleGroupPartitionKeys = ruleGroupPartitions;
        }

        public IEnumerable<IMatchResultEntity<TDataSource>> Execute(IEnumerable<MatchingRuleEntity> filterRules, IEnumerable<TDataSource> dataSource)
        {
            var linkWorkflow = new LinkDataSourceToRuleActivity<TDataSource>();
            var linkResults = new List<IMatchResultEntity<TDataSource>>();
            IEnumerable<MatchingRuleEntity> rules;

            foreach(var key in RuleGroupPartitionKeys)
            {
                rules = filterRules.Where(x => x.PartitionKey == key);
                linkResults.AddRange(linkWorkflow.Execute(rules, dataSource));
            }

            Results = linkResults;
            return Results;
        }
    }
}
