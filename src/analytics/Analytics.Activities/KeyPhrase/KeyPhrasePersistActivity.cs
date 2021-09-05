using Azure.Data.Tables;
using GoodToCode.Analytics.Domain;
using GoodToCode.Shared.Persistence.StorageTables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Activities
{
    public class KeyPhrasePersistActivity
    {
        private IStorageTablesItemService<KeyPhraseEntity> servicePersist;

        public KeyPhrasePersistActivity(IStorageTablesServiceConfiguration config)
        {
            servicePersist = new StorageTablesItemService<KeyPhraseEntity>(config);
        }

        public async Task<IEnumerable<TableEntity>> ExecuteAsync(IEnumerable<KeyPhraseEntity> entities)
        {
            var returnValue = new List<TableEntity>();

            foreach (var entity in entities)
                returnValue.Add(await ExecuteAsync(entity));

            return returnValue;
        }

        public async Task<TableEntity> ExecuteAsync(KeyPhraseEntity entity)
        {
            return await servicePersist.AddItemAsync(entity);
        }

    }
}
