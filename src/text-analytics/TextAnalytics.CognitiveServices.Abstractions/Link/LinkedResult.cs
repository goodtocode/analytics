using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GoodToCode.Shared.TextAnalytics.Abstractions
{
    public class LinkedResult : ReadOnlyCollection<LinkedMatches>
    {
        public LinkedResult(IList<LinkedMatches> list) : base(list)
        {
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
