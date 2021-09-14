using GoodToCode.Analytics.Domain;
using GoodToCode.Shared.Analytics.CognitiveServices;
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Activities
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
            foreach (var cell in cellsToAnalyze.Where(c => c.CellValue?.Length > 0))
                returnValue.AddRange(await new OpinionExtractActivity(serviceExcel, serviceAnalyzer).ExecuteAsync(cell));
            return returnValue;
        }

        public async Task<IEnumerable<TextOpinions>> ExecuteAsync(ICellData cellToAnalyze)
        {
            var returnValue = new List<TextOpinions>();
            if (cellToAnalyze.CellValue?.Length == 0) return returnValue;
            var analyzeResults = await serviceAnalyzer.ExtractOpinionAsync(cellToAnalyze.CellValue, languageIso);
            foreach (var result in analyzeResults)
                returnValue.Add(new TextOpinions(cellToAnalyze, result));
            return returnValue;
        }
    }
}
