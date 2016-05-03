using System.Collections.Generic;

namespace Mantle.DictionaryStorage.Interfaces
{
    public interface IReadOnlyDictionaryStorageClient<T>
        where T : class, new()
    {
        bool EntityExists(string entityId, string partitionId);
        IEnumerable<T> LoadAllEntities(string parititionId);
        T LoadEntity(string entityId, string partitionId);
    }
}