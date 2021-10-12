namespace GoodToCode.Analytics.Matching.Domain
{
    public interface IMatchingRule
    {
        string MatchColumn { get; }
        string MatchResult { get; }
        string MatchType { get; }
        string MatchValue { get; }
    }
}