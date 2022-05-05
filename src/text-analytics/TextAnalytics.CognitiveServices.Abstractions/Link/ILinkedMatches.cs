namespace GoodToCode.Shared.TextAnalytics.Abstractions
{
    public interface ILinkedMatches
    {
        string Matches { get; set; }
        string Name { get; set; }
        string Url { get; set; }
    }
}