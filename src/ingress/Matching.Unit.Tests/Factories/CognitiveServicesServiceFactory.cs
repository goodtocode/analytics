using GoodToCode.Shared.Analytics.CognitiveServices;

namespace GoodToCode.Matching.Unit.Tests
{
    public class CognitiveServicesServiceFactory
    {
        public static ICognitiveServicesService CreateCognitiveServices()
        {
            return new CognitiveServicesServiceFake();
        }
    }
}
