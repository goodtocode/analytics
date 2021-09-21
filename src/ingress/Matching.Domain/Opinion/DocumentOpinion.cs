﻿using GoodToCode.Shared.Analytics.Abstractions;
using GoodToCode.Shared.Blob.Abstractions;
using System.Text.Json.Serialization;

namespace GoodToCode.Matching.Domain
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

        public DocumentOpinion(ICellData cell, IConfidence result) : base(cell)
        {
            Negative = result.Negative;
            Neutral = result.Neutral;
            Positive = result.Positive;
        }
    }
}