using System;
using System.Collections.Generic;
using System.Linq;
using Mantle.Configuration.Attributes;
using Mantle.DictionaryStorage.Entities;
using Mantle.DictionaryStorage.Interfaces;
using Mantle.Extensions;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Mantle.DictionaryStorage.Redis.Clients
{
    public class RedisDictionaryStorageClient<T> : IDictionaryStorageClient<T>
        where T : class, new()
    {
        private IConnectionMultiplexer connectionMultiplexer;

        public RedisDictionaryStorageClient()
        {
            DatabaseId = -1;
        }

        [Configurable(IsRequired = true)]
        public string ConnectionString { get; set; }

        [Configurable]
        public int DatabaseId { get; set; }

        [Configurable]
        public TimeSpan? ExpirationTimeSpan { get; set; }

        [Configurable]
        public bool UseSlidingExpiration { get; set; }

        public IConnectionMultiplexer ConnectionMultiplexer
        {
            get { return GetConnectionMultiplexer(); }
            set { connectionMultiplexer = value; }
        }

        public IDatabase Database => GetDatabase();

        public void DeleteEntity(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            Database.HashDelete(partitionId, entityId);
        }

        public void InsertOrUpdateDictionaryStorageEntities(IEnumerable<DictionaryStorageEntity<T>> dsEntities)
        {
            dsEntities.Require(nameof(dsEntities));

            var dsEntityGroups = dsEntities
                .GroupBy(e => e.PartitionId)
                .ToList();

            foreach (var dsEntityGroup in dsEntityGroups)
            {
                var hashEntries = dsEntityGroup
                    .Select(e => new HashEntry(e.EntityId, JsonConvert.SerializeObject(e.Entity)))
                    .ToArray();

                Database.HashSet(dsEntityGroup.Key, hashEntries);

                if (ExpirationTimeSpan != null)
                    ResetPartitionExpiration(dsEntityGroup.Key);
            }
        }

        public void InsertOrUpdateDictionaryStorageEntity(DictionaryStorageEntity<T> entity)
        {
            entity.Require(nameof(entity));

            Database.HashSet(entity.PartitionId, entity.EntityId,
                             JsonConvert.SerializeObject(entity.Entity), When.Always);

            if (ExpirationTimeSpan != null)
                ResetPartitionExpiration(entity.PartitionId);
        }

        public bool DoesEntityExist(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            return Database.HashExists(partitionId, entityId);
        }

        public IEnumerable<DictionaryStorageEntity<T>> LoadAllDictionaryStorageEntities(string partitionId)
        {
            partitionId.Require(nameof(partitionId));

            if ((ExpirationTimeSpan != null) && UseSlidingExpiration)
                ResetPartitionExpiration(partitionId);

            foreach (var hashEntry in Database.HashGetAll(partitionId))
            {
                yield return new DictionaryStorageEntity<T>(hashEntry.Name, partitionId,
                                                            JsonConvert.DeserializeObject<T>(hashEntry.Value));
            }
        }

        public DictionaryStorageEntity<T> LoadDictionaryStorageEntity(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            var value = (string) Database.HashGet(partitionId, entityId);

            if (value == null)
                return null;

            if ((ExpirationTimeSpan != null) && UseSlidingExpiration)
                ResetPartitionExpiration(partitionId);

            return new DictionaryStorageEntity<T>(entityId, partitionId,
                                                  JsonConvert.DeserializeObject<T>(value));
        }

        private bool DoesPartitionExist(string partitionId)
        {
            return Database.KeyExists(partitionId);
        }

        private void ResetPartitionExpiration(string partitionId)
        {
            Database.KeyExpire(partitionId, ExpirationTimeSpan.Value, CommandFlags.FireAndForget);
        }

        private IConnectionMultiplexer GetConnectionMultiplexer()
        {
            return connectionMultiplexer = connectionMultiplexer ??
                                           StackExchange.Redis.ConnectionMultiplexer.Connect(ConnectionString);
        }

        private IDatabase GetDatabase()
        {
            return GetConnectionMultiplexer().GetDatabase(DatabaseId);
        }
    }
}