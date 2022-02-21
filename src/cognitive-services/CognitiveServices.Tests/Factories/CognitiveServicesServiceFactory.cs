using GoodToCode.Shared.TextAnalytics.CognitiveServices;

namespace GoodToCode.Analytics.CognitiveServices.Tests
{
    public class CognitiveServicesServiceFactory
    {
        public static ICognitiveServicesService CreateCognitiveServices()
        {
            return new CognitiveServicesServiceFake();
        }
    }
}
