namespace GoodToCode.Shared.TextAnalytics.Abstractions
{
    public class Confidence : IConfidence
    {
        public double Negative { get; set; }
        public double Neutral { get; set; }
        public double Positive { get; set; }
    }
}