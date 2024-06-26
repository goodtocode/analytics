﻿using System;
using System.Collections.Generic;

namespace GoodToCode.Analytics.Abstractions
{
    public static class IMatchingRuleEntityExtensions
    {
        public static FilterExpression<T> ToFilterExpression<T>(this IMatchingRuleEntity rule)
        {
            return ToFilterExpression<T>((MatchingRuleEntity)rule);
        }
        
        public static FilterExpression<T> ToFilterExpression<T>(this MatchingRuleEntity rule)
        {
            var matchColumn = rule.MatchColumn;

            if (typeof(T).GetProperty(matchColumn) == null)
            {
                matchColumn = rule.MatchColumn.ToIdentifier();
                if (typeof(T).GetProperty(matchColumn) == null)
                    throw new ArgumentException("MatchColumn value must match a property in T", rule.GetType().Name);
            }

            return rule.MatchType switch
            {
                MatchType.BeginsWith => new FilterExpression<T>(x => x.GetType().GetProperty(matchColumn).GetValue(x, null) != null 
                                                                && x.GetType().GetProperty(matchColumn).GetValue(x, null).ToString().StartsWith(rule.MatchValue)),
                MatchType.EndsWith => new FilterExpression<T>(x => x.GetType().GetProperty(matchColumn).GetValue(x, null) != null
                                                                && x.GetType().GetProperty(matchColumn).GetValue(x, null).ToString().EndsWith(rule.MatchValue)),
                MatchType.IsEqual => new FilterExpression<T>(x => x.GetType().GetProperty(matchColumn).GetValue(x, null) != null
                                                                && x.GetType().GetProperty(matchColumn).GetValue(x, null).ToString() == rule.MatchValue),
                MatchType.NotEquals => new FilterExpression<T>(x => x.GetType().GetProperty(matchColumn).GetValue(x, null) != null
                                                                && x.GetType().GetProperty(matchColumn).GetValue(x, null).ToString() != rule.MatchValue),
                _ => new FilterExpression<T>(x => x.GetType().GetProperty(matchColumn).GetValue(x, null) != null
                                                                && x.GetType().GetProperty(matchColumn).GetValue(x, null).ToString().Contains(rule.MatchValue)),
            };
        }

        public static IEnumerable<FilterExpression<T>> ToFilterExpression<T>(this IEnumerable<MatchingRuleEntity> rules)
        {
            var returnData = new List<FilterExpression<T>>();
            foreach(var rule in rules)
                returnData.Add(rule.ToFilterExpression<T>());
            return returnData;
        }
    }
}