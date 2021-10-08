using GoodToCode.Analytics.CognitiveServices.Domain;
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using GoodToCode.Shared.TextAnalytics.CognitiveServices;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.CognitiveServices.Activities
{
    public class NamedEntityExtractActivity
    {
        private readonly ITextAnalyzerService serviceAnalyzer;
        private readonly IExcelService serviceExcel;
        private readonly string languageIso = "en-US";

        public NamedEntityExtractActivity(IExcelService serviceExcelReader, ITextAnalyzerService serviceTextAnalyzer)
        {
            serviceAnalyzer = serviceTextAnalyzer;
            serviceExcel = serviceExcelReader;
        }

        public async Task<IEnumerable<NamedEntity>> ExecuteAsync(Stream excelStream, int sheetToAnalyze, int columnToAnalyze)
        {
            var returnValue = new List<NamedEntity>();
            var cellsToAnalyze = serviceExcel.GetColumn(excelStream, sheetToAnalyze, columnToAnalyze);
            foreach (var cell in cellsToAnalyze.Where(c => string.IsNullOrEmpty(c.CellValue) == false))
                returnValue.AddRange(await new NamedEntityExtractActivity(serviceExcel, serviceAnalyzer).ExecuteAsync(cell));

            return returnValue;
        }

        public async Task<IEnumerable<NamedEntity>> ExecuteAsync(IEnumerable<ICellData> cellsToAnalyze)
        {
            var returnValue = new List<NamedEntity>();
            foreach (var cell in cellsToAnalyze.Where(c => string.IsNullOrEmpty(c.CellValue) == false))
                returnValue.AddRange(await new NamedEntityExtractActivity(serviceExcel, serviceAnalyzer).ExecuteAsync(cell));
            return returnValue;
        }

        public async Task<IEnumerable<NamedEntity>> ExecuteAsync(ICellData cellToAnalyze)
        {
            var returnValue = new List<NamedEntity>();
            if (string.IsNullOrWhiteSpace(cellToAnalyze?.CellValue)) return returnValue;
            var analyzed = await serviceAnalyzer.ExtractEntitiesAsync(cellToAnalyze.CellValue, languageIso);
            foreach (var item in analyzed)
                returnValue.Add(new NamedEntity(cellToAnalyze, item));

            return returnValue;
        }
    }
}
