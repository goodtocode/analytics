using GoodToCode.Shared.Analytics.CognitiveServices;

namespace GoodToCode.Matching.Unit.Tests
{
    public class TextAnalyzerServiceFactory
    {
        public static ITextAnalyzerService CreateTextAnalyzer()
        {
            return new TextAnalyzerServiceFake();
        }
    }
}
