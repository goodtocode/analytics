using Azure.Data.Tables;
using GoodToCode.Analytics.Domain;
using GoodToCode.Shared.Persistence.StorageTables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Activities
{
    public class HealthcareEntityPersistActivity
    {
        private IStorageTablesItemService<HealthcareNamedEntity> servicePersist;

        public HealthcareEntityPersistActivity(IStorageTablesServiceConfiguration config)
        {
            servicePersist = new StorageTablesItemService<HealthcareNamedEntity>(config);
        }

        public async Task<IEnumerable<TableEntity>> ExecuteAsync(IEnumerable<HealthcareNamedEntity> entities)
        {
            var returnValue = new List<TableEntity>();

            foreach (var entity in entities)
                returnValue.Add(await servicePersist.AddItemAsync(entity));

            return returnValue;
        }

        public async Task<TableEntity> ExecuteAsync(HealthcareNamedEntity entity)
        {
            return await servicePersist.AddItemAsync(entity);
        }
    }
}
