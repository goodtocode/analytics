﻿using System.Collections.Generic;

namespace GoodToCode.Analytics.Matching.Domain
{
    public interface IFilterHandler<T>
    {
        IEnumerable<T> ApplyFilter(IEnumerable<T> filterableList);
    }
}