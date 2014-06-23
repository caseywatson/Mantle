using System;
using System.Collections.Generic;

namespace Mantle.DictionaryStorage.Interfaces
{
    public interface IDictionaryStorageClient<T>
        where T : class, new()
    {
        void DeleteEntity(string entityId, string partitionId);
        bool EntityExists(string entityId, string partitionId);
        void InsertOrUpdateEntity(T entity, string entityId, string partitionId);

        void InsertOrUpdateEntities(IEnumerable<T> entities, Func<T, string> entityIdSelector,
                                    Func<T, string> partitionIdSelector);

        T LoadEntity(string entityId, string partitionId);
        IEnumerable<T> LoadAllEntities(string parititionId);
    }
}