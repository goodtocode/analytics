using GoodToCode.Shared.Analytics.CognitiveServices;

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
