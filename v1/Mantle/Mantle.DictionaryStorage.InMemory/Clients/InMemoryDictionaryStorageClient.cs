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
            entityId.Require("entityId");
            partitionId.Require("partitionId");

            try
            {
                dictionaryLock.EnterWriteLock();

                if (dictionary.ContainsKey(partitionId) == false)
                    throw new InvalidOperationException(String.Format("Partition [{0}] does not exist.", partitionId));

                if (dictionary[partitionId].ContainsKey(entityId) == false)
                    throw new InvalidOperationException(String.Format("Entity [{0}/{1}] does not exist.", partitionId,
                                                                      entityId));

                dictionary[partitionId].Remove(entityId);
            }
            finally
            {
                dictionaryLock.ExitWriteLock();
            }
        }

        public bool EntityExists(string entityId, string partitionId)
        {
            entityId.Require("entityId");
            partitionId.Require("partitionId");

            try
            {
                dictionaryLock.EnterReadLock();

                if (dictionary.ContainsKey(partitionId) == false)
                    return false;

                if (dictionary[partitionId].ContainsKey(entityId) == false)
                    return false;

                return true;
            }
            finally
            {
                dictionaryLock.ExitReadLock();
            }
        }

        public void InsertOrUpdateEntities(IEnumerable<T> entities, Func<T, string> entityIdSelector,
                                           Func<T, string> partitionIdSelector)
        {
            entities.Require("entities");
            entityIdSelector.Require("entityIdSelector");
            partitionIdSelector.Require("partitionIdSelector");

            try
            {
                dictionaryLock.EnterWriteLock();

                foreach (T entity in entities)
                {
                    string entityId = entityIdSelector(entity);
                    string partitionId = partitionIdSelector(entity);

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
            entity.Require("entity");
            entityId.Require("entityId");
            partitionId.Require("partitionId");

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
            partitionId.Require("partitionId");

            try
            {
                if (dictionary.ContainsKey(partitionId) == false)
                    throw new InvalidOperationException(String.Format("Partition [{0}] does not exist.", partitionId));

                return (dictionary[partitionId].Values);
            }
            finally
            {
                dictionaryLock.ExitReadLock();
            }
        }

        public T LoadEntity(string entityId, string partitionId)
        {
            entityId.Require("entityId");
            partitionId.Require("partitionId");

            try
            {
                if (dictionary.ContainsKey(partitionId) == false)
                    throw new InvalidOperationException(String.Format("Partition [{0}] does not exist.", partitionId));

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