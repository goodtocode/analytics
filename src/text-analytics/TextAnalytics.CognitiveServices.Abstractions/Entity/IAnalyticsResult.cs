
namespace GoodToCode.Shared.TextAnalytics.Abstractions
{
    public interface IAnalyticsResult: IAnalyzedText
    {
        string Category { get; }
        string SubCategory { get; }
        double Confidence { get; }
    }
}
