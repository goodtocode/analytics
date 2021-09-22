using GoodToCode.Shared.Analytics.Abstractions;

namespace GoodToCode.Analytics.CognitiveServices.Domain
{
    public interface ISentimentEntity : IRowEntity, IConfidence, ILanguageIso, IAnalyzedText
    {        
        string Sentiment { get; }
    }
}
