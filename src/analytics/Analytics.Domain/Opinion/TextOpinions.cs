using GoodToCode.Shared.Analytics.Abstractions;
using GoodToCode.Shared.Blob.Abstractions;

namespace GoodToCode.Analytics.Domain
{
    public class TextOpinions : ITextOpinions
    {
        public DocumentOpinion DocumentSentiment { get; set; }
        public OpinionSentiments OpinionSentiments { get; set; }
        public SentenceOpinion SentenceOpinion { get; set; }
        public SentenceSentiment SentenceSentiment { get; set; }

        public TextOpinions(ICellData cell, IOpinionResult result)
        {
            DocumentSentiment = new DocumentOpinion(cell, result.DocumentSentiment);
            OpinionSentiments = new OpinionSentiments(cell, result.OpinionSentiments);
            SentenceOpinion = new SentenceOpinion(cell, result.SentenceOpinion);
            SentenceSentiment = new SentenceSentiment(cell, result.SentenceSentiment);
        }
    }
}