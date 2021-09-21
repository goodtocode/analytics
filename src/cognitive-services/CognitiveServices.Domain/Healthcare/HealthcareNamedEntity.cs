using GoodToCode.Shared.Analytics.Abstractions;
using GoodToCode.Shared.Blob.Abstractions;
using System;
using System.Text.Json.Serialization;

namespace GoodToCode.Analytics.Domain
{
    public class HealthcareNamedEntity : RowEntity, IHealthcareNamedEntity
    {
        [JsonInclude]
        public string AnalyzedText { get; private set; }
        [JsonInclude]
        public string Category { get; private set; }
        [JsonInclude]
        public string SubCategory { get; private set; }
        [JsonInclude]
        public double Confidence { get; private set; }

        public HealthcareNamedEntity() { }

        public HealthcareNamedEntity(ICellData cell, IAnalyticsResult result) : base(Guid.NewGuid().ToString(), cell)
        {
            AnalyzedText = result.AnalyzedText;
            Category = result.Category;
            SubCategory = result.SubCategory;
            Confidence = result.Confidence;
        }

    }
}