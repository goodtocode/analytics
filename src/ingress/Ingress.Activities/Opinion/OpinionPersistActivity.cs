using Azure.Data.Tables;
using GoodToCode.Matching.Domain;
using GoodToCode.Shared.Persistence.StorageTables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Matching.Activities
{
    public class OpinionPersistActivity
    {
        private IStorageTablesService<DocumentOpinion> serviceDoc;
        private IStorageTablesService<OpinionSentiments> serviceOpinion;
        private IStorageTablesService<SentenceOpinion> serviceSentenceOp;
        private IStorageTablesService<SentenceSentiment> serviceSentenceSen;

        public OpinionPersistActivity(IStorageTablesServiceConfiguration config)
        {
            serviceDoc = new StorageTablesService<DocumentOpinion>(config);
            serviceOpinion = new StorageTablesService<OpinionSentiments>(config);
            serviceSentenceOp = new StorageTablesService<SentenceOpinion>(config);
            serviceSentenceSen = new StorageTablesService<SentenceSentiment>(config);
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