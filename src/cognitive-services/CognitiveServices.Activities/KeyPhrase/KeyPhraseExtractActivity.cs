using GoodToCodeAnalytics.CognitiveServices.Domain;
using GoodToCode.Shared.Analytics.Abstractions;
using GoodToCode.Shared.Analytics.CognitiveServices;
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.CognitiveServices.Activities
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
            var cellsToAnalyze = sd.GetColumn(columnToAnalyze);
            returnValue.AddRange(await new KeyPhraseExtractActivity(serviceExcel, serviceAnalyzer).ExecuteAsync(cellsToAnalyze));

            return returnValue;
        }

        public async Task<IEnumerable<KeyPhraseEntity>> ExecuteAsync(IEnumerable<ICellData> cellsToAnalyze)
        {
            var returnValue = new List<KeyPhraseEntity>();
            foreach (var cell in cellsToAnalyze.Where(c => string.IsNullOrEmpty(c.CellValue) == false))
                returnValue.AddRange(await new KeyPhraseExtractActivity(serviceExcel, serviceAnalyzer).ExecuteAsync(cell));
            return returnValue;
        }

        public async Task<IEnumerable<KeyPhraseEntity>> ExecuteAsync(ICellData cellToAnalyze)
        {
            var returnValue = new List<KeyPhraseEntity>();
            KeyPhrases analyzed;

            if (string.IsNullOrWhiteSpace(cellToAnalyze?.CellValue)) return returnValue;
            analyzed = await serviceAnalyzer.ExtractKeyPhrasesAsync(cellToAnalyze.CellValue, languageIso);
            foreach (var phrase in analyzed)
                returnValue.Add(new KeyPhraseEntity(cellToAnalyze, phrase));

            return returnValue;
        }
    }
}