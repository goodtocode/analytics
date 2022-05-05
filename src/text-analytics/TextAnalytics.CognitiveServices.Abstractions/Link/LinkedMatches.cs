namespace GoodToCode.Shared.TextAnalytics.Abstractions
{
    public class LinkedMatches : ILinkedMatches
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Matches { get; set; }
    }
}
