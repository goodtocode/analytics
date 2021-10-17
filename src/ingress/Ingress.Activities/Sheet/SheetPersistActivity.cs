using Azure.Data.Tables;
using GoodToCode.Analytics.Abstractions;
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Persistence.Abstractions;
using GoodToCode.Shared.Persistence.StorageTables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Ingress.Activities
{
    public class SheetPersistActivity
    {
        private readonly IStorageTablesService<CellEntity> servicePersist;

        public SheetPersistActivity(IStorageTablesServiceConfiguration config)
        {
            servicePersist = new StorageTablesService<CellEntity>(config);
        }

        public async Task<IEnumerable<TableEntity>> ExecuteAsync(IEnumerable<ISheetData> entities)
        {
            var returnData = new List<TableEntity>();
            foreach(var entity in entities)
                returnData.Add(await ExecuteAsync(entity));

            return returnData;
        }

        public async Task<TableEntity> ExecuteAsync(ISheetData entity)
        {

            return await servicePersist.AddItemAsync(entity.Rows.ToDictionary());
        }
    }
}
