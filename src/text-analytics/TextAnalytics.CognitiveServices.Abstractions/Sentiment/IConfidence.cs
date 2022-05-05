namespace GoodToCode.Shared.TextAnalytics.Abstractions
{
    public interface IConfidence
    {
        double Negative { get; }
        double Neutral { get; }
        double Positive { get; }
    }
}