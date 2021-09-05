using Azure.Data.Tables;
using GoodToCode.Analytics.Domain;
using GoodToCode.Shared.Persistence.StorageTables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Activities
{
    public class RowEntityPersistActivity
    {
        private IStorageTablesItemService<RowEntity> servicePersist;

        public RowEntityPersistActivity(IStorageTablesServiceConfiguration config)
        {
            servicePersist = new StorageTablesItemService<RowEntity>(config);
        }

        public async Task<IEnumerable<TableEntity>> ExecuteAsync(IEnumerable<RowEntity> entities)
        {
            var returnValue = new List<TableEntity>();

            foreach (var entity in entities)
                returnValue.Add(await ExecuteAsync(entity));

            return returnValue;
        }

        public async Task<TableEntity> ExecuteAsync(RowEntity entity)
        {
            return await servicePersist.AddItemAsync(entity);
        }
    }
}
