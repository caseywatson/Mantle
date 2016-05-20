using System;
using System.Collections.Generic;
using System.Linq;
using Mantle.DictionaryStorage.Entities;
using Mantle.DictionaryStorage.Interfaces;
using Mantle.Extensions;

namespace Mantle.DictionaryStorage.Extensions
{
    public static class DictionaryStorageClientExtensions
    {
        public static void InsertOrUpdateEntities<T>(this IDictionaryStorageClient<T> dictionaryStorageClient,
                                                     IEnumerable<T> entities,
                                                     Func<T, string> entityIdSelector,
                                                     Func<T, string> partitionIdSelector)
            where T : class, new()
        {
            dictionaryStorageClient.Require(nameof(dictionaryStorageClient));
            entities.Require(nameof(entities));
            entityIdSelector.Require(nameof(entityIdSelector));
            partitionIdSelector.Require(nameof(partitionIdSelector));

            dictionaryStorageClient.InsertOrUpdateDictionaryStorageEntities(entities
                                                                                .Select(
                                                                                    e =>
                                                                                        new DictionaryStorageEntity<T>(
                                                                                        entityIdSelector(e),
                                                                                        partitionIdSelector(e), e)));
        }

        public static void InsertOrUpdateEntity<T>(this IDictionaryStorageClient<T> dictionaryStorageClient,
                                                   T entity, string entityId, string partitionId)
            where T : class, new()
        {
            dictionaryStorageClient.Require(nameof(dictionaryStorageClient));
            entity.Require(nameof(entity));
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            dictionaryStorageClient.InsertOrUpdateDictionaryStorageEntity(
                new DictionaryStorageEntity<T>(entityId, partitionId, entity));
        }

        public static IEnumerable<T> LoadAllEntities<T>(this IDictionaryStorageClient<T> dictionaryStorageClient,
                                                        string partitionId)
            where T : class, new()
        {
            dictionaryStorageClient.Require(nameof(dictionaryStorageClient));
            partitionId.Require(nameof(partitionId));

            return dictionaryStorageClient.LoadAllDictionaryStorageEntities(partitionId)
                .Select(e => e.Entity);
        }

        public static T LoadEntity<T>(this IDictionaryStorageClient<T> dictionaryStorageClient,
                                      string entityId, string partitionId)
            where T : class, new()
        {
            dictionaryStorageClient.Require(nameof(dictionaryStorageClient));
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            return dictionaryStorageClient.LoadDictionaryStorageEntity(entityId, partitionId)?.Entity;
        }
    }
}