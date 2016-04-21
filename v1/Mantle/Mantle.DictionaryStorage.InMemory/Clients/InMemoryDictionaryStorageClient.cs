using System;
using System.Collections.Generic;
using System.Threading;
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

                if (dictionary.ContainsKey(partitionId) == false)
                    throw new InvalidOperationException($"Partition [{partitionId}] does not exist.");

                if (dictionary[partitionId].ContainsKey(entityId) == false)
                    throw new InvalidOperationException($"Entity [{partitionId}/{entityId}] does not exist.");

                dictionary[partitionId].Remove(entityId);
            }
            finally
            {
                dictionaryLock.ExitWriteLock();
            }
        }

        public bool EntityExists(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            try
            {
                dictionaryLock.EnterReadLock();
                return (dictionary[partitionId]?.ContainsKey(entityId) == true);
            }
            finally
            {
                dictionaryLock.ExitReadLock();
            }
        }

        public void InsertOrUpdateEntities(IEnumerable<T> entities, Func<T, string> entityIdSelector,
            Func<T, string> partitionIdSelector)
        {
            entities.Require(nameof(entities));
            entityIdSelector.Require(nameof(entityIdSelector));
            partitionIdSelector.Require(nameof(partitionIdSelector));

            try
            {
                dictionaryLock.EnterWriteLock();

                foreach (var entity in entities)
                {
                    var entityId = entityIdSelector(entity);
                    var partitionId = partitionIdSelector(entity);

                    if (dictionary.ContainsKey(partitionId) == false)
                        dictionary[partitionId] = new Dictionary<string, T>();

                    dictionary[partitionId][entityId] = entity;
                }
            }
            finally
            {
                dictionaryLock.ExitWriteLock();
            }
        }

        public void InsertOrUpdateEntity(T entity, string entityId, string partitionId)
        {
            entity.Require(nameof(entity));
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            try
            {
                dictionaryLock.EnterWriteLock();

                if (dictionary.ContainsKey(partitionId) == false)
                    dictionary[partitionId] = new Dictionary<string, T>();

                dictionary[partitionId][entityId] = entity;
            }
            finally
            {
                dictionaryLock.ExitWriteLock();
            }
        }

        public IEnumerable<T> LoadAllEntities(string partitionId)
        {
            partitionId.Require(nameof(partitionId));

            try
            {
                dictionaryLock.EnterReadLock();

                if (dictionary.ContainsKey(partitionId) == false)
                    throw new InvalidOperationException($"Partition [{partitionId}] does not exist.");

                return (dictionary[partitionId].Values);
            }
            finally
            {
                dictionaryLock.ExitReadLock();
            }
        }

        public T LoadEntity(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            try
            {
                dictionaryLock.EnterReadLock();

                if (dictionary.ContainsKey(partitionId) == false)
                    throw new InvalidOperationException($"Partition [{partitionId}] does not exist.");

                if (dictionary[partitionId].ContainsKey(entityId))
                    return dictionary[partitionId][entityId];

                return null;
            }
            finally
            {
                dictionaryLock.ExitReadLock();
            }
        }
    }
}