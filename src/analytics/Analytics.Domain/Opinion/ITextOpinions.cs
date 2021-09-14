namespace GoodToCode.Analytics.Domain
{
    public interface ITextOpinions
    {
        DocumentOpinion DocumentSentiment { get; }
        OpinionSentiments OpinionSentiments { get; }
        SentenceOpinion SentenceOpinion { get;  }
        SentenceSentiment SentenceSentiment { get; }
    }
}