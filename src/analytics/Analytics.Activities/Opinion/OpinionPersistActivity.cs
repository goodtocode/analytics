using Azure.Data.Tables;
using GoodToCode.Analytics.Domain;
using GoodToCode.Shared.Persistence.StorageTables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Activities
{
    public class OpinionPersistActivity
    {
        private IStorageTablesItemService<DocumentOpinion> serviceDoc;
        private IStorageTablesItemService<OpinionSentiments> serviceOpinion;
        private IStorageTablesItemService<SentenceOpinion> serviceSentenceOp;
        private IStorageTablesItemService<SentenceSentiment> serviceSentenceSen;

        public OpinionPersistActivity(IStorageTablesServiceConfiguration config)
        {
            serviceDoc = new StorageTablesItemService<DocumentOpinion>(config);
            serviceOpinion = new StorageTablesItemService<OpinionSentiments>(config);
            serviceSentenceOp = new StorageTablesItemService<SentenceOpinion>(config);
            serviceSentenceSen = new StorageTablesItemService<SentenceSentiment>(config);
        }

        public async Task<IEnumerable<TableEntity>> ExecuteAsync(IEnumerable<TextOpinions> entities)
        {
            var returnValue = new List<TableEntity>();

            foreach (var item in entities)
                returnValue.AddRange(await ExecuteAsync(item));

            return returnValue;
        }

        public async Task<IEnumerable<TableEntity>> ExecuteAsync(TextOpinions entities)
        {
            var returnValue = new List<TableEntity>();

            returnValue.Add(await serviceDoc.AddItemAsync(entities.DocumentSentiment));
            returnValue.Add(await serviceOpinion.AddItemAsync(entities.OpinionSentiments));
            returnValue.Add(await serviceSentenceOp.AddItemAsync(entities.SentenceOpinion));
            returnValue.Add(await serviceSentenceSen.AddItemAsync(entities.SentenceSentiment));

            return returnValue;
        }

    }
}