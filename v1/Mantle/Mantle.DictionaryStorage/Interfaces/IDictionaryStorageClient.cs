using System;
using System.Collections.Generic;
using Mantle.DictionaryStorage.Entities;

namespace Mantle.DictionaryStorage.Interfaces
{
    public interface IDictionaryStorageClient<T> : IReadOnlyDictionaryStorageClient<T>
        where T : class, new()
    {
        void DeleteEntity(string entityId, string partitionId);
        void InsertOrUpdateEntities(IEnumerable<DictionaryStorageEntity<T>> entities);
        void InsertOrUpdateEntity(DictionaryStorageEntity<T> entity);
    }
}