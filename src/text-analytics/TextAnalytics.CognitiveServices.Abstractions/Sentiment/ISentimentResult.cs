using System;

namespace GoodToCode.Shared.TextAnalytics.Abstractions
{
    public interface ISentimentResult : IAnalyzedText, IConfidence, ILanguageIso
    {
        Enum Sentiment { get; }
    }
}