using Azure.AI.TextAnalytics;
using GoodToCode.Shared.TextAnalytics.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoodToCode.Shared.TextAnalytics.CognitiveServices
{
    public static class TextAnalyzerServiceExtensions
    {
        public static IServiceCollection AddTextAnalyzerService(this IServiceCollection collection, IConfiguration config)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (config == null) throw new ArgumentNullException(nameof(config));

            collection.Configure<CognitiveServiceOptions>(config);
            return collection.AddTransient<ITextAnalyzerService, TextAnalyzerService>();
        }

        public static List<ISentimentResult> ToSentimentResult(this AnalyzeSentimentResultCollection results, string languageIso = "en-US")
        {
            List<ISentimentResult> returnValue = new List<ISentimentResult>();
            foreach (var item in results)
            {
                returnValue.AddRange(item.DocumentSentiment.Sentences.Select(x => new SentimentResult(
                    text: x.Text,
                    language: languageIso,
                    sentiment: x.Sentiment,
                    positive: x.ConfidenceScores.Positive,
                    neutral: x.ConfidenceScores.Neutral,
                    negative: x.ConfidenceScores.Negative
                )));
            }
            return returnValue;
        }

        public static ISentimentResult ToSentimentResult(this DocumentSentiment result)
        {

            return new SentimentResult(
                text: result.Sentences.Count > 0 ? result.Sentences.ElementAt(0).Text : string.Empty,
                sentiment: result.Sentiment,
                positive: result.ConfidenceScores.Positive,
                negative: result.ConfidenceScores.Negative,
                neutral: result.ConfidenceScores.Neutral
            );
        }
    }
}
