using Azure.Data.Tables;
using GoodToCodeAnalytics.CognitiveServices.Domain;
using GoodToCode.Shared.Persistence.StorageTables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.CognitiveServices.Activities
{
    public class RowEntityPersistActivity
    {
        private IStorageTablesService<RowEntity> servicePersist;

        public RowEntityPersistActivity(IStorageTablesServiceConfiguration config)
        {
            servicePersist = new StorageTablesService<RowEntity>(config);
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
