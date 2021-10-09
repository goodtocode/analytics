using Azure.Data.Tables;
using GoodToCode.Analytics.CognitiveServices.Domain;
using GoodToCode.Shared.Persistence.StorageTables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.CognitiveServices.Activities
{
    public class HealthcareEntityPersistActivity
    {
        private readonly IStorageTablesService<HealthcareNamedEntity> servicePersist;

        public HealthcareEntityPersistActivity(IStorageTablesServiceConfiguration config)
        {
            servicePersist = new StorageTablesService<HealthcareNamedEntity>(config);
        }

        public async Task<IEnumerable<TableEntity>> ExecuteAsync(IEnumerable<HealthcareNamedEntity> entities)
        {
            return await servicePersist.AddItemsAsync(entities);
        }

        public async Task<TableEntity> ExecuteAsync(HealthcareNamedEntity entity)
        {
            return await servicePersist.AddItemAsync(entity);
        }
    }
}
