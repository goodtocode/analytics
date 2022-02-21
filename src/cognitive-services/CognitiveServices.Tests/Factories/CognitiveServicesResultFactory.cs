using GoodToCode.Analytics.CognitiveServices.Domain;
using GoodToCode.Shared.TextAnalytics.Abstractions;
using GoodToCode.Shared.Blob.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace GoodToCode.Analytics.CognitiveServices.Tests
{
    public class CognitiveServicesResultFactory
    {
        public static IEnumerable<AnalyticsResult> CreateAnalyticsResults()
        {
            return new List<AnalyticsResult>() { new AnalyticsResult() { Category = "Event", Confidence = 1, SubCategory = "", AnalyzedText = "Admitted to hospital." } };
        }

        public static IAnalyticsResult CreateAnalyticsResultHealthcare()
        {
            return new AnalyticsResult() { Category = "AdministrativeEvent", Confidence = 1, SubCategory = "", AnalyzedText = "Admitted to hospital." };
        }

        public static ICellData CreateCellData()
        {
            return new CellData() { CellValue = "I went to Seattle last week. While there, it was hot and I had heat exhaustion. But overall it was great!", ColumnIndex = 1, ColumnName = "Summary", RowIndex = 1, SheetIndex = 1, SheetName = "Trip Log", WorkbookName = "Trips.xlsx" };
        }

        public static IConfidence CreateConfidence()
        {
            return new Confidence() { Negative = 0.5d, Neutral = 0.5d, Positive = 0.5d };
        }

        public static ISentimentResult CreateSentimentResult()
        {
            return new SentimentResult("I dont know what this sentance means.", SentimentScore.Mixed, 0.5, 0.5, 0.5);
        }

        public static LinkedResult CreateLinkedResult()
        {
            return null;
        }

        public static KeyPhrases CreateKeyPhrases()
        {
            return new KeyPhrases(new List<string>() { "went to Seattle", "last week" });
        }

        public static HealthcareNamedEntity CreateHealthcareEntity()
        {
            return new HealthcareNamedEntity(CreateCellData(), CreateAnalyticsResultHealthcare());
        }

        public static IEnumerable<NamedEntity> CreateNamedEntities()
        {
            return new List<NamedEntity>() { new NamedEntity(CreateCellData(), CreateAnalyticsResults().FirstOrDefault()) };
        }

        public static IEnumerable<OpinionResult> CreateOpinionResults()
        {
            DocumentOpinion doc = new(CreateCellData(), CreateConfidence());
            return new List<OpinionResult>() { new OpinionResult(doc, CreateSentimentResult(), CreateSentimentResult(), CreateSentimentResult()) };
        }

    }
}
