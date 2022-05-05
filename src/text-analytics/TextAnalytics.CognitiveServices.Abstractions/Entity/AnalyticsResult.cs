namespace GoodToCode.Shared.TextAnalytics.Abstractions
{
    public class AnalyticsResult : IAnalyticsResult
    {
        public string AnalyzedText { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public double Confidence { get; set; }
    }
}
