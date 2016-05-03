using System;
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
                return dictionary[partitionId]?.ContainsKey(entityId) == true;
            }
            finally
            {
                dictionaryLock.ExitReadLock();
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

                return dictionary[partitionId].Values;
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

        public void InsertOrUpdateEntities(IEnumerable<DictionaryStorageEntity<T>> dsEntities)
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

        public void InsertOrUpdateEntity(DictionaryStorageEntity<T> dsEntity)
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