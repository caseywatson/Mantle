using System.Collections.Generic;

namespace Mantle.DictionaryStorage.Interfaces
{
    public interface IDictionaryStorageClient<T>
        where T : class, new()
    {
        void Delete(string entityId, string partitionId, string dictionaryName = null);
        bool Exists(string entityId, string partitionId, string dictionaryName = null);
        void InsertOrUpdate(T entity, string entityId, string partitionId, string dictionaryName = null);
        T Load(string entityId, string partitionId, string dictionaryName = null);
        IEnumerable<T> LoadAll(string parititionId, string dictionaryName = null);
    }
}