using System;
using System.Globalization;

namespace GoodToCode.Shared.TextAnalytics.Abstractions
{
    /// <summary>
    /// Abstraction of Azure.AI.TextAnalytics.TextSentiment
    /// </summary>
    public class SentimentResult : ISentimentResult
    {
        public string AnalyzedText { get; } = string.Empty;

        public string LanguageIso { get; } = "en-US";

        public Enum Sentiment { get; } = SentimentScore.Neutral;

        public double Negative { get; }

        public double Neutral { get; }

        public double Positive { get; }

        public SentimentResult(string text, Enum sentiment, double positive, double neutral, double negative, string language)
        {
            Positive = positive;
            Neutral = neutral;
            Negative = negative;
            AnalyzedText = text;
            Sentiment = sentiment;
            LanguageIso = language;
        }

        public SentimentResult(string text, Enum sentiment, double positive, double neutral, double negative) : this(text, sentiment, positive, neutral, negative, "en-US")
        {
        }

        public SentimentResult(string text, string language, Enum sentiment, IConfidence confidence) : this(text, sentiment, confidence.Positive, confidence.Neutral, confidence.Negative, language)
        {
        }
    }
}
