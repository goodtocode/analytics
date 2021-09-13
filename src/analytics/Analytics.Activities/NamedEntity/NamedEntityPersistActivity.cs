using Azure.Data.Tables;
using GoodToCode.Analytics.Domain;
using GoodToCode.Shared.Persistence.StorageTables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Activities
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
            var returnValue = new List<TableEntity>();

            foreach (var entity in entities)
                returnValue.Add(await ExecuteAsync(entity));

            return returnValue;
        }

        public async Task<TableEntity> ExecuteAsync(NamedEntity entity)
        {
            return await servicePersist.AddItemAsync(entity);
        }
    }
}
