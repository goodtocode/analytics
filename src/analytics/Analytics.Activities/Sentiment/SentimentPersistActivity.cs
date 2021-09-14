using Azure.Data.Tables;
using GoodToCode.Analytics.Domain;
using GoodToCode.Shared.Persistence.StorageTables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Activities
{
    public class SentimentPersistActivity
    {
        private IStorageTablesService<SentimentEntity> servicePersist;

        public SentimentPersistActivity(IStorageTablesServiceConfiguration config)
        {
            servicePersist = new StorageTablesService<SentimentEntity>(config);
        }

        public async Task<IEnumerable<TableEntity>> ExecuteAsync(IEnumerable<SentimentEntity> entities)
        {
            return await servicePersist.AddItemsAsync(entities);
        }

        public async Task<TableEntity> ExecuteAsync(SentimentEntity entity)
        {
            return await servicePersist.AddItemAsync(entity);
        }
    }
}
