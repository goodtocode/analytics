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
    public class NamedEntityExtractActivity
    {
        private ITextAnalyzerService serviceAnalyzer;
        private INpoiService serviceExcel;
        private string languageIso = "en-US";

        public NamedEntityExtractActivity(INpoiService serviceExcelReader, ITextAnalyzerService serviceTextAnalyzer)
        {
            serviceAnalyzer = serviceTextAnalyzer;
            serviceExcel = serviceExcelReader;
        }

        public async Task<IEnumerable<NamedEntity>> ExecuteAsync(Stream excelStream, int sheetToAnalyze, int columnToAnalyze)
        {
            var returnValue = new List<NamedEntity>();
            var sheet = serviceExcel.GetWorkbook(excelStream).GetSheetAt(sheetToAnalyze);
            var sd = sheet.ToSheetData();
            var cellsToAnalyze = sd.GetColumn(columnToAnalyze);
            foreach (var cell in cellsToAnalyze.Where(c => c.CellValue?.Length > 0))
                returnValue.AddRange(await new NamedEntityExtractActivity(serviceExcel, serviceAnalyzer).ExecuteAsync(cell));

            return returnValue;
        }

        public async Task<IEnumerable<NamedEntity>> ExecuteAsync(IEnumerable<ICellData> cellsToAnalyze)
        {
            var returnValue = new List<NamedEntity>();
            foreach (var cell in cellsToAnalyze.Where(c => c.CellValue?.Length > 0))
                returnValue.AddRange(await new NamedEntityExtractActivity(serviceExcel, serviceAnalyzer).ExecuteAsync(cell));
            return returnValue;
        }

        public async Task<IEnumerable<NamedEntity>> ExecuteAsync(ICellData cellToAnalyze)
        {
            var returnValue = new List<NamedEntity>();
            if (cellToAnalyze.CellValue?.Length == 0) return returnValue;
            var analyzed = await serviceAnalyzer.ExtractEntitiesAsync(cellToAnalyze.CellValue, languageIso);
            foreach (var item in analyzed)
                returnValue.Add(new NamedEntity(cellToAnalyze, item));

            return returnValue;
        }
    }
}
