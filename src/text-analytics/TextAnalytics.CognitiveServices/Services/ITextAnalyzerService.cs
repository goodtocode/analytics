using Azure.AI.TextAnalytics;
using GoodToCode.Shared.TextAnalytics.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Shared.TextAnalytics.CognitiveServices
{
    public interface ITextAnalyzerService
    {
        Task<Tuple<ISentimentResult, IEnumerable<ISentimentResult>>> AnalyzeSentimentAsync(string text, string languageIso);
        Task<IList<ISentimentResult>> AnalyzeSentimentSentencesAsync(string text, string languageIso);
        Task<string> DetectLanguageAsync(string text);
        Task<IEnumerable<AnalyticsResult>> ExtractEntitiesAsync(string text, string languageIso);
        Task<LinkedResult> ExtractEntityLinksAsync(string text, string languageIso);
        Task<KeyPhrases> ExtractKeyPhrasesAsync(string text, string languageIso);
        Task<IEnumerable<OpinionResult>> ExtractOpinionAsync(string text, string languageIso);
    }
}