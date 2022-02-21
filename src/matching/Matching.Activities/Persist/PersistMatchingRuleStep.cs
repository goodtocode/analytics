using Azure.Data.Tables;
using GoodToCode.Analytics.Abstractions;
using GoodToCode.Shared.Persistence.StorageTables;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Matching.Activities
{
    public class PersistMatchingRuleStep
    {
        public IEnumerable<TableEntity> Results;
        private readonly IStorageTablesService<MatchingRuleEntity> servicePersist;

        public PersistMatchingRuleStep(StorageTablesServiceConfiguration configStorage)
        {
            servicePersist = new StorageTablesService<MatchingRuleEntity>(configStorage);
        }

        public async Task<IEnumerable<TableEntity>> ExecuteAsync(IEnumerable<MatchingRuleEntity> dataSource)
        {
            return await servicePersist.AddItemsAsync(dataSource);
        }
    }
}
