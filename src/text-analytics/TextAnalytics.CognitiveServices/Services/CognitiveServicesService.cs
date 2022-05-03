using Azure.AI.TextAnalytics;
using GoodToCode.Shared.TextAnalytics.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Shared.TextAnalytics.CognitiveServices
{
    public class CognitiveServicesService : TextAnalyzerService, ICognitiveServicesService
    {
        public CognitiveServicesService(ICognitiveServiceConfiguration serviceConfiguration) : base(serviceConfiguration)
        {
        }

        public CognitiveServicesService(CognitiveServiceOptions options) : this(options.Value)
        {
        }

        public async Task<IEnumerable<IAnalyticsResult>> ExtractHealthcareEntitiesAsync(string text, string languageIso = "en-US")
        {
            List<IAnalyticsResult> returnData = new List<IAnalyticsResult>();
            string document1 = text;
            List<string> batchInput = new List<string>()
                {
                    document1,
                    string.Empty
                };
            var options = new AnalyzeHealthcareEntitiesOptions { };
            AnalyzeHealthcareEntitiesOperation healthOperation = await client.StartAnalyzeHealthcareEntitiesAsync(batchInput, languageIso, options);
            await healthOperation.WaitForCompletionAsync();
            await foreach (AnalyzeHealthcareEntitiesResultCollection documentsInPage in healthOperation.Value)
            {
                foreach (AnalyzeHealthcareEntitiesResult entitiesInDoc in documentsInPage)
                {
                    if (!entitiesInDoc.HasError)
                    {
                        foreach (var entity in entitiesInDoc.Entities)
                        {
                            returnData.Add(new HealthcareResult() { AnalyzedText = entity.Text, Category = entity.Category.ToString(), SubCategory = entity.SubCategory, Confidence = entity.ConfidenceScore });
                        }
                    }
                }
            }
            return returnData;
        }
    }
}
