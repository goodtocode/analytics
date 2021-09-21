using GoodToCode.Shared.Analytics.Abstractions;
using GoodToCode.Shared.Blob.Abstractions;
using System.Text.Json.Serialization;

namespace GoodToCode.Matching.Domain
{
    public class TextOpinions : ITextOpinions
    {
        [JsonInclude]
        public DocumentOpinion DocumentSentiment { get; private set; }
        [JsonInclude]
        public OpinionSentiments OpinionSentiments { get; private set; }
        [JsonInclude]
        public SentenceOpinion SentenceOpinion { get; private set; }
        [JsonInclude]
        public SentenceSentiment SentenceSentiment { get; private set; }

        public TextOpinions() { }

        public TextOpinions(ICellData cell, IOpinionResult result)
        {
            DocumentSentiment = new DocumentOpinion(cell, result.DocumentSentiment);
            OpinionSentiments = new OpinionSentiments(cell, result.OpinionSentiments);
            SentenceOpinion = new SentenceOpinion(cell, result.SentenceOpinion);
            SentenceSentiment = new SentenceSentiment(cell, result.SentenceSentiment);
        }
    }
}