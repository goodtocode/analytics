using Azure.Data.Tables;
using GoodToCode.Analytics.Domain;
using GoodToCode.Shared.Persistence.StorageTables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Activities
{
    public class SentimentPersistActivity
    {
        private IStorageTablesItemService<SentimentEntity> servicePersist;

        public SentimentPersistActivity(IStorageTablesServiceConfiguration config)
        {
            servicePersist = new StorageTablesItemService<SentimentEntity>(config);
        }

        public async Task<IEnumerable<TableEntity>> ExecuteAsync(IEnumerable<SentimentEntity> entities)
        {
            var returnValue = new List<TableEntity>();

            foreach (var entity in entities)
                returnValue.Add(await ExecuteAsync(entity));

            return returnValue;
        }

        public async Task<TableEntity> ExecuteAsync(SentimentEntity entity)
        {
            return await servicePersist.AddItemAsync(entity);
        }
    }
}
