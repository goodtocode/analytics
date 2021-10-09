using GoodToCode.Analytics.CognitiveServices.Domain;
using GoodToCode.Shared.TextAnalytics.Abstractions;
using GoodToCode.Shared.TextAnalytics.CognitiveServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.CognitiveServices.Unit.Tests
{
    public class CognitiveServicesServiceFake : ICognitiveServicesService
    {
        public async Task<Tuple<ISentimentResult, IEnumerable<ISentimentResult>>> AnalyzeSentimentAsync(string text, string languageIso = "en-US")
        {
            return await Task.Run(() =>
                new Tuple<ISentimentResult, IEnumerable<ISentimentResult>>(
                    CognitiveServicesResultFactory.CreateSentimentResult(),
                    new List<ISentimentResult>() { CognitiveServicesResultFactory.CreateSentimentResult() }));
        }
        public async Task<IList<ISentimentResult>> AnalyzeSentimentSentencesAsync(string text, string languageIso = "en-US")
        {
            return await Task.Run(() => new List<ISentimentResult>() { CognitiveServicesResultFactory.CreateSentimentResult() });
        }

        public async Task<string> DetectLanguageAsync(string text)
        {
            return await Task.Run(() => "en-US");
        }

        public async Task<IEnumerable<AnalyticsResult>> ExtractEntitiesAsync(string text, string languageIso = "en-US")
        {
            return await Task.Run(() => CognitiveServicesResultFactory.CreateAnalyticsResults());
        }

        public async Task<LinkedResult> ExtractEntityLinksAsync(string text, string languageIso = "en-US")
        {
            return await Task.Run(() => new LinkedResult(null) { });
        }

        public async Task<IEnumerable<IAnalyticsResult>> ExtractHealthcareEntitiesAsync(string text, string languageIso = "en-US")
        {
            return await Task.Run(() => new List<HealthcareNamedEntity>() { new HealthcareNamedEntity() });
        }

        public async Task<KeyPhrases> ExtractKeyPhrasesAsync(string text, string languageIso = "en-US")
        {
            return await Task.Run(() => CognitiveServicesResultFactory.CreateKeyPhrases());
        }

        public async Task<IEnumerable<OpinionResult>> ExtractOpinionAsync(string text, string languageIso = "en-US")
        {
            return await Task.Run(() => CognitiveServicesResultFactory.CreateOpinionResults());
        }
    }
}
