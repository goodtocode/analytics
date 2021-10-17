using Azure.Data.Tables;
using GoodToCode.Analytics.Abstractions;
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Persistence.Abstractions;
using GoodToCode.Shared.Persistence.StorageTables;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Ingress.Activities
{
    public class WorkbookPersistActivity
    {
        private readonly IStorageTablesService<CellEntity> servicePersist;

        public WorkbookPersistActivity(IStorageTablesServiceConfiguration config)
        {
            servicePersist = new StorageTablesService<CellEntity>(config);
        }

        public async Task<IEnumerable<TableEntity>> ExecuteAsync(IWorkbookData entity)
        {
            var returnData = new List<TableEntity>();
            foreach (var sheet in entity.Sheets)
            {
                var rows = sheet.Rows.ToDictionary();
                returnData.Add(await servicePersist.AddItemAsync(rows));
            }

            return returnData;
        }
    }
}