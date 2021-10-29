using Azure.Data.Tables;
using GoodToCode.Analytics.Abstractions;
using GoodToCode.Shared.Persistence.Abstractions;
using GoodToCode.Shared.Persistence.StorageTables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Matching.Activities
{
    public class PersistDataSourceMatchesActivity<TDataSource> where TDataSource : class, IEntity, new()
    {
        public IEnumerable<IMatchResultEntity<TDataSource>> Results;
        private readonly StorageTablesServiceConfiguration configDestination;
        private readonly IStorageTablesService<TDataSource> servicePersist;


        public PersistDataSourceMatchesActivity(StorageTablesServiceConfiguration destinationTableConfig)
        {
            configDestination = destinationTableConfig;
            servicePersist = new StorageTablesService<TDataSource>(configDestination);
        }

        public async Task<IEnumerable<TableEntity>> ExecuteAsync(IEnumerable<IMatchResultEntity<TDataSource>> entities, string paritionKey)
        {
            var returnData = new List<TableEntity>();
            foreach (var row in entities)
                returnData.Add(await ExecuteAsync(row, paritionKey));
            return returnData;
        }

        public async Task<TableEntity> ExecuteAsync(IMatchResultEntity<TDataSource> entity, string paritionKey)
        {
            var entityDict = entity.ToDictionary();
            entityDict.TryAdd("PartitionKey", paritionKey);
            entityDict.TryAdd("RowKey", Guid.NewGuid().ToString());
            return await servicePersist.AddItemAsync(entityDict);
        }
    }
}
