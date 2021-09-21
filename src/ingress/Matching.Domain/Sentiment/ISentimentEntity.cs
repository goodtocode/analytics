using GoodToCode.Shared.Analytics.Abstractions;

namespace GoodToCode.Matching.Domain
{
    public interface ISentimentEntity : IRowEntity, IConfidence, ILanguageIso, IAnalyzedText
    {        
        string Sentiment { get; }
    }
}
