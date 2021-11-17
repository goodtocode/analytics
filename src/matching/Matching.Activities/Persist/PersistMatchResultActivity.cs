﻿using Azure.Data.Tables;
using GoodToCode.Analytics.Abstractions;
using GoodToCode.Shared.Persistence.Abstractions;
using GoodToCode.Shared.Persistence.StorageTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Matching.Activities
{
    public class PersistMatchResultActivity<TDataSource> where TDataSource : class, IEntity, new()
    {
        public IEnumerable<IMatchResultEntity<TDataSource>> Results;
        private readonly StorageTablesServiceConfiguration configDestination;
        private readonly IStorageTablesService<MatchResultEntity<TDataSource>> servicePersist;

        public PersistMatchResultActivity(StorageTablesServiceConfiguration destinationTableConfig)
        {
            configDestination = destinationTableConfig;
            servicePersist = new StorageTablesService<MatchResultEntity<TDataSource>>(configDestination);
        }

        public async Task<IEnumerable<TableEntity>> ExecuteAsync(IEnumerable<IMatchResultEntity<TDataSource>> entities)
        {
            var returnData = new List<TableEntity>();
            
            await servicePersist.DeletePartitionsAsync(entities.Select(c => c.PartitionKey).Distinct());
            foreach (var row in entities)
                returnData.Add(await ExecuteAsync(row));
            return returnData;
        }

        public async Task<TableEntity> ExecuteAsync(IMatchResultEntity<TDataSource> entity)
        {
            var entityStrong = (MatchResultEntity<TDataSource>)entity;
            return await servicePersist.AddItemAsync(entityStrong);
        }
    }
}