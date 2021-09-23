using GoodToCode.Shared.TextAnalytics.CognitiveServices;

namespace GoodToCode.Analytics.CognitiveServices.Unit.Tests
{
    public class CognitiveServicesServiceFactory
    {
        public static ICognitiveServicesService CreateCognitiveServices()
        {
            return new CognitiveServicesServiceFake();
        }
    }
}
