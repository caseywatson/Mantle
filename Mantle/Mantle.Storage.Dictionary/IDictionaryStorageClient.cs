using System.Collections.Generic;

namespace Mantle.Storage.Dictionary
{
    public interface IDictionaryStorageClient
    {
        IEnumerable<T> LoadEntities<T>(string dictionaryId);

        T LoadEntity<T>(string entityId, string dictionaryId);
        T LoadEntity<T>(string entityId);

        void Insert<T>(T entity) where T : DictionaryEntity;
        void Insert<T>(T entity, string entityId);
        void Insert<T>(T entity, string entityId, string dictionaryId);

        void Update<T>(T entity) where T : DictionaryEntity;
        void Update<T>(T entity, string entityId);
        void Update<T>(T entity, string entityId, string dictionaryId);

        void Delete<T>(T entity) where T : DictionaryEntity;
        void Delete<T>(string entityId);
        void Delete<T>(string entityId, string dictionaryId);
    }
}