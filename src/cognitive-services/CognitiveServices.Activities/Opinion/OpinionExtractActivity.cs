using GoodToCodeAnalytics.CognitiveServices.Domain;
using GoodToCode.Shared.Analytics.CognitiveServices;
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
        private ITextAnalyzerService serviceAnalyzer;
        private INpoiService serviceExcel;
        private string languageIso = "en-US";

        public OpinionExtractActivity(INpoiService serviceExcelReader, ITextAnalyzerService serviceTextAnalyzer)
        {
            serviceAnalyzer = serviceTextAnalyzer;
            serviceExcel = serviceExcelReader;
        }

        public async Task<IEnumerable<TextOpinions>> ExecuteAsync(Stream excelStream, int sheetToAnalyze, int columnToAnalyze)
        {
            var returnValue = new List<TextOpinions>();
            
            var sheet = serviceExcel.GetWorkbook(excelStream).GetSheetAt(sheetToAnalyze);
            var sd = sheet.ToSheetData();
            var cellsToAnalyze = sd.GetColumn(columnToAnalyze);
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
