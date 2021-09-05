using GoodToCode.Shared.Analytics.Abstractions;
using GoodToCode.Shared.Blob.Abstractions;
using System;

namespace GoodToCode.Analytics.Domain
{
    public class SentimentEntity : RowEntity, ISentimentEntity
    {
        public string AnalyzedText { get; set; }
        public string Sentiment { get; set; }
        public double Negative { get; set; }
        public double Neutral { get; set; }
        public double Positive { get; set; }
        public string LanguageIso { get; set; }

        public SentimentEntity() { }

        public SentimentEntity(ICellData cell, ISentimentResult result) : base(Guid.NewGuid().ToString(), cell)
        {
            AnalyzedText = result.AnalyzedText;
            Sentiment = result.Sentiment.ToString();
            Negative = result.Negative;
            Neutral = result.Neutral;
            Positive = result.Positive;
            LanguageIso = result.LanguageIso;
        }
    }
}