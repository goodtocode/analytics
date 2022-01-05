using Azure.Data.Tables;
using GoodToCode.Shared.Persistence.Abstractions;
using GoodToCode.Shared.Persistence.StorageTables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Matching.Activities
{
    public class PersistDataSourceActivity<TDataSource> where TDataSource : class, IEntity, new()
    {
        public IEnumerable<TableEntity> Results;
        private readonly IStorageTablesService<TDataSource> servicePersist;

        public PersistDataSourceActivity(StorageTablesServiceConfiguration configStorage)
        {
            servicePersist = new StorageTablesService<TDataSource>(configStorage);
        }

        public async Task<IEnumerable<TableEntity>> ExecuteAsync(IEnumerable<TDataSource> dataSource)
        {
            return await servicePersist.AddItemsAsync(dataSource);
        }
    }
}
