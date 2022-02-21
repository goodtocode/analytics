using GoodToCode.Shared.TextAnalytics.CognitiveServices;

namespace GoodToCode.Analytics.CognitiveServices.Tests
{
    public class TextAnalyzerServiceFactory
    {
        public static ITextAnalyzerService CreateTextAnalyzer()
        {
            return new TextAnalyzerServiceFake();
        }
    }
}
