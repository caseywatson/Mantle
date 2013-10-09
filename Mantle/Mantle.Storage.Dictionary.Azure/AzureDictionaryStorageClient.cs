using System;
using System.Collections.Generic;
using System.Linq;
using Mantle.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Mantle.Storage.Dictionary.Azure
{
    public class AzureDictionaryStorageClient : IDictionaryStorageClient
    {
        private readonly CloudTableClient cloudTableClient;

        public AzureDictionaryStorageClient(IAzureStorageConfiguration storageConfiguration)
        {
            if (storageConfiguration == null)
                throw new ArgumentNullException("storageConfiguration");

            storageConfiguration.Validate();

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConfiguration.ConnectionString);

            cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
        }

        public IEnumerable<T> LoadEntities<T>(string dictionaryId = null)
        {
            CloudTable table = GetCloudTable<T>();
            string partitionKey = (dictionaryId ?? typeof (T).ToString());
            TableQuery<AzureDictionaryEntity> tableQuery =
                new TableQuery<AzureDictionaryEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey",
                    QueryComparisons.Equal, partitionKey));

            return table.ExecuteQuery(tableQuery).Select(r => r.FromAzureDictionaryEntity<T>());
        }

        public T LoadEntity<T>(string entityId, string dictionaryId)
        {
            if (String.IsNullOrEmpty(entityId))
                throw new ArgumentException("Entity ID is required.", "entityId");

            if (String.IsNullOrEmpty(dictionaryId))
                throw new ArgumentException("Dictionary ID is required.", "dictionaryId");

            CloudTable table = GetCloudTable<T>();
            AzureDictionaryEntity tableEntity = LoadTableEntity(entityId, dictionaryId, table);

            if (tableEntity == null)
                return default(T);

            return tableEntity.FromAzureDictionaryEntity<T>();
        }

        public T LoadEntity<T>(string entityId)
        {
            return LoadEntity<T>(entityId, typeof (T).Name);
        }

        public void Insert<T>(T entity) where T : DictionaryEntity
        {
            CloudTable table = GetCloudTable<T>();
            AzureDictionaryEntity tableEntity = entity.ToAzureDictionaryEntity();
            TableOperation insertOperation = TableOperation.Insert(tableEntity);

            table.Execute(insertOperation);
        }

        public void Insert<T>(T entity, string entityId)
        {
            if (String.IsNullOrEmpty(entityId))
                throw new ArgumentException("Entity ID is required.", "entityId");

            CloudTable table = GetCloudTable<T>();
            AzureDictionaryEntity tableEntity = entity.ToAzureDictionaryEntity(entityId);
            TableOperation insertOperation = TableOperation.Insert(tableEntity);

            table.Execute(insertOperation);
        }

        public void Insert<T>(T entity, string entityId, string dictionaryId)
        {
            if (String.IsNullOrEmpty(entityId))
                throw new ArgumentException("Entity ID is required.", "entityId");

            if (String.IsNullOrEmpty(dictionaryId))
                throw new ArgumentException("Dictionary ID is required.", "dictionaryId");

            CloudTable table = GetCloudTable<T>();
            AzureDictionaryEntity tableEntity = entity.ToAzureDictionaryEntity(entityId, dictionaryId);
            TableOperation insertOperation = TableOperation.Insert(tableEntity);

            table.Execute(insertOperation);
        }

        public void Update<T>(T entity) where T : DictionaryEntity
        {
            CloudTable table = GetCloudTable<T>();
            AzureDictionaryEntity tableEntity = LoadTableEntity(entity.EntityId, entity.DictionaryId, table);

            if (tableEntity == null)
                throw new ArgumentException(
                    "Unable to update entity. The specified entity was not found in dictionary storage.", "entity");

            tableEntity.Data = entity.SerializeToBytes();

            TableOperation updateOperation = TableOperation.Replace(tableEntity);

            table.Execute(updateOperation);
        }

        public void Update<T>(T entity, string entityId)
        {
            if (String.IsNullOrEmpty(entityId))
                throw new ArgumentException("Entity ID is required.", "entityId");

            CloudTable table = GetCloudTable<T>();
            AzureDictionaryEntity tableEntity = LoadTableEntity(entityId, typeof (T).Name, table);

            if (tableEntity == null)
                throw new ArgumentException(
                    "Unable to update entity. The specified entity was not found in dictionary storage.", "entity");

            tableEntity.Data = entity.SerializeToBytes();

            TableOperation updateOperation = TableOperation.Replace(tableEntity);

            table.Execute(updateOperation);
        }

        public void Update<T>(T entity, string entityId, string dictionaryId)
        {
            if (String.IsNullOrEmpty(entityId))
                throw new ArgumentException("Entity ID is required.", "entityId");

            if (String.IsNullOrEmpty(dictionaryId))
                throw new ArgumentException("Dictionary ID is required.", "dictionaryId");

            CloudTable table = GetCloudTable<T>();
            AzureDictionaryEntity tableEntity = LoadTableEntity(entityId, dictionaryId, table);

            if (tableEntity == null)
                throw new ArgumentException(
                    "Unable to update entity. The specified entity was not found in dictionary storage.", "entity");

            tableEntity.Data = entity.SerializeToBytes();

            TableOperation updateOperation = TableOperation.Replace(tableEntity);

            table.Execute(updateOperation);
        }

        public void Delete<T>(T entity) where T : DictionaryEntity
        {
            CloudTable table = GetCloudTable<T>();
            AzureDictionaryEntity tableEntity = LoadTableEntity(entity.EntityId, entity.DictionaryId, table);

            if (tableEntity == null)
                throw new ArgumentException(
                    "Unable to delete entity. The specified entity was not found in dictionary storage.", "entity");

            TableOperation deleteOperation = TableOperation.Delete(tableEntity);

            table.Execute(deleteOperation);
        }

        public void Delete<T>(string entityId)
        {
            if (String.IsNullOrEmpty(entityId))
                throw new ArgumentException("Entity ID is required.", "entityId");

            CloudTable table = GetCloudTable<T>();
            AzureDictionaryEntity tableEntity = LoadTableEntity(entityId, typeof (T).Name, table);

            if (tableEntity == null)
                throw new ArgumentException(
                    "Unable to delete entity. The specified entity was not found in dictionary storage.", "entity");

            TableOperation deleteOperation = TableOperation.Delete(tableEntity);

            table.Execute(deleteOperation);
        }

        public void Delete<T>(string entityId, string dictionaryId)
        {
            if (String.IsNullOrEmpty(entityId))
                throw new ArgumentException("Entity ID is required.", "entityId");

            if (String.IsNullOrEmpty(dictionaryId))
                throw new ArgumentException("Dictionary ID is required.", "dictionaryId");

            CloudTable table = GetCloudTable<T>();
            AzureDictionaryEntity tableEntity = LoadTableEntity(entityId, dictionaryId, table);

            if (tableEntity == null)
                throw new ArgumentException(
                    "Unable to delete entity. The specified entity was not found in dictionary storage.", "entity");

            TableOperation deleteOperation = TableOperation.Delete(tableEntity);

            table.Execute(deleteOperation);
        }

        private CloudTable GetCloudTable<T>()
        {
            CloudTable table = cloudTableClient.GetTableReference(GetTableName<T>());

            table.CreateIfNotExists();

            return table;
        }

        private string GetTableName<T>()
        {
            string tableName = typeof (T).Name.ScrubForLettersOrDigitsOnly().ToLower();

            if (tableName.Length < 3)
                return tableName.PadRight(3, '0');

            if (tableName.Length > 63)
                return tableName.Substring(0, 63);

            return tableName;
        }

        private AzureDictionaryEntity LoadTableEntity(string rowKey, string partitionKey, CloudTable tableReference)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<AzureDictionaryEntity>(partitionKey, rowKey);
            TableResult retrieveResult = tableReference.Execute(retrieveOperation);

            if (retrieveResult.Result != null)
                return (retrieveResult.Result as AzureDictionaryEntity);

            return null;
        }
    }
}