using GoodToCode.Analytics.Domain;
using GoodToCode.Shared.Analytics.CognitiveServices;
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Activities
{
    public class KeyPhraseExtractActivity
    {
        private ITextAnalyzerService serviceAnalyzer;
        private INpoiService serviceExcel;
        private string languageIso = "en-US";

        public KeyPhraseExtractActivity(INpoiService serviceExcelReader, ITextAnalyzerService serviceTextAnalyzer)
        {
            serviceAnalyzer = serviceTextAnalyzer;
            serviceExcel = serviceExcelReader;
        }

        public async Task<IEnumerable<KeyPhraseEntity>> ExecuteAsync(Stream excelStream, int sheetToAnalyze, int columnToAnalyze)
        {
            var returnValue = new List<KeyPhraseEntity>();
            
            var sheet = serviceExcel.GetWorkbook(excelStream).GetSheetAt(sheetToAnalyze);
            var sd = sheet.ToSheetData();
            var columnsToAnalyze = sd.GetColumn(columnToAnalyze);
            returnValue.AddRange(await new KeyPhraseExtractActivity(serviceExcel, serviceAnalyzer).ExecuteAsync(columnsToAnalyze));

            return returnValue;
        }

        public async Task<IEnumerable<KeyPhraseEntity>> ExecuteAsync(IEnumerable<ICellData> cellsToAnalyze)
        {
            var returnValue = new List<KeyPhraseEntity>();
            foreach (var column in cellsToAnalyze)
                returnValue.AddRange(await new KeyPhraseExtractActivity(serviceExcel, serviceAnalyzer).ExecuteAsync(column));
            return returnValue;
        }

        public async Task<IEnumerable<KeyPhraseEntity>> ExecuteAsync(ICellData cellToAnalyze)
        {
            var returnValue = new List<KeyPhraseEntity>();
            var analyzed = await serviceAnalyzer.ExtractKeyPhrasesAsync(cellToAnalyze.CellValue, languageIso);
            foreach(var phrase in analyzed)
                returnValue.Add(new KeyPhraseEntity(cellToAnalyze, phrase));
            return returnValue;
        }
    }
}