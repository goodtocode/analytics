using GoodToCode.Analytics.Abstractions;
using System.Text.Json.Serialization;

namespace GoodToCode.Analytics.Matching.Domain
{
    public class MatchingRule : IMatchingRule
    {
        [JsonInclude]
        public string MatchColumn { get; private set; }
        [JsonInclude]
        public string MatchType { get; private set; }
        [JsonInclude]
        public string MatchValue { get; private set; }
        [JsonInclude]
        public string MatchResult { get; private set; }
        [JsonInclude]
        public int Order { get; set; }

        public MatchingRule() { }

        public MatchingRule(string matchColumn, string matchType, string matchValue, string matchResult)
        {
            MatchColumn = matchColumn;
            MatchType = matchType;
            MatchValue = matchValue;
            MatchResult = matchResult;
        }
    }
}