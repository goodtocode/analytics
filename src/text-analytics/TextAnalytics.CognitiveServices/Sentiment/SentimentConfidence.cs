using GoodToCode.Shared.TextAnalytics.Abstractions;

namespace GoodToCode.Shared.TextAnalytics.CognitiveServices
{
    /// <summary>
    /// Abstraction of Azure.AI.TextAnalytics.TextSentiment
    /// </summary>
    public class SentimentConfidence : IConfidence
    {
        public double Positive { get; } = 0.0;
        public double Neutral { get; } = 0.0;
        public double Negative { get; } = 0.0;

        public SentimentConfidence(double positive, double neutral, double negative)
        {
            Positive = positive;
            Neutral = neutral;
            Negative = negative;
        }

        public SentimentConfidence(IConfidence confidence) : this(confidence.Positive, confidence.Neutral, confidence.Negative)
        {

        }
    }
}
