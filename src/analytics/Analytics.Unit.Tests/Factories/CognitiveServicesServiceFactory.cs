using GoodToCode.Shared.Analytics.CognitiveServices;

namespace GoodToCode.Analytics.Unit.Tests
{
    public class CognitiveServicesServiceFactory
    {
        public static ICognitiveServicesService CreateCognitiveServices()
        {
            return new CognitiveServicesServiceFake();
        }
    }
}
