using System;
using System.Collections.Generic;
using System.Linq;
using Mantle.Configuration.Attributes;
using Mantle.DictionaryStorage.Azure.Entities;
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
            typeMetadata = new TypeMetadata(typeof (T));
        }

        public CloudStorageAccount CloudStorageAccount
        {
            get { return GetCloudStorageAccount(); }
        }

        public CloudTableClient CloudTableClient
        {
            get { return GetCloudTableClient(); }
        }

        [Configurable(IsRequired = true)]
        public string DictionaryName { get; set; }

        [Configurable(IsRequired = true)]
        public string StorageConnectionString { get; set; }

        public void DeleteEntity(string entityId, string partitionId)
        {
            partitionId.Require("entityId");
            partitionId.Require("partitionId");

            CloudTable table = CloudTableClient.GetTableReference(DictionaryName);

            if (table.Exists() == false)
                throw new InvalidOperationException(String.Format("Dictionary [{0}] does not exist.", DictionaryName));

            TableOperation retrieveOp = TableOperation.Retrieve<AzureTableDictionaryStorageEntity<T>>(partitionId,
                                                                                                      entityId);

            TableResult retrieveResult = table.Execute(retrieveOp);

            if (retrieveResult.Result == null)
                throw new InvalidOperationException(
                    String.Format("Entity [{0}/{1}] was not found in dictionary [{2}].", partitionId,
                                  entityId, DictionaryName));

            TableOperation deleteOp =
                TableOperation.Delete(retrieveResult.Result as AzureTableDictionaryStorageEntity<T>);

            table.Execute(deleteOp);
        }

        public bool EntityExists(string entityId, string partitionId)
        {
            entityId.Require("entityId");
            partitionId.Require("partitionId");

            CloudTable table = CloudTableClient.GetTableReference(DictionaryName);

            if (table.Exists() == false)
                return false;

            TableOperation op = TableOperation.Retrieve<AzureTableDictionaryStorageEntity<T>>(partitionId, entityId);

            return (table.Execute(op).Result != null);
        }

        public void InsertOrUpdateEntities(IEnumerable<T> entities, Func<T, string> entityIdSelector,
                                           Func<T, string> partitionIdSelector)
        {
            entities.Require("entities");
            entityIdSelector.Require("entityIdSelector");
            partitionIdSelector.Require("partitionIdSelector");

            CloudTable table = CloudTableClient.GetTableReference(DictionaryName);

            table.CreateIfNotExists();

            IEnumerable<IGrouping<string, AzureTableDictionaryStorageEntity<T>>> storageEntityGroups =
                entities.Select(
                                e =>
                                    new AzureTableDictionaryStorageEntity<T>(typeMetadata)
                                    {
                                        Data = e,
                                        PartitionKey = partitionIdSelector(e),
                                        RowKey = entityIdSelector(e)
                                    }).GroupBy(e => e.PartitionKey);

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

        public void InsertOrUpdateEntity(T entity, string entityId, string partitionId)
        {
            entity.Require("entity");
            entityId.Require("entityId");
            partitionId.Require("partitionId");

            var storageEntity = new AzureTableDictionaryStorageEntity<T>(typeMetadata);

            storageEntity.Data = entity;
            storageEntity.RowKey = entityId;
            storageEntity.PartitionKey = partitionId;

            CloudTable table = CloudTableClient.GetTableReference(DictionaryName);

            table.CreateIfNotExists();

            TableOperation op = TableOperation.InsertOrReplace(storageEntity);

            table.Execute(op);
        }

        public IEnumerable<T> LoadAllEntities(string partitionId)
        {
            partitionId.Require("partitionId");

            CloudTable table = CloudTableClient.GetTableReference(DictionaryName);

            if (table.Exists() == false)
                throw new InvalidOperationException(String.Format("Dictionary [{0}] does not exist.", DictionaryName));

            TableQuery<AzureTableDictionaryStorageEntity<T>> query = new TableQuery
                <AzureTableDictionaryStorageEntity<T>>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionId));

            foreach (var queryResult in table.ExecuteQuery(query))
                yield return queryResult.Data;
        }

        public T LoadEntity(string entityId, string partitionId)
        {
            entityId.Require("entityId");
            partitionId.Require("partitionId");

            CloudTable table = CloudTableClient.GetTableReference(DictionaryName);

            if (table.Exists() == false)
                throw new InvalidOperationException(String.Format("Dictionary [{0}] does not exist.", DictionaryName));

            TableOperation op = TableOperation.Retrieve<AzureTableDictionaryStorageEntity<T>>(partitionId, entityId);
            TableResult result = table.Execute(op);

            if (result.Result == null)
                return null;

            var storageEntity = (result.Result as AzureTableDictionaryStorageEntity<T>);

            if (storageEntity == null)
                return null;

            return storageEntity.Data;
        }

        private CloudStorageAccount GetCloudStorageAccount()
        {
            return (cloudStorageAccount = (cloudStorageAccount ??
                                           CloudStorageAccount.Parse(StorageConnectionString)));
        }

        private CloudTableClient GetCloudTableClient()
        {
            return (cloudTableClient = (cloudTableClient ??
                                        CloudStorageAccount.CreateCloudTableClient()));
        }
    }
}