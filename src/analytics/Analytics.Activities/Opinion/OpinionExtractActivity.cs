using GoodToCode.Analytics.Domain;
using GoodToCode.Shared.Analytics.CognitiveServices;
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using System.Collections.Generic;
using System.IO;
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
            var columnsToAnalyze = sd.GetColumn(columnToAnalyze);
            returnValue.AddRange(await new OpinionExtractActivity(serviceExcel, serviceAnalyzer).ExecuteAsync(columnsToAnalyze));

            return returnValue;
        }

        public async Task<IEnumerable<TextOpinions>> ExecuteAsync(IEnumerable<ICellData> cellsToAnalyze)
        {
            var returnValue = new List<TextOpinions>();
            foreach (var column in cellsToAnalyze)
                returnValue.AddRange(await new OpinionExtractActivity(serviceExcel, serviceAnalyzer).ExecuteAsync(column));
            return returnValue;
        }

        public async Task<IEnumerable<TextOpinions>> ExecuteAsync(ICellData cellToAnalyze)
        {
            var returnValue = new List<TextOpinions>();
            var analyzeResults = await serviceAnalyzer.ExtractOpinionAsync(cellToAnalyze.CellValue, languageIso);
            foreach (var result in analyzeResults)
                returnValue.Add(new TextOpinions(cellToAnalyze, result));
            return returnValue;
        }
    }
}
