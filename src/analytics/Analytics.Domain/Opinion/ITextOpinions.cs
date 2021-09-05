namespace GoodToCode.Analytics.Domain
{
    public interface ITextOpinions
    {
        DocumentOpinion DocumentSentiment { get; set; }
        OpinionSentiments OpinionSentiments { get; set; }
        SentenceOpinion SentenceOpinion { get; set; }
        SentenceSentiment SentenceSentiment { get; set; }
    }
}