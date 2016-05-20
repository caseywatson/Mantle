using System.Collections.Generic;
using System.Linq;
using Mantle.Configuration.Attributes;
using Mantle.DictionaryStorage.Azure.Entities;
using Mantle.DictionaryStorage.Entities;
using Mantle.DictionaryStorage.Interfaces;
using Mantle.Extensions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Mantle.DictionaryStorage.Azure.Clients
{
    public class AzureTableDictionaryStorageClient<T> : IDictionaryStorageClient<T>
        where T : class, new()
    {
        private readonly TypeMetadata typeMetadata;

        private CloudStorageAccount cloudStorageAccount;
        private CloudTableClient cloudTableClient;

        public AzureTableDictionaryStorageClient()
        {
            typeMetadata = new TypeMetadata(typeof(T));

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

        public void DeleteEntity(string entityId, string partitionId)
        {
            partitionId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            var table = CloudTableClient.GetTableReference(TableName);

            if (table.Exists())
            {
                var retrieveOp =
                    TableOperation.Retrieve<AzureTableDictionaryStorageEntity<T>>(partitionId, entityId);

                var retrieveResult = table.Execute(retrieveOp);

                if (retrieveResult.Result != null)
                {
                    var deleteOp =
                        TableOperation.Delete(retrieveResult.Result as AzureTableDictionaryStorageEntity<T>);

                    table.Execute(deleteOp);
                }
            }
        }

        public void DeletePartition(string partitionId)
        {
            partitionId.Require(nameof(partitionId));

            var table = CloudTableClient.GetTableReference(TableName);

            if (table.Exists())
            {
                var query = new TableQuery<AzureTableDictionaryStorageEntity<T>>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionId));

                foreach (var entitiesChunk in table.ExecuteQuery(query).Chunk(100))
                {
                    var batchDeleteOp = new TableBatchOperation();

                    foreach (var entity in entitiesChunk)
                        batchDeleteOp.Delete(entity);

                    table.ExecuteBatch(batchDeleteOp);
                }
            }
        }

        public bool DoesEntityExist(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            var table = CloudTableClient.GetTableReference(TableName);

            if (table.Exists() == false)
                return false;

            var op = TableOperation.Retrieve<AzureTableDictionaryStorageEntity<T>>(partitionId, entityId);

            return table.Execute(op).Result != null;
        }

        public void InsertOrUpdateDictionaryStorageEntities(IEnumerable<DictionaryStorageEntity<T>> dsEntities)
        {
            dsEntities.Require(nameof(dsEntities));

            var table = CloudTableClient.GetTableReference(TableName);

            if (AutoSetup)
                table.CreateIfNotExists();

            var storageEntityGroups = dsEntities
                .Select(e => new AzureTableDictionaryStorageEntity<T>(typeMetadata)
                {
                    Data = e.Entity,
                    PartitionKey = e.PartitionId,
                    RowKey = e.EntityId
                })
                .GroupBy(e => e.PartitionKey);

            foreach (var storageEntityGroup in storageEntityGroups)
            {
                foreach (var chunk in storageEntityGroup.Chunk(100))
                {
                    var batchOp = new TableBatchOperation();

                    foreach (var storageEntity in chunk)
                        batchOp.InsertOrReplace(storageEntity);

                    table.ExecuteBatch(batchOp);
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
                table.CreateIfNotExists();

            var op = TableOperation.InsertOrReplace(storageEntity);

            table.Execute(op);
        }

        public IEnumerable<DictionaryStorageEntity<T>> LoadAllDictionaryStorageEntities(string partitionId)
        {
            partitionId.Require(nameof(partitionId));

            var table = CloudTableClient.GetTableReference(TableName);

            if (table.Exists())
            {
                var query = new TableQuery<AzureTableDictionaryStorageEntity<T>>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionId));

                foreach (var queryResult in table.ExecuteQuery(query))
                    yield return new DictionaryStorageEntity<T>(queryResult.RowKey, partitionId, queryResult.Data);
            }
        }

        public DictionaryStorageEntity<T> LoadDictionaryStorageEntity(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            var table = CloudTableClient.GetTableReference(TableName);

            if (table.Exists() == false)
                return null;

            var op = TableOperation.Retrieve<AzureTableDictionaryStorageEntity<T>>(partitionId, entityId);
            var storageEntity = table.Execute(op).Result as AzureTableDictionaryStorageEntity<T>;

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