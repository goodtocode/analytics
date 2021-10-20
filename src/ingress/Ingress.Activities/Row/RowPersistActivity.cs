using Azure.Data.Tables;
using GoodToCode.Analytics.Abstractions;
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Persistence.StorageTables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Ingress.Activities
{
    public class RowPersistActivity
    {
        private readonly IStorageTablesService<RowEntity> servicePersist;

        public RowPersistActivity(IStorageTablesServiceConfiguration config)
        {
            servicePersist = new StorageTablesService<RowEntity>(config);
        }

        public async Task<IEnumerable<TableEntity>> ExecuteAsync(IEnumerable<IRowData> entities, string paritionKey)
        {
            var returnData = new List<TableEntity>();
            foreach (var row in entities)
                returnData.Add(await ExecuteAsync(row, paritionKey));
            return returnData;
        }

        public async Task<TableEntity> ExecuteAsync(IRowData entity, string paritionKey)
        {
            var entityDict = entity.ToDictionary();
            entityDict.Add("PartitionKey", paritionKey);
            entityDict.Add("RowKey", Guid.NewGuid().ToString());
            return await servicePersist.AddItemAsync(entityDict);
        }
    }
}
