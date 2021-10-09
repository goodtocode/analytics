using GoodToCode.Analytics.Abstractions;
using GoodToCode.Shared.TextAnalytics.Abstractions;

namespace GoodToCode.Analytics.CognitiveServices.Domain
{
    public interface ISentimentEntity : IRowEntity, IConfidence, ILanguageIso, IAnalyzedText
    {        
        string Sentiment { get; }
    }
}
