﻿using System.Collections.Generic;

namespace GoodToCode.Analytics.Abstractions
{
    public class DataSourceMatch<TDataSource> : IDataSourceMatch<TDataSource>
    {
        public IEnumerable<FilterExpression<TDataSource>> MatchedRules { get; }
        public IEnumerable<TDataSource> MatchedData { get; }

        public DataSourceMatch(IEnumerable<FilterExpression<TDataSource>> rules, IEnumerable<TDataSource> data)
        {
            MatchedRules = rules; 
            MatchedData = data;
        }
    }
}
