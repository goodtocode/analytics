using GoodToCode.Shared.TextAnalytics.Abstractions;
using GoodToCode.Shared.Blob.Abstractions;
using System;
using System.Text.Json.Serialization;

namespace GoodToCode.Analytics.CognitiveServices.Domain
{
    public class NamedEntity : RowEntity, INamedEntity
    {

        [JsonInclude]
        public string Category { get; private set; }
        [JsonInclude]
        public string SubCategory { get; private set; }
        [JsonInclude]
        public double Confidence { get; private set; }
        [JsonInclude]
        public string AnalyzedText { get; private set; }

        public NamedEntity() { }

        public NamedEntity(ICellData cell, IAnalyticsResult result) : base(Guid.NewGuid().ToString(), cell)
        {
            AnalyzedText = result.AnalyzedText;
            Category = result.Category;
            SubCategory = result.SubCategory;
            Confidence = result.Confidence;
        }
    }
}


