﻿using GoodToCode.Analytics.Matching.Domain;
using System.Collections.Generic;

namespace GoodToCode.Analytics.Matching.Activities
{
    public interface IMultiFilterActivity<T>
    {
        MultiFilterHandler<T> Handler { get; }
        IEnumerable<T> Results { get; }

        IEnumerable<T> Execute(IEnumerable<T> listToFilter);
    }
}