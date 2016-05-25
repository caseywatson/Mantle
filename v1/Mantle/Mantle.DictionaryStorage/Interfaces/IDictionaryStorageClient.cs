using System.Collections.Generic;
using Mantle.DictionaryStorage.Entities;

namespace Mantle.DictionaryStorage.Interfaces
{
    public interface IDictionaryStorageClient<T> : IReadOnlyDictionaryStorageClient<T>
        where T : class, new()
    {
        bool DeleteEntity(string entityId, string partitionId);
        bool DeletePartition(string partitionId);
        void InsertOrUpdateDictionaryStorageEntities(IEnumerable<DictionaryStorageEntity<T>> entities);
        void InsertOrUpdateDictionaryStorageEntity(DictionaryStorageEntity<T> entity);
    }
}