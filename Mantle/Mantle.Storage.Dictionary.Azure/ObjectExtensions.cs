using System;
using System.IO;
using System.Runtime.Serialization;

namespace Mantle.Storage.Dictionary.Azure
{
    public static class ObjectExtensions
    {
        public static AzureDictionaryEntity ToAzureDictionaryEntity<T>(this T entity, string entityId,
            string dictionaryId = null)
        {
            if (String.IsNullOrEmpty(entityId))
                throw new ArgumentException("Entity ID is required.", "entityId");

            var azEntity = new AzureDictionaryEntity();

            if (String.IsNullOrEmpty(dictionaryId))
                azEntity.PartitionKey = typeof (T).Name;
            else
                azEntity.PartitionKey = dictionaryId;

            azEntity.RowKey = entityId;

            using (var entityStream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof (T));
                serializer.WriteObject(entityStream, entity);
                entityStream.Position = 0;
                azEntity.Data = entityStream.ToArray();
            }

            return azEntity;
        }
    }
}