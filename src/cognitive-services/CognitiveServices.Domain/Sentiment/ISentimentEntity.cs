using GoodToCode.Shared.Analytics.Abstractions;

namespace GoodToCodeAnalytics.CognitiveServices.Domain
{
    public interface ISentimentEntity : IRowEntity, IConfidence, ILanguageIso, IAnalyzedText
    {        
        string Sentiment { get; }
    }
}
