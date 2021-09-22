using GoodToCode.Shared.Analytics.CognitiveServices;

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
