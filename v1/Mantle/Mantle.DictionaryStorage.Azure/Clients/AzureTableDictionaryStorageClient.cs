using System.Collections.Generic;
using System.Linq;
using Mantle.Configuration.Attributes;
using Mantle.DictionaryStorage.Azure.Entities;
using Mantle.DictionaryStorage.Entities;
using Mantle.DictionaryStorage.Interfaces;
using Mantle.Extensions;
using Mantle.FaultTolerance.Interfaces;
using Mantle.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Mantle.DictionaryStorage.Azure.Clients
{
    public class AzureTableDictionaryStorageClient<T> : IDictionaryStorageClient<T>
        where T : class, new()
    {
        private readonly ITransientFaultStrategy transientFaultStrategy;
        private readonly ITypeMetadata<T> typeMetadata;

        private CloudStorageAccount cloudStorageAccount;
        private CloudTableClient cloudTableClient;

        public AzureTableDictionaryStorageClient(ITransientFaultStrategy transientFaultStrategy,
                                                 ITypeMetadata<T> typeMetadata)
        {
            this.transientFaultStrategy = transientFaultStrategy;
            this.typeMetadata = typeMetadata;

            AutoSetup = true;
        }

        public CloudStorageAccount CloudStorageAccount => GetCloudStorageAccount();

        public CloudTableClient CloudTableClient => GetCloudTableClient();

        [Configurable]
        public bool AutoSetup { get; set; }

        [Configurable(IsRequired = true)]
        public string TableName { get; set; }

        [Configurable(IsRequired = true)]
        public string StorageConnectionString { get; set; }

        public bool DeleteEntity(string entityId, string partitionId)
        {
            partitionId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            var table = CloudTableClient.GetTableReference(TableName);

            if (transientFaultStrategy.Try(() => table.Exists()))
            {
                var retrieveOp =
                    TableOperation.Retrieve<AzureTableDictionaryStorageEntity<T>>(partitionId, entityId);

                var retrieveResult = transientFaultStrategy.Try(() => table.Execute(retrieveOp));

                if (retrieveResult.Result != null)
                {
                    var deleteOp =
                        TableOperation.Delete(retrieveResult.Result as AzureTableDictionaryStorageEntity<T>);

                    transientFaultStrategy.Try(() => table.Execute(deleteOp));

                    return true;
                }
            }

            return false;
        }

        public bool DeletePartition(string partitionId)
        {
            partitionId.Require(nameof(partitionId));

            var table = CloudTableClient.GetTableReference(TableName);

            if (transientFaultStrategy.Try(() => table.Exists()))
            {
                var query = new TableQuery<AzureTableDictionaryStorageEntity<T>>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionId));

                var entities = transientFaultStrategy.Try(() => table.ExecuteQuery(query).ToList());

                if (entities.Any())
                {
                    var entityChunks = transientFaultStrategy.Try(() => table.ExecuteQuery(query).Chunk(100).ToList());

                    foreach (var entityChunk in entityChunks)
                    {
                        var batchDeleteOp = new TableBatchOperation();

                        foreach (var entity in entityChunk)
                            batchDeleteOp.Delete(entity);

                        transientFaultStrategy.Try(() => table.ExecuteBatch(batchDeleteOp));
                    }

                    return true;
                }
            }

            return false;
        }

        public bool DoesEntityExist(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            var table = CloudTableClient.GetTableReference(TableName);

            if (transientFaultStrategy.Try(() => table.Exists()) == false)
                return false;

            var op = TableOperation.Retrieve<AzureTableDictionaryStorageEntity<T>>(partitionId, entityId);

            return (transientFaultStrategy.Try(() => table.Execute(op).Result) != null);
        }

        public void InsertOrUpdateDictionaryStorageEntities(IEnumerable<DictionaryStorageEntity<T>> dsEntities)
        {
            dsEntities.Require(nameof(dsEntities));

            var table = CloudTableClient.GetTableReference(TableName);

            if (AutoSetup)
                transientFaultStrategy.Try(() => table.CreateIfNotExists());

            var groups = dsEntities
                .Select(e => new AzureTableDictionaryStorageEntity<T>(typeMetadata)
                {
                    Data = e.Entity,
                    PartitionKey = e.PartitionId,
                    RowKey = e.EntityId
                })
                .GroupBy(e => e.PartitionKey);

            foreach (var group in groups)
            {
                var chunks = transientFaultStrategy.Try(() => group.Chunk(100).ToList());

                foreach (var chunk in chunks)
                {
                    var batchOp = new TableBatchOperation();

                    foreach (var storageEntity in chunk)
                        batchOp.InsertOrReplace(storageEntity);

                    transientFaultStrategy.Try(() => table.ExecuteBatch(batchOp));
                }
            }
        }

        public void InsertOrUpdateDictionaryStorageEntity(DictionaryStorageEntity<T> dsEntity)
        {
            dsEntity.Require(nameof(dsEntity));

            var storageEntity = new AzureTableDictionaryStorageEntity<T>(typeMetadata);

            storageEntity.Data = dsEntity.Entity;
            storageEntity.RowKey = dsEntity.EntityId;
            storageEntity.PartitionKey = dsEntity.PartitionId;

            var table = CloudTableClient.GetTableReference(TableName);

            if (AutoSetup)
                transientFaultStrategy.Try(() => table.CreateIfNotExists());

            var op = TableOperation.InsertOrReplace(storageEntity);

            transientFaultStrategy.Try(() => table.Execute(op));
        }

        public IEnumerable<DictionaryStorageEntity<T>> LoadAllDictionaryStorageEntities(string partitionId)
        {
            partitionId.Require(nameof(partitionId));

            var table = CloudTableClient.GetTableReference(TableName);

            if (transientFaultStrategy.Try(() => table.Exists()))
            {
                var query = new TableQuery<AzureTableDictionaryStorageEntity<T>>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionId));

                var queryResults = transientFaultStrategy.Try(() => table.ExecuteQuery(query).ToList());

                foreach (var queryResult in queryResults)
                    yield return new DictionaryStorageEntity<T>(queryResult.RowKey, partitionId, queryResult.Data);
            }
        }

        public DictionaryStorageEntity<T> LoadDictionaryStorageEntity(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            var table = CloudTableClient.GetTableReference(TableName);

            if (transientFaultStrategy.Try(() => table.Exists()) == false)
                return null;

            var op = TableOperation.Retrieve<AzureTableDictionaryStorageEntity<T>>(partitionId, entityId);

            var storageEntity = transientFaultStrategy.Try(
                () => table.Execute(op).Result as AzureTableDictionaryStorageEntity<T>);

            if (storageEntity?.Data == null)
                return null;

            return new DictionaryStorageEntity<T>(entityId, partitionId, storageEntity.Data);
        }

        private CloudStorageAccount GetCloudStorageAccount()
        {
            return cloudStorageAccount = cloudStorageAccount ??
                                         CloudStorageAccount.Parse(StorageConnectionString);
        }

        private CloudTableClient GetCloudTableClient()
        {
            return cloudTableClient = cloudTableClient ??
                                      CloudStorageAccount.CreateCloudTableClient();
        }
    }
}