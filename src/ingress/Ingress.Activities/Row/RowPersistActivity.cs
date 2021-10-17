using Azure.Data.Tables;
using GoodToCode.Analytics.Abstractions;
using GoodToCode.Shared.Persistence.StorageTables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Ingress.Activities
{
    public class RowPersistActivity
    {
        private readonly IStorageTablesService<CellEntity> servicePersist;

        public RowPersistActivity(IStorageTablesServiceConfiguration config)
        {
            servicePersist = new StorageTablesService<CellEntity>(config);
        }

        public async Task<IEnumerable<TableEntity>> ExecuteAsync(IEnumerable<CellEntity> entities)
        {
            return await servicePersist.AddItemsAsync(entities);
        }

        public async Task<TableEntity> ExecuteAsync(CellEntity entity)
        {
            return await servicePersist.AddItemAsync(entity);
        }
    }
}
