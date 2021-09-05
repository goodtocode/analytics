using GoodToCode.Shared.Analytics.Abstractions;
using GoodToCode.Shared.Blob.Abstractions;
using System;

namespace GoodToCode.Analytics.Domain
{
    public class NamedEntity : RowEntity, INamedEntity
    {
        public string Category { get; set; }

        public string SubCategory { get; set; }

        public double Confidence { get; set; }

        public string AnalyzedText { get; set; }

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


