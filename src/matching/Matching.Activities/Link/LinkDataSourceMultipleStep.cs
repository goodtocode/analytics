﻿using GoodToCode.Analytics.Abstractions;
using GoodToCode.Analytics.Matching.Domain;
using GoodToCode.Shared.Persistence.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace GoodToCode.Analytics.Matching.Activities
{
    public class LinkDataSourceMultipleStep<TDataSource> where TDataSource : IEntity
    {
        public List<ChainableFilterHandler<IMatchingRuleEntity>> Handlers { get; } = new List<ChainableFilterHandler<IMatchingRuleEntity>>();
        public IEnumerable<IMatchResultEntity<TDataSource>> Results;

        public LinkDataSourceMultipleStep()
        {
        }

        public IEnumerable<IMatchResultEntity<TDataSource>> Execute(IEnumerable<MatchingRuleEntity> filterRules, IEnumerable<TDataSource> dataSource)
        {
            var currResults = new List<IMatchResultEntity<TDataSource>>();
            IEnumerable<TDataSource> results;
            
            var ruleGroups = filterRules.OrderBy(o => o.Order).GroupBy(r => r.MatchResult);
            foreach(var group in ruleGroups)
            {
                var expression = group.ToFilterExpression<TDataSource>();
                if (group.Count() > 1)
                    results = new MultiFilterStep<TDataSource>(expression).Execute(dataSource);
                else 
                    results = new SingleFilterStep<TDataSource>(expression.FirstOrDefault()).Execute(dataSource);
                foreach (var result in results)
                    currResults.Add(new MatchResultEntity<TDataSource>(group.FirstOrDefault(), result));
            }

            Results = currResults;
            return Results;
        }
    }
}
