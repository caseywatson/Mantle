using System.Collections.Generic;
using Mantle.DictionaryStorage.Entities;

namespace Mantle.DictionaryStorage.Interfaces
{
    public interface IReadOnlyDictionaryStorageClient<T>
        where T : class, new()
    {
        bool DoesEntityExist(string entityId, string partitionId);
        IEnumerable<DictionaryStorageEntity<T>> LoadAllDictionaryStorageEntities(string partitionId);
        DictionaryStorageEntity<T> LoadDictionaryStorageEntity(string entityId, string partitionId);
    }
}