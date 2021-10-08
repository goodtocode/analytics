using GoodToCode.Analytics.CognitiveServices.Domain;
using GoodToCode.Shared.TextAnalytics.CognitiveServices;
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.CognitiveServices.Activities
{
    public class OpinionExtractActivity
    {
        private readonly ITextAnalyzerService serviceAnalyzer;
        private readonly IExcelService serviceExcel;
        private readonly string languageIso = "en-US";

        public OpinionExtractActivity(IExcelService serviceExcelReader, ITextAnalyzerService serviceTextAnalyzer)
        {
            serviceAnalyzer = serviceTextAnalyzer;
            serviceExcel = serviceExcelReader;
        }

        public async Task<IEnumerable<TextOpinions>> ExecuteAsync(Stream excelStream, int sheetToAnalyze, int columnToAnalyze)
        {
            var returnValue = new List<TextOpinions>();

            var cellsToAnalyze = serviceExcel.GetColumn(excelStream, sheetToAnalyze, columnToAnalyze);
            returnValue.AddRange(await new OpinionExtractActivity(serviceExcel, serviceAnalyzer).ExecuteAsync(cellsToAnalyze));

            return returnValue;
        }

        public async Task<IEnumerable<TextOpinions>> ExecuteAsync(IEnumerable<ICellData> cellsToAnalyze)
        {
            var returnValue = new List<TextOpinions>();
            foreach (var cell in cellsToAnalyze.Where(c => string.IsNullOrEmpty(c.CellValue) == false))
                returnValue.AddRange(await new OpinionExtractActivity(serviceExcel, serviceAnalyzer).ExecuteAsync(cell));
            return returnValue;
        }

        public async Task<IEnumerable<TextOpinions>> ExecuteAsync(ICellData cellToAnalyze)
        {
            var returnValue = new List<TextOpinions>();
            if (string.IsNullOrWhiteSpace(cellToAnalyze?.CellValue)) return returnValue;
            var analyzeResults = await serviceAnalyzer.ExtractOpinionAsync(cellToAnalyze.CellValue, languageIso);
            foreach (var result in analyzeResults)
                returnValue.Add(new TextOpinions(cellToAnalyze, result));
            return returnValue;
        }
    }
}
