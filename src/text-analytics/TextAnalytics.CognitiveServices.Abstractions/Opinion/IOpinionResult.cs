namespace GoodToCode.Shared.TextAnalytics.Abstractions
{
    public interface IOpinionResult
    {
        IConfidence DocumentSentiment { get; set; }
        ISentimentResult OpinionSentiments { get; set; }
        ISentimentResult SentenceOpinion { get; set; }
        ISentimentResult SentenceSentiment { get; set; }
    }
}