using GoodToCode.Shared.Analytics.Abstractions;
using GoodToCode.Shared.Blob.Abstractions;
using System;

namespace GoodToCode.Analytics.Domain
{
    public class SentenceOpinion : RowEntity, ISentenceOpinion
    {
        public string AnalyzedText { get; }
        public string Sentiment { get; }
        public double Negative { get; }
        public double Neutral { get; }
        public double Positive { get; }
        public string LanguageIso { get; }

        public SentenceOpinion() { }

        public SentenceOpinion(ICellData cell, ISentimentResult result) : base(Guid.NewGuid().ToString(), cell)
        {
            AnalyzedText = result.AnalyzedText;
            Sentiment = result.Sentiment.ToString();
            Negative = result.Negative;
            Neutral = result.Neutral;
            Positive = result.Positive;
        }
    }
}