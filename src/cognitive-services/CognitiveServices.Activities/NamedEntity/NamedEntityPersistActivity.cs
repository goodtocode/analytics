using Azure.Data.Tables;
using GoodToCode.Analytics.CognitiveServices.Domain;
using GoodToCode.Shared.Persistence.StorageTables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.CognitiveServices.Activities
{
    public class NamedEntityPersistActivity
    {
        private IStorageTablesService<NamedEntity> servicePersist;

        public NamedEntityPersistActivity(IStorageTablesServiceConfiguration config)
        {
            servicePersist = new StorageTablesService<NamedEntity>(config);
        }

        public async Task<IEnumerable<TableEntity>> ExecuteAsync(IEnumerable<NamedEntity> entities)
        {
            return await servicePersist.AddItemsAsync(entities);
        }

        public async Task<TableEntity> ExecuteAsync(NamedEntity entity)
        {
            return await servicePersist.AddItemAsync(entity);
        }
    }
}
