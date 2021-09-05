using GoodToCode.Shared.Analytics.Abstractions;
using GoodToCode.Shared.Blob.Abstractions;

namespace GoodToCode.Analytics.Domain
{
    public class DocumentOpinion : RowEntity, IDocumentOpinion
    {
        public double Negative { get; }
        public double Neutral { get; }
        public double Positive { get; }

        public DocumentOpinion() { }

        public DocumentOpinion(ICellData cell, IConfidence result) : base(cell)
        {
            Negative = result.Negative;
            Neutral = result.Neutral;
            Positive = result.Positive;
        }
    }
}