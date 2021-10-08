using Azure.Data.Tables;
using GoodToCode.Analytics.Ingress.Domain;
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Persistence.StorageTables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Ingress.Activities
{
    public class WorkbookPersistActivity
    {
        private IStorageTablesService<RowEntity> servicePersist;

        public WorkbookPersistActivity(IStorageTablesServiceConfiguration config)
        {
            servicePersist = new StorageTablesService<RowEntity>(config);
        }

        public async Task<TableEntity> ExecuteAsync(IWorkbookData entity)
        {
            foreach(var sheet in entity.Sheets)
                foreach(var row in sheet.row)
            return await servicePersist.AddItemAsync(entity);
        }
    }
}
