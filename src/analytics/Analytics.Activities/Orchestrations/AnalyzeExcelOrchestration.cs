using GoodToCode.Shared.Analytics.CognitiveServices;
using GoodToCode.Shared.Blob.Excel;
using GoodToCode.Shared.Persistence.StorageTables;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Activities
{
    public class AnalyzeExcelOrchestration
    {
        private readonly ILogger log;
        private readonly IStorageTablesServiceConfiguration configOriginal;
        private readonly IStorageTablesServiceConfiguration configHealthcare;
        private readonly IStorageTablesServiceConfiguration configKeyPhrase;
        private readonly IStorageTablesServiceConfiguration configOpinion;
        private readonly IStorageTablesServiceConfiguration configSentiment;
        private readonly ITextAnalyzerService serviceAnalyzer;
        private readonly INpoiService serviceExcel;
        private string languageIso = "en-US";

        public AnalyzeExcelOrchestration(ILogger logger, IStorageTablesServiceConfiguration storageConfig, ITextAnalyzerService service, INpoiService serviceSheet)
        {
            log = logger;
            serviceAnalyzer = service;
            serviceExcel = serviceSheet;
            configOriginal = storageConfig;
            configHealthcare = new StorageTablesServiceConfiguration(configOriginal.ConnectionString, $"{configOriginal.TableName}-Healthcare");
            configKeyPhrase = new StorageTablesServiceConfiguration(configOriginal.ConnectionString, $"{configOriginal.TableName}-KeyPhrase");
            configOpinion = new StorageTablesServiceConfiguration(configOriginal.ConnectionString, $"{configOriginal.TableName}-Opinion");
            configSentiment = new StorageTablesServiceConfiguration(configOriginal.ConnectionString, $"{configOriginal.TableName}-Sentiment");
        }

        public async Task ExecuteAsync(Stream excelStream, string documentName = "")
        {
            try
            {
                documentName = string.IsNullOrWhiteSpace(documentName) ? $"Analytics-{DateTime.UtcNow:u}" : documentName;
                var wb = serviceExcel.GetWorkbook(excelStream);
                foreach (var sheet in wb)
                {
                    var sd = sheet.ToSheetData(documentName);
                    if (!sd.Rows.Any()) throw new ArgumentException("Passed sheet does not have any rows.");
                    var header = sd.GetRow(1);
                    var columnsToAnalyze = header.Cells.Where(c => c.ColumnName.Contains("*"));
                    foreach (var column in columnsToAnalyze)
                    {
                        for (var count = 2; count <= sd.Rows.Count(); count++)
                        {
                            var rowColumnToAnalyze = sd.GetRow(count).Cells.FirstOrDefault(c => c.ColumnIndex == column.ColumnIndex);

                            var kpReturn = await new KeyPhraseExtractActivity(serviceExcel, serviceAnalyzer).ExecuteAsync(column);
                            await new KeyPhrasePersistActivity(configKeyPhrase).ExecuteAsync(kpReturn);

                            var toReturn = await new OpinionExtractActivity(serviceExcel, serviceAnalyzer).ExecuteAsync(column);
                            await new OpinionPersistActivity(configOpinion).ExecuteAsync(toReturn);

                            var tsReturn = await new SentimentAnalyzeActivity(serviceExcel, serviceAnalyzer).ExecuteAsync(column);
                            await new SentimentPersistActivity(configSentiment).ExecuteAsync(tsReturn);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}