using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Mantle.Storage.Dictionary.InMemory
{
    public class InMemoryDictionaryStorageClient : IDictionaryStorageClient
    {
        private readonly Dictionary<string, Dictionary<string, object>> dictionary;
        private readonly ReaderWriterLockSlim dictionaryLock;

        public InMemoryDictionaryStorageClient()
        {
            dictionaryLock = new ReaderWriterLockSlim();
            dictionary = new Dictionary<string, Dictionary<string, object>>();
        }

        public IEnumerable<T> LoadEntities<T>(string dictionaryId)
        {
            if (String.IsNullOrEmpty(dictionaryId))
                throw new ArgumentException("Dictionary ID is required.", "dictionaryId");

            try
            {
                dictionaryLock.EnterReadLock();

                if (dictionary.ContainsKey(dictionaryId))
                {
                    foreach (T dictionaryValue in dictionary[dictionaryId].Values.OfType<T>())
                        yield return dictionaryValue;
                }
            }
            finally
            {
                dictionaryLock.ExitReadLock();
            }
        }

        public T LoadEntity<T>(string entityId, string dictionaryId)
        {
            if (String.IsNullOrEmpty(entityId))
                throw new ArgumentException("Entity ID is required.", "entityId");

            if (String.IsNullOrEmpty(dictionaryId))
                throw new ArgumentException("Dictionary ID is required.", "dictionaryId");

            try
            {
                dictionaryLock.EnterReadLock();

                if (dictionary.ContainsKey(dictionaryId))
                {
                    if (dictionary[dictionaryId].ContainsKey(entityId))
                        return ((T) (dictionary[dictionaryId][entityId]));
                }

                return default(T);
            }
            finally
            {
                dictionaryLock.ExitReadLock();
            }
        }

        public T LoadEntity<T>(string entityId)
        {
            return LoadEntity<T>(entityId, typeof (T).Name);
        }

        public void Insert<T>(T entity) where T : DictionaryEntity
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            try
            {
                dictionaryLock.EnterWriteLock();

                if (dictionary.ContainsKey(entity.DictionaryId) == false)
                    dictionary.Add(entity.DictionaryId, new Dictionary<string, object>());

                if (dictionary[entity.DictionaryId].ContainsKey(entity.EntityId))
                    throw new InvalidOperationException(
                        String.Format("Unable to insert entity. Entity [{0}/{1}] already exists.", entity.DictionaryId,
                            entity.EntityId));

                dictionary[entity.DictionaryId].Add(entity.EntityId, entity);
            }
            finally
            {
                dictionaryLock.ExitWriteLock();
            }
        }

        public void Insert<T>(T entity, string entityId)
        {
            Insert(entity, entityId, typeof (T).Name);
        }

        public void Insert<T>(T entity, string entityId, string dictionaryId)
        {
            if (String.IsNullOrEmpty(entityId))
                throw new ArgumentException("Entity ID is required.", "entityId");

            if (String.IsNullOrEmpty(dictionaryId))
                throw new ArgumentException("Dictionary ID is required.", "dictionaryId");

            try
            {
                dictionaryLock.EnterWriteLock();

                if (dictionary.ContainsKey(dictionaryId) == false)
                    dictionary.Add(dictionaryId, new Dictionary<string, object>());

                if (dictionary[dictionaryId].ContainsKey(entityId))
                    throw new InvalidOperationException(
                        String.Format("Unable to insert entity. Entity [{0}/{1}] already exists.", dictionaryId,
                            entityId));

                dictionary[dictionaryId].Add(entityId, entity);
            }
            finally
            {
                dictionaryLock.ExitWriteLock();
            }
        }

        public void Update<T>(T entity) where T : DictionaryEntity
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            try
            {
                dictionaryLock.EnterWriteLock();

                if (dictionary.ContainsKey(entity.DictionaryId) == false)
                    dictionary.Add(entity.DictionaryId, new Dictionary<string, object>());

                dictionary[entity.DictionaryId][entity.EntityId] = entity;
            }
            finally
            {
                dictionaryLock.ExitWriteLock();
            }
        }

        public void Update<T>(T entity, string entityId)
        {
            Update(entity, entityId, typeof (T).Name);
        }

        public void Update<T>(T entity, string entityId, string dictionaryId)
        {
            if (String.IsNullOrEmpty(entityId))
                throw new ArgumentException("Entity ID is required.", "entityId");

            if (String.IsNullOrEmpty(dictionaryId))
                throw new ArgumentException("Dictionary ID is required.", "dictionaryId");

            try
            {
                dictionaryLock.EnterWriteLock();

                if (dictionary.ContainsKey(dictionaryId) == false)
                    dictionary.Add(dictionaryId, new Dictionary<string, object>());

                dictionary[dictionaryId][entityId] = entity;
            }
            finally
            {
                dictionaryLock.ExitWriteLock();
            }
        }

        public void Delete<T>(T entity) where T : DictionaryEntity
        {
            try
            {
                dictionaryLock.EnterWriteLock();

                if (dictionary.ContainsKey(entity.DictionaryId) &&
                    dictionary[entity.DictionaryId].ContainsKey(entity.EntityId))
                {
                    dictionary[entity.DictionaryId].Remove(entity.EntityId);

                    if (dictionary[entity.DictionaryId].Count == 0)
                        dictionary.Remove(entity.DictionaryId);
                }
                else
                {
                    throw new InvalidOperationException(
                        String.Format("Unable to delete entity. Entity [{0}/{1}] was not found.", entity.DictionaryId,
                            entity.EntityId));
                }
            }
            finally
            {
                dictionaryLock.ExitWriteLock();
            }
        }

        public void Delete<T>(string entityId)
        {
            Delete<T>(entityId, typeof (T).Name);
        }

        public void Delete<T>(string entityId, string dictionaryId)
        {
            if (String.IsNullOrEmpty(entityId))
                throw new ArgumentException("Entity ID is required.", "entityId");

            if (String.IsNullOrEmpty(dictionaryId))
                throw new ArgumentException("Dictionary ID is required.", "dictionaryId");

            try
            {
                dictionaryLock.EnterWriteLock();

                if (dictionary.ContainsKey(dictionaryId) && dictionary[dictionaryId].ContainsKey(entityId))
                {
                    dictionary[dictionaryId].Remove(entityId);

                    if (dictionary[dictionaryId].Count == 0)
                        dictionary.Remove(dictionaryId);
                }
                else
                {
                    throw new InvalidOperationException(
                        String.Format("Unable to delete entity. Entity [{0}/{1}] was not found.", dictionaryId, entityId));
                }
            }
            finally
            {
                dictionaryLock.ExitWriteLock();
            }
        }
    }
}