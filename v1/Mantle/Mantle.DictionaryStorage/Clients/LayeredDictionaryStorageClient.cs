using System;
using System.Collections.Generic;
using Mantle.DictionaryStorage.Entities;
using Mantle.DictionaryStorage.Interfaces;

namespace Mantle.DictionaryStorage.Clients
{
    public abstract class LayeredDictionaryStorageClient<T> : IDictionaryStorageClient<T>, IDisposable
        where T : class, new()
    {
        public void DeleteEntity(string entityId, string partitionId)
        {
            throw new NotImplementedException();
        }

        public bool DoesEntityExist(string entityId, string partitionId)
        {
            throw new NotImplementedException();
        }

        public void InsertOrUpdateDictionaryStorageEntities(IEnumerable<DictionaryStorageEntity<T>> entities)
        {
            throw new NotImplementedException();
        }

        public void InsertOrUpdateDictionaryStorageEntity(DictionaryStorageEntity<T> entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DictionaryStorageEntity<T>> LoadAllDictionaryStorageEntities(string partitionId)
        {
            throw new NotImplementedException();
        }

        public DictionaryStorageEntity<T> LoadDictionaryStorageEntity(string entityId, string partitionId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}