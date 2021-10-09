using GoodToCode.Analytics.CognitiveServices.Domain;
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using GoodToCode.Shared.TextAnalytics.Abstractions;
using GoodToCode.Shared.TextAnalytics.CognitiveServices;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.CognitiveServices.Activities
{
    public class KeyPhraseExtractActivity
    {
        private readonly ITextAnalyzerService serviceAnalyzer;
        private readonly IExcelService serviceExcel;
        private readonly string languageIso = "en-US";

        public KeyPhraseExtractActivity(IExcelService serviceExcelReader, ITextAnalyzerService serviceTextAnalyzer)
        {
            serviceAnalyzer = serviceTextAnalyzer;
            serviceExcel = serviceExcelReader;
        }

        public async Task<IEnumerable<KeyPhraseEntity>> ExecuteAsync(Stream excelStream, int sheetToAnalyze, int columnToAnalyze)
        {
            var returnValue = new List<KeyPhraseEntity>();

            var cellsToAnalyze = serviceExcel.GetColumn(excelStream, sheetToAnalyze, columnToAnalyze);
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