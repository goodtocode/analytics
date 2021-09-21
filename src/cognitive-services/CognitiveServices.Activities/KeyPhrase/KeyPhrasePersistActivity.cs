using Azure.Data.Tables;
using GoodToCode.Analytics.Domain;
using GoodToCode.Shared.Persistence.StorageTables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Activities
{
    public class KeyPhrasePersistActivity
    {
        private IStorageTablesService<KeyPhraseEntity> servicePersist;

        public KeyPhrasePersistActivity(IStorageTablesServiceConfiguration config)
        {
            servicePersist = new StorageTablesService<KeyPhraseEntity>(config);
        }

        public async Task<IEnumerable<TableEntity>> ExecuteAsync(IEnumerable<KeyPhraseEntity> entities)
        {
            return await servicePersist.AddItemsAsync(entities);
        }

        public async Task<TableEntity> ExecuteAsync(KeyPhraseEntity entity)
        {
            return await servicePersist.AddItemAsync(entity);
        }

    }
}
