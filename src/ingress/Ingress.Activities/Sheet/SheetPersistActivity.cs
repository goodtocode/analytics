using Azure.Data.Tables;
using GoodToCode.Analytics.Ingress.Domain;
using GoodToCode.Shared.Persistence.StorageTables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Ingress.Activities
{
    public class SheetPersistActivity
    {
        private readonly IStorageTablesService<RowEntity> servicePersist;

        public SheetPersistActivity(IStorageTablesServiceConfiguration config)
        {
            servicePersist = new StorageTablesService<RowEntity>(config);
        }

        public async Task<IEnumerable<TableEntity>> ExecuteAsync(IEnumerable<RowEntity> entities)
        {
            return await servicePersist.AddItemsAsync(entities);
        }

        public async Task<TableEntity> ExecuteAsync(RowEntity entity)
        {
            return await servicePersist.AddItemAsync(entity);
        }
    }
}
