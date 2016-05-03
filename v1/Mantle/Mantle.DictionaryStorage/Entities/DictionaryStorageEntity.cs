using Mantle.Extensions;

namespace Mantle.DictionaryStorage.Entities
{
    public class DictionaryStorageEntity<T>
        where T : class, new()
    {
        public DictionaryStorageEntity()
        {
        }

        public DictionaryStorageEntity(string entityId, string partitionId, T entity)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));
            entity.Require(nameof(entity));

            EntityId = entityId;
            PartitionId = partitionId;
            Entity = entity;
        }

        public string EntityId { get; set; }
        public string PartitionId { get; set; }

        public T Entity { get; set; }
    }
}