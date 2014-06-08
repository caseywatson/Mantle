using System;
using System.IO;
using System.Runtime.Serialization;

namespace Mantle.Storage.Dictionary.Azure
{
    public static class DictionaryEntityExtensions
    {
        public static T FromAzureDictionaryEntity<T>(this AzureDictionaryEntity dictionaryEntity)
        {
            var serializer = new DataContractSerializer(typeof (T));
            return ((T) (serializer.ReadObject(new MemoryStream(dictionaryEntity.Data))));
        }

        public static AzureDictionaryEntity ToAzureDictionaryEntity<T>(this T dictionaryEntity)
            where T : DictionaryEntity
        {
            var azEntity = new AzureDictionaryEntity();

            if (String.IsNullOrEmpty(dictionaryEntity.DictionaryId))
                azEntity.PartitionKey = Guid.NewGuid().ToString();
            else
                azEntity.PartitionKey = dictionaryEntity.DictionaryId;

            azEntity.RowKey = dictionaryEntity.EntityId;

            using (var entityStream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof (T));
                serializer.WriteObject(entityStream, dictionaryEntity);
                entityStream.Position = 0;
                azEntity.Data = entityStream.ToArray();
            }

            return azEntity;
        }
    }
}