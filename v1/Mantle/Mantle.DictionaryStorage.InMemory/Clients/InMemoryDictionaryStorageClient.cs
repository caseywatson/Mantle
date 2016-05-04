using System.Collections.Generic;
using System.Threading;
using Mantle.DictionaryStorage.Entities;
using Mantle.DictionaryStorage.Interfaces;
using Mantle.Extensions;

namespace Mantle.DictionaryStorage.InMemory.Clients
{
    public class InMemoryDictionaryStorageClient<T> : IDictionaryStorageClient<T>
        where T : class, new()
    {
        private readonly Dictionary<string, Dictionary<string, T>> dictionary;
        private readonly ReaderWriterLockSlim dictionaryLock;

        public InMemoryDictionaryStorageClient()
        {
            dictionary = new Dictionary<string, Dictionary<string, T>>();
            dictionaryLock = new ReaderWriterLockSlim();
        }

        public void DeleteEntity(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            try
            {
                dictionaryLock.EnterWriteLock();

                if (dictionary.ContainsKey(partitionId) && dictionary[partitionId].ContainsKey(entityId))
                    dictionary[partitionId].Remove(entityId);
            }
            finally
            {
                dictionaryLock.ExitWriteLock();
            }
        }

        public bool DoesEntityExist(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            try
            {
                dictionaryLock.EnterReadLock();
                return (dictionary.ContainsKey(partitionId) && dictionary[partitionId].ContainsKey(entityId));
            }
            finally
            {
                dictionaryLock.ExitReadLock();
            }
        }

        public IEnumerable<DictionaryStorageEntity<T>> LoadAllDictionaryStorageEntities(string partitionId)
        {
            partitionId.Require(nameof(partitionId));

            try
            {
                dictionaryLock.EnterReadLock();

                if (dictionary.ContainsKey(partitionId))
                {
                    var partitionDictionary = dictionary[partitionId];

                    foreach (var key in partitionDictionary.Keys)
                    {
                        yield return new DictionaryStorageEntity<T>(key, partitionId,
                                                                    partitionDictionary[key]);
                    }
                }
            }
            finally
            {
                dictionaryLock.ExitReadLock();
            }
        }

        public DictionaryStorageEntity<T> LoadDictionaryStorageEntity(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            try
            {
                dictionaryLock.EnterReadLock();

                if (dictionary.ContainsKey(partitionId) && dictionary[partitionId].ContainsKey(entityId))
                    return new DictionaryStorageEntity<T>(entityId, partitionId, dictionary[partitionId][entityId]);

                return null;
            }
            finally
            {
                dictionaryLock.ExitReadLock();
            }
        }

        public void InsertOrUpdateDictionaryStorageEntities(IEnumerable<DictionaryStorageEntity<T>> dsEntities)
        {
            dsEntities.Require(nameof(dsEntities));

            try
            {
                dictionaryLock.EnterWriteLock();

                foreach (var dsEntity in dsEntities)
                {
                    if (dictionary.ContainsKey(dsEntity.PartitionId) == false)
                        dictionary[dsEntity.PartitionId] = new Dictionary<string, T>();

                    dictionary[dsEntity.PartitionId][dsEntity.EntityId] = dsEntity.Entity;
                }
            }
            finally
            {
                dictionaryLock.ExitWriteLock();
            }
        }

        public void InsertOrUpdateDictionaryStorageEntity(DictionaryStorageEntity<T> dsEntity)
        {
            dsEntity.Require(nameof(dsEntity));

            try
            {
                dictionaryLock.EnterWriteLock();

                if (dictionary.ContainsKey(dsEntity.PartitionId) == false)
                    dictionary[dsEntity.PartitionId] = new Dictionary<string, T>();

                dictionary[dsEntity.PartitionId][dsEntity.EntityId] = dsEntity.Entity;
            }
            finally
            {
                dictionaryLock.ExitWriteLock();
            }
        }
    }
}