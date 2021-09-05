using GoodToCode.Analytics.Domain;
using GoodToCode.Shared.Blob.Excel;
using GoodToCode.Shared.Persistence.StorageTables;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Activities
{
    public class PersistExcelOrchestration
    {
        private readonly ILogger log;
        private readonly IStorageTablesServiceConfiguration configStorage;

        public PersistExcelOrchestration(ILogger logger, IStorageTablesServiceConfiguration storageConfig)
        {
            log = logger;
            configStorage = storageConfig;
        }

        public async Task ExecuteAsync(Stream excelStream, string documentName = "")
        {
            try
            {
                documentName = string.IsNullOrWhiteSpace(documentName) ? $"Persist-{DateTime.UtcNow:u}" : documentName;
                var service = new NpoiService();
                var wb = service.GetWorkbook(excelStream);
                foreach (var sheet in wb)
                {
                    var sd = sheet.ToSheetData(documentName);
                    if (!sd.Rows.Any()) throw new ArgumentException("Passed sheet does not have any rows.");
                    var header = sd.GetRow(1);
                    var columnsToPersist = header.Cells.Where(c => c.ColumnName.Contains("*"));
                    foreach (var column in columnsToPersist)
                    {
                        await new RowEntityPersistActivity(configStorage).ExecuteAsync(new RowEntity(column));
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