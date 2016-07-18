using System;
using System.Collections.Generic;
using System.Linq;
using Mantle.Configuration.Attributes;
using Mantle.DictionaryStorage.Entities;
using Mantle.DictionaryStorage.Interfaces;
using Mantle.Extensions;
using Mantle.FaultTolerance.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Mantle.DictionaryStorage.Redis.Clients
{
    public class RedisDictionaryStorageClient<T> : IDictionaryStorageClient<T>
        where T : class, new()
    {
        private readonly ITransientFaultStrategy transientFaultStrategy;

        private IConnectionMultiplexer connectionMultiplexer;

        public RedisDictionaryStorageClient(ITransientFaultStrategy transientFaultStrategy)
        {
            this.transientFaultStrategy = transientFaultStrategy;

            RedisDatabaseId = -1;
        }

        [Configurable(IsRequired = true)]
        public string RedisConnectionString { get; set; }

        [Configurable]
        public int RedisDatabaseId { get; set; }

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

        public bool DeleteEntity(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            return transientFaultStrategy.Try(() => Database.HashDelete(partitionId, entityId));
        }

        public bool DeletePartition(string partitionId)
        {
            partitionId.Require(nameof(partitionId));

            return transientFaultStrategy.Try(() => Database.KeyDelete(partitionId));
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

                transientFaultStrategy.Try(() => Database.HashSet(dsEntityGroup.Key, hashEntries));

                if (ExpirationTimeSpan != null)
                    ResetPartitionExpiration(dsEntityGroup.Key);
            }
        }

        public void InsertOrUpdateDictionaryStorageEntity(DictionaryStorageEntity<T> entity)
        {
            entity.Require(nameof(entity));

            transientFaultStrategy.Try(() => Database.HashSet(entity.PartitionId, entity.EntityId,
                                                              JsonConvert.SerializeObject(entity.Entity)));

            if (ExpirationTimeSpan != null)
                ResetPartitionExpiration(entity.PartitionId);
        }

        public bool DoesEntityExist(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            return transientFaultStrategy.Try(() => Database.HashExists(partitionId, entityId));
        }

        public IEnumerable<DictionaryStorageEntity<T>> LoadAllDictionaryStorageEntities(string partitionId)
        {
            partitionId.Require(nameof(partitionId));

            if ((ExpirationTimeSpan != null) && UseSlidingExpiration)
                ResetPartitionExpiration(partitionId);

            var hashEntries = transientFaultStrategy.Try(() => Database.HashGetAll(partitionId).ToList());

            foreach (var hashEntry in hashEntries)
            {
                yield return new DictionaryStorageEntity<T>(hashEntry.Name, partitionId,
                                                            JsonConvert.DeserializeObject<T>(hashEntry.Value));
            }
        }

        public DictionaryStorageEntity<T> LoadDictionaryStorageEntity(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            var value = (string) (transientFaultStrategy.Try(() => Database.HashGet(partitionId, entityId)));

            if (value == null)
                return null;

            if ((ExpirationTimeSpan != null) && UseSlidingExpiration)
                ResetPartitionExpiration(partitionId);

            return new DictionaryStorageEntity<T>(entityId, partitionId,
                                                  JsonConvert.DeserializeObject<T>(value));
        }

        private void ResetPartitionExpiration(string partitionId)
        {
            transientFaultStrategy.Try(
                () => Database.KeyExpire(partitionId, ExpirationTimeSpan.Value, CommandFlags.FireAndForget));
        }

        private IConnectionMultiplexer GetConnectionMultiplexer()
        {
            return connectionMultiplexer = connectionMultiplexer ??
                                           transientFaultStrategy.Try(
                                               () => StackExchange.Redis.ConnectionMultiplexer.Connect(
                                                   RedisConnectionString));
        }

        private IDatabase GetDatabase()
        {
            return transientFaultStrategy.Try(() => GetConnectionMultiplexer().GetDatabase(RedisDatabaseId));
        }
    }
}