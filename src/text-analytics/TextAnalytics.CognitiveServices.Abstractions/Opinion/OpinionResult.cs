namespace GoodToCode.Shared.TextAnalytics.Abstractions
{
    public class OpinionResult : IOpinionResult
    {
        public IConfidence DocumentSentiment { get; set; }
        public ISentimentResult SentenceSentiment { get; set; }
        public ISentimentResult SentenceOpinion { get; set; }
        public ISentimentResult OpinionSentiments { get; set; }

        public OpinionResult(IConfidence documentSentiment, ISentimentResult sentenceSentiment, ISentimentResult sentenceOpinion, ISentimentResult opinionSentiments)
        {
            DocumentSentiment = documentSentiment;
            SentenceSentiment = sentenceSentiment;
            SentenceOpinion = sentenceOpinion;
            OpinionSentiments = opinionSentiments;
        }
    }
}
