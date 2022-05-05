using Azure.AI.TextAnalytics;
using GoodToCode.Shared.TextAnalytics.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Shared.TextAnalytics.CognitiveServices
{
    public interface ICognitiveServicesService : ITextAnalyzerService
    { 
        Task<IEnumerable<IAnalyticsResult>> ExtractHealthcareEntitiesAsync(string text, string languageIso);
    }
}