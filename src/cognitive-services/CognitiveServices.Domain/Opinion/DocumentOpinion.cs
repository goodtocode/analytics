using GoodToCode.Shared.TextAnalytics.Abstractions;
using GoodToCode.Shared.Blob.Abstractions;
using System.Text.Json.Serialization;
using GoodToCode.Analytics.Abstractions;
using System.Collections.Generic;

namespace GoodToCode.Analytics.CognitiveServices.Domain
{
    public class DocumentOpinion : RowEntity, IDocumentOpinion
    {
        [JsonInclude]
        public double Negative { get; private set; }
        [JsonInclude]
        public double Neutral { get; private set; }
        [JsonInclude]
        public double Positive { get; private set; }

        public DocumentOpinion() { }

        public DocumentOpinion(ICellData cell, IConfidence result) : base(new List<ICellData>() { cell })
        {
            Negative = result.Negative;
            Neutral = result.Neutral;
            Positive = result.Positive;
        }
    }
}