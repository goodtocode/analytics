using GoodToCode.Shared.TextAnalytics.CognitiveServices;

namespace GoodToCode.Analytics.CognitiveServices.Unit.Tests
{
    public class TextAnalyzerServiceFactory
    {
        public static ITextAnalyzerService CreateTextAnalyzer()
        {
            return new TextAnalyzerServiceFake();
        }
    }
}
