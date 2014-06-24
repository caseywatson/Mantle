using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mantle.DictionaryStorage.Interfaces;
using Mantle.Extensions;

namespace Mantle.DictionaryStorage.InMemory.Clients
{
    public class InMemoryDictionaryStorageClient<T> : IDictionaryStorageClient<T>
        where T : class, new()
    {
        private readonly Dictionary<string, Dictionary<string, T>> dictionary; 
        private readonly ReaderWriterLockSlim dictionaryLock;

        public InMemoryDictionaryStorageClient()
        {
            dictionaryLock = new ReaderWriterLockSlim();
        }

        public void DeleteEntity(string entityId, string partitionId)
        {
            entityId.Require("entityId");
            partitionId.Require("partitionId");
        }

        public bool EntityExists(string entityId, string partitionId)
        {
            throw new NotImplementedException();
        }

        public void InsertOrUpdateEntity(T entity, string entityId, string partitionId)
        {
            throw new NotImplementedException();
        }

        public void InsertOrUpdateEntities(IEnumerable<T> entities, Func<T, string> entityIdSelector, Func<T, string> partitionIdSelector)
        {
            throw new NotImplementedException();
        }

        public T LoadEntity(string entityId, string partitionId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> LoadAllEntities(string parititionId)
        {
            throw new NotImplementedException();
        }
    }
}
