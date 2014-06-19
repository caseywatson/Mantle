using System;
using System.Collections.Generic;
using Mantle.Configuration.Attributes;
using Mantle.DictionaryStorage.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Mantle.DictionaryStorage.Azure.Clients
{
    public class AzureTableDictionaryStorageClient<T> : IDictionaryStorageClient<T>
        where T : class, new()
    {
        [Configurable]
        public string StorageConnectionString { get; set; }

        public CloudStorageAccount CloudStorageAccount { get; private set; }
        public CloudTableClient CloudTableClient { get; private set; }

        public void Delete(string entityId, string partitionId, string dictionaryName = null)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string entityId, string partitionId, string dictionaryName = null)
        {
            throw new NotImplementedException();
        }

        public void InsertOrUpdate(T entity, string entityId, string partitionId, string dictionaryName = null)
        {
            throw new NotImplementedException();
        }

        public T Load(string entityId, string partitionId, string dictionaryName = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> LoadAll(string parititionId, string dictionaryName = null)
        {
            throw new NotImplementedException();
        }
    }
}