using Azure.Data.Tables;
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Persistence.StorageTables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Ingress.Activities
{
    public class WorkbookPersistActivity
    {
        private readonly SheetPersistActivity activityPersist;

        public WorkbookPersistActivity(IStorageTablesServiceConfiguration config)
        {
            activityPersist = new SheetPersistActivity(config);
        }

        public async Task<IEnumerable<TableEntity>> ExecuteAsync(IWorkbookData entity, string paritionKey)
        {
            return await activityPersist.ExecuteAsync(entity.Sheets, paritionKey);
        }
    }
}