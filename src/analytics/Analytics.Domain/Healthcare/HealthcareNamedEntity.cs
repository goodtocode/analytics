using GoodToCode.Shared.Analytics.Abstractions;
using GoodToCode.Shared.Blob.Abstractions;
using System;

namespace GoodToCode.Analytics.Domain
{
    public class HealthcareNamedEntity : RowEntity, IHealthcareNamedEntity
    {
        public string AnalyzedText { get; set; }

        public string Category { get; set; }

        public string SubCategory { get; set; }

        public double Confidence { get; set; }

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