using GoodToCode.Analytics.Domain;
using GoodToCode.Shared.Analytics.CognitiveServices;
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Activities
{
    public class SentimentAnalyzeActivity
    {
        private ITextAnalyzerService serviceAnalyzer;
        private INpoiService serviceExcel;

        public SentimentAnalyzeActivity(INpoiService serviceExcelReader, ITextAnalyzerService serviceTextAnalyzer)
        {
            serviceAnalyzer = serviceTextAnalyzer;
            serviceExcel = serviceExcelReader;
        }

        private const int characterLimit = 5120;
        private const int kBLimit = 1024;
        private const int maxDocsLanguage = 1000;
        private const int maxDocsSentiment = 10;
        private const int maxDocsOpinion = 10;
        private const int maxDocsKeyPhrase = 10;
        private const int maxDocsNamedEntity = 5;
        private const int maxDocsEntityLinks = 5;
        private const int maxDocsHealthcare = 10;

        private const int characterLimitAnalyzeEndpoint = 125000;
        private const int maxDocsAnalyzeEndpoint = 25;
        private string languageIso = "en-US";

        /// <summary>

        /// Maximum size of entire request	1 MB. Also applies to Text Analytics for health.
        /// Max Documents Per Request
        ///     Language Detection	1000
        ///     Sentiment Analysis	10
        ///     Opinion Mining	10
        ///     Key Phrase Extraction	10
        ///     Named Entity Recognition	5
        ///     Entity Linking	5
        ///     Text Analytics for health	10 for the web-based API, 1000 for the container.
        ///     Analyze endpoint    25 for all operations.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool IsValid(string text)
        {
            /// Min size 1
            if (string.IsNullOrWhiteSpace(text))
                return false;

            /// Maximum size of a single document	5,120 characters as measured by StringInfo.LengthInTextElements. Also applies to Text Analytics for health.
            if (text?.Length > characterLimit)
                return false;

            if (string.IsNullOrWhiteSpace(text) || text?.Length > characterLimit)
                return false;

            /// Maximum size of a single document (/analyze endpoint)	125K characters as measured by StringInfo.LengthInTextElements. Does not apply to Text Analytics for health.

            // Sentiment Analysis  10 sentences per request
            var sentences = Regex.Split(text, @"(?<=[\.!\?])\s+");
            if (sentences.Length > 1000)
                return false;

            return true;
        }

        public async Task<IEnumerable<SentimentEntity>> ExecuteAsync(Stream excelStream, int sheetToAnalyze, int columnToAnalyze)
        {

            var returnValue = new List<SentimentEntity>();
            var sheet = serviceExcel.GetWorkbook(excelStream).GetSheetAt(sheetToAnalyze);
            var sd = sheet.ToSheetData();
            var columnsToAnalyze = sd.GetColumn(columnToAnalyze);
            foreach (var column in columnsToAnalyze.Where(c => c.CellValue?.Length > 0))
                returnValue.AddRange(await new SentimentAnalyzeActivity(serviceExcel, serviceAnalyzer).ExecuteAsync(column));

            return returnValue;
        }

        public async Task<IEnumerable<SentimentEntity>> ExecuteAsync(IEnumerable<ICellData> cellsToAnalyze)
        {
            var returnValue = new List<SentimentEntity>();
            foreach (var column in cellsToAnalyze.Where(c => c.CellValue?.Length > 0))
                returnValue.AddRange(await new SentimentAnalyzeActivity(serviceExcel, serviceAnalyzer).ExecuteAsync(column));
            return returnValue;
        }

        public async Task<IEnumerable<SentimentEntity>> ExecuteAsync(ICellData cellToAnalyze)
        {
            var returnValue = new List<SentimentEntity>();
            if (cellToAnalyze.CellValue?.Length == 0) return returnValue;
            var analyzed = await serviceAnalyzer.AnalyzeSentimentSentencesAsync(cellToAnalyze.CellValue, languageIso);
            foreach (var item in analyzed)
                returnValue.Add(new SentimentEntity(cellToAnalyze, item));
            return returnValue;
        }
    }
}
