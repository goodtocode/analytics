using Azure;
using Azure.AI.TextAnalytics;
using GoodToCode.Shared.TextAnalytics.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoodToCode.Shared.TextAnalytics.CognitiveServices
{
    public class TextAnalyzerService : ITextAnalyzerService
    {
        protected readonly ICognitiveServiceConfiguration config;
        protected readonly AzureKeyCredential credentials;
        protected readonly TextAnalyticsClient client;

        public TextAnalyzerService(ICognitiveServiceConfiguration serviceConfiguration)
        {
            config = serviceConfiguration;
            credentials = new AzureKeyCredential(config.KeyCredential);
            client = new TextAnalyticsClient(new Uri(config.Endpoint), credentials);
        }

        public TextAnalyzerService(CognitiveServiceOptions options) : this(options.Value)
        {
        }

        public async Task<Tuple<ISentimentResult, IEnumerable<ISentimentResult>>> AnalyzeSentimentAsync(string text, string languageIso = "en-US")
        {
            Tuple<ISentimentResult, IEnumerable<ISentimentResult>> returnSentiment = null;
            var returnSentences = new List<ISentimentResult>();
            if (text.Length > 0)
            {
                var result = await client.AnalyzeSentimentAsync(text, languageIso);
                foreach (var sentence in result.Value.Sentences)
                    returnSentences.Add(
                        new SentimentResult(sentence.Text, languageIso, sentence.Sentiment,
                        new Confidence() { Negative = sentence.ConfidenceScores.Negative, Neutral = sentence.ConfidenceScores.Neutral, Positive = sentence.ConfidenceScores.Positive }));
                returnSentiment = new Tuple<ISentimentResult, IEnumerable<ISentimentResult>>(result.Value.ToSentimentResult(), returnSentences);
            }
            return returnSentiment;
        }

        public async Task<IList<ISentimentResult>> AnalyzeSentimentSentencesAsync(string text, string languageIso = "en-US")
        {
            int batchSize = 10;
            var returnSentiment = new List<ISentimentResult>();
            AnalyzeSentimentResultCollection results = null;
            var sentences = Regex.Split(text, @"(?<=[\.!\?])\s+").Where(x => !string.IsNullOrEmpty(x)).ToArray();

            if (sentences.Length > 0)
            {
                for (int count = 0; count < sentences.Count(); count += batchSize)
                {
                    int lengthToCopy = (sentences.Length - count) >= batchSize ? batchSize : (sentences.Length - count);
                    var batch = new string[lengthToCopy];
                    Array.Copy(sentences, count, batch, 0, lengthToCopy);
                    results = await client.AnalyzeSentimentBatchAsync(batch, languageIso);
                    returnSentiment.AddRange(results.ToSentimentResult(languageIso));
                }
            }

            return returnSentiment;
        }

        public async Task<KeyPhrases> ExtractKeyPhrasesAsync(string text, string languageIso = "en-US")
        {
            var response = await client.ExtractKeyPhrasesAsync(text, languageIso);
            return new KeyPhrases(response.Value);
        }

        public async Task<LinkedResult> ExtractEntityLinksAsync(string text, string languageIso = "en-US")
        {
            var response = await client.RecognizeLinkedEntitiesAsync(text);

            var returnData = response.Value.Select(x => new LinkedMatches() { Name = x.Name, Url = x.Url.ToString(), Matches = string.Join(@"\n", x.Matches.Select(m => m.Text).ToArray()) });
            return new LinkedResult(returnData.ToList());
        }

        public async Task<IEnumerable<OpinionResult>> ExtractOpinionAsync(string text, string languageIso = "en-US")
        {
            var returnData = new List<OpinionResult>();

            var documents = new List<string>
                {
                    text
                };

            AnalyzeSentimentResultCollection reviews = await client.AnalyzeSentimentBatchAsync(documents, options: new AnalyzeSentimentOptions()
            {
                IncludeOpinionMining = true
            });


            foreach (AnalyzeSentimentResult review in reviews)
            {

                foreach (SentenceSentiment sentence in review.DocumentSentiment.Sentences)
                {
                    foreach (SentenceOpinion sentenceOpinion in sentence.Opinions)
                    {
                        foreach (AssessmentSentiment assessment in sentenceOpinion.Assessments)
                        {

                            returnData.Add(
                                new OpinionResult(
                                    new SentimentConfidence(review.DocumentSentiment.ConfidenceScores.Positive, review.DocumentSentiment.ConfidenceScores.Negative, review.DocumentSentiment.ConfidenceScores.Neutral),
                                    new SentimentResult(sentence.Text, sentence.Sentiment, sentence.ConfidenceScores.Positive, sentence.ConfidenceScores.Negative, sentence.ConfidenceScores.Neutral),
                                    new SentimentResult(sentenceOpinion.Target.Text, sentence.Sentiment, sentenceOpinion.Target.ConfidenceScores.Positive, sentenceOpinion.Target.ConfidenceScores.Negative, sentenceOpinion.Target.ConfidenceScores.Neutral),
                                    new SentimentResult(assessment.Text, assessment.Sentiment, assessment.ConfidenceScores.Positive, assessment.ConfidenceScores.Negative, assessment.ConfidenceScores.Neutral)
                                    ));
                        }
                    }
                }
            }
            return returnData;
        }

        public async Task<string> DetectLanguageAsync(string text)
        {
            var response = await client.DetectLanguageAsync(text);
            return response.Value.Iso6391Name;
        }

        public async Task<IEnumerable<AnalyticsResult>> ExtractEntitiesAsync(string text, string languageIso = "en-US")
        {
            var response = await client.RecognizeEntitiesAsync(text, languageIso);
            return response.Value.Select(x => new AnalyticsResult() { AnalyzedText = x.Text, SubCategory = x.SubCategory, Category = x.Category.ToString(), Confidence = x.ConfidenceScore });
        }
    }
}
