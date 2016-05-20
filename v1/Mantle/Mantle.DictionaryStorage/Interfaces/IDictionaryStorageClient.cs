using System.Collections.Generic;
using Mantle.DictionaryStorage.Entities;

namespace Mantle.DictionaryStorage.Interfaces
{
    public interface IDictionaryStorageClient<T> : IReadOnlyDictionaryStorageClient<T>
        where T : class, new()
    {
        void DeleteEntity(string entityId, string partitionId);
        void DeletePartition(string partitionId);
        void InsertOrUpdateDictionaryStorageEntities(IEnumerable<DictionaryStorageEntity<T>> entities);
        void InsertOrUpdateDictionaryStorageEntity(DictionaryStorageEntity<T> entity);
    }
}