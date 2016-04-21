using System;
using System.Collections.Generic;
using System.IO;
using Amazon.DynamoDBv2.Model;
using Mantle.Aws.Interfaces;
using Mantle.Configuration.Attributes;
using Mantle.DictionaryStorage.Interfaces;
using Mantle.Extensions;
using Newtonsoft.Json;

namespace Mantle.DictionaryStorage.Aws.Clients
{
    public class DynamoDbDictionaryStorageClient<T> : IDictionaryStorageClient<T>
        where T : class, new()
    {
        private readonly IAwsRegionEndpoints awsRegionEndpoints;
        private readonly Dictionary<Type, Func<AttributeValue, object>> fromDynamoDbAttributeValue;
        private readonly Dictionary<Type, Func<object, AttributeValue>> toDynamoDbAttributeValue;
        private readonly TypeMetadata typeMetadata;

        public DynamoDbDictionaryStorageClient(IAwsRegionEndpoints awsRegionEndpoints)
        {
            this.awsRegionEndpoints = awsRegionEndpoints;

            fromDynamoDbAttributeValue = GetFromDynamoDbAttributeValueConversions();
            toDynamoDbAttributeValue = GetToDynamoDbAttributeValueConversions();

            typeMetadata = new TypeMetadata<T>();
        }

        [Configurable]
        public bool AutoSetup { get; set; }

        [Configurable(IsRequired = true)]
        public string AwsAccessKeyId { get; set; }

        [Configurable(IsRequired = true)]
        public string AwsSecretAccessKey { get; set; }

        [Configurable(IsRequired = true)]
        public string AwsRegionName { get; set; }

        [Configurable(IsRequired = true)]
        public string TableName { get; set; }

        public void DeleteEntity(string entityId, string partitionId)
        {
            throw new NotImplementedException();
        }

        public bool EntityExists(string entityId, string partitionId)
        {
            throw new NotImplementedException();
        }

        public void InsertOrUpdateEntities(IEnumerable<T> entities, Func<T, string> entityIdSelector,
            Func<T, string> partitionIdSelector)
        {
            throw new NotImplementedException();
        }

        public void InsertOrUpdateEntity(T entity, string entityId, string partitionId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> LoadAllEntities(string parititionId)
        {
            throw new NotImplementedException();
        }

        public T LoadEntity(string entityId, string partitionId)
        {
            throw new NotImplementedException();
        }

        private Dictionary<string, AttributeValue> ToDynamoDbDocumentDictionary(DynamoDbDictionaryStorageEntity entity)
        {
            var docDictionary = new Dictionary<string, AttributeValue>();
            var entityDictionary = new Dictionary<string, AttributeValue>();

            docDictionary[AttributeNames.EntityId] = new AttributeValue {S = entity.EntityId};
            docDictionary[AttributeNames.PartitionId] = new AttributeValue {S = entity.PartitionId};

            foreach (var property in typeMetadata.Properties)
            {
                var propertyInfo = property.PropertyInfo;
                var propertyType = propertyInfo.PropertyType;
                var propertyValue = propertyInfo.GetValue(entity);

                if (propertyValue == null)
                {
                    entityDictionary[propertyInfo.Name] = new AttributeValue {NULL = true};
                }
                else if (toDynamoDbAttributeValue.ContainsKey(propertyType))
                {
                    entityDictionary[propertyInfo.Name] = toDynamoDbAttributeValue[propertyType](propertyValue);
                }
                else
                {
                    var serializedValue = JsonConvert.SerializeObject(propertyValue, Formatting.Indented);

                    entityDictionary[propertyInfo.Name] = new AttributeValue {S = serializedValue};
                }
            }

            docDictionary[AttributeNames.Entity] = new AttributeValue {M = entityDictionary};

            return docDictionary;
        }

        private DynamoDbDictionaryStorageEntity FromDynamoDbDocumentDictionary(
            Dictionary<string, AttributeValue> docDictionary)
        {
            var entityDictionary = GetEntityDictionary(docDictionary);
            var entity = new DynamoDbDictionaryStorageEntity(GetEntityId(docDictionary), GetPartitionId(docDictionary));

            foreach (var property in typeMetadata.Properties)
            {
                var propertyInfo = property.PropertyInfo;
                var propertyType = propertyInfo.PropertyType;

                if (entityDictionary.ContainsKey(propertyInfo.Name))
                {
                    var attributeValue = entityDictionary[propertyInfo.Name];

                    if (attributeValue.NULL == false)
                    {
                        if (fromDynamoDbAttributeValue.ContainsKey(propertyType))
                        {
                            propertyInfo.SetValue(entity.Entity,
                                fromDynamoDbAttributeValue[propertyType](attributeValue));
                        }
                        else
                        {
                            var deserializedValue = JsonConvert.DeserializeObject(attributeValue.S, propertyType);

                            propertyInfo.SetValue(entity.Entity, deserializedValue);
                        }
                    }
                }
            }

            return entity;
        }

        private Dictionary<string, AttributeValue> GetEntityDictionary(Dictionary<string, AttributeValue> docDictionary)
        {
            var entity = docDictionary[AttributeNames.Entity]?.M;

            if (entity == null)
                throw new InvalidOperationException($"[{AttributeNames.Entity}] not found.");

            return entity;
        }

        private string GetEntityId(Dictionary<string, AttributeValue> docDictionary)
        {
            var entityId = docDictionary?[AttributeNames.EntityId]?.S;

            if (string.IsNullOrEmpty(entityId))
                throw new InvalidOperationException($"[{AttributeNames.EntityId}] not found.");

            return entityId;
        }

        private string GetPartitionId(Dictionary<string, AttributeValue> docDictionary)
        {
            var partitionId = docDictionary[AttributeNames.PartitionId]?.S;

            if (string.IsNullOrEmpty(partitionId))
                throw new InvalidOperationException($"[{AttributeNames.PartitionId}] not found.");

            return partitionId;
        }

        private Dictionary<Type, Func<AttributeValue, object>> GetFromDynamoDbAttributeValueConversions()
        {
            return new Dictionary<Type, Func<AttributeValue, object>>
            {
                [typeof(bool)] = av => (av.IsBOOLSet && av.BOOL),
                [typeof(bool?)] = av => (av.IsBOOLSet && av.BOOL),
                [typeof(byte)] = av => av.N.TryParseByte().GetValueOrDefault(),
                [typeof(byte?)] = av => av.N.TryParseByte(),
                [typeof(byte[])] = av => av.B.ToArray(),
                [typeof(DateTime)] = av => av.S.TryParseDateTime().GetValueOrDefault(),
                [typeof(DateTime?)] = av => av.S.TryParseDateTime(),
                [typeof(decimal)] = av => av.N.TryParseDecimal().GetValueOrDefault(),
                [typeof(decimal?)] = av => av.N.TryParseDecimal(),
                [typeof(double)] = av => av.N.TryParseDouble().GetValueOrDefault(),
                [typeof(double?)] = av => av.N.TryParseDouble(),
                [typeof(float)] = av => av.N.TryParseFloat().GetValueOrDefault(),
                [typeof(float?)] = av => av.N.TryParseFloat(),
                [typeof(Guid)] = av => av.S.TryParseGuid().GetValueOrDefault(),
                [typeof(Guid?)] = av => av.S.TryParseGuid(),
                [typeof(int)] = av => av.N.TryParseInt().GetValueOrDefault(),
                [typeof(int?)] = av => av.N.TryParseInt(),
                [typeof(long)] = av => av.N.TryParseLong().GetValueOrDefault(),
                [typeof(long?)] = av => av.N.TryParseLong(),
                [typeof(string)] = av => av.S,
                [typeof(TimeSpan)] = av => av.S.TryParseTimeSpan().GetValueOrDefault(),
                [typeof(TimeSpan?)] = av => av.S.TryParseTimeSpan()
            };
        }

        private Dictionary<Type, Func<object, AttributeValue>> GetToDynamoDbAttributeValueConversions()
        {
            return new Dictionary<Type, Func<object, AttributeValue>>
            {
                [typeof(bool)] = o => new AttributeValue {BOOL = (bool) o},
                [typeof(bool?)] = o => new AttributeValue {BOOL = ((bool?) o).Value},
                [typeof(byte)] = o => new AttributeValue {N = ((byte) o).ToString()},
                [typeof(byte?)] = o => new AttributeValue {N = ((byte?) o).Value.ToString()},
                [typeof(byte[])] = o => new AttributeValue {B = new MemoryStream((byte[]) o)},
                [typeof(DateTime)] = o => new AttributeValue {S = ((DateTime) o).ToString("o")},
                [typeof(DateTime?)] = o => new AttributeValue {S = ((DateTime?) o).Value.ToString("o")},
                [typeof(decimal)] = o => new AttributeValue {N = ((decimal) o).ToString()},
                [typeof(decimal?)] = o => new AttributeValue {N = ((decimal?) o).Value.ToString()},
                [typeof(double)] = o => new AttributeValue {N = ((double) o).ToString()},
                [typeof(double?)] = o => new AttributeValue {N = ((double?) o).Value.ToString()},
                [typeof(float)] = o => new AttributeValue {N = ((float) o).ToString()},
                [typeof(float?)] = o => new AttributeValue {N = ((float?) o).Value.ToString()},
                [typeof(Guid)] = o => new AttributeValue {S = ((Guid) o).ToString()},
                [typeof(Guid?)] = o => new AttributeValue {S = ((Guid?) o).Value.ToString()},
                [typeof(int)] = o => new AttributeValue {N = ((int) o).ToString()},
                [typeof(int?)] = o => new AttributeValue {N = ((int?) o).Value.ToString()},
                [typeof(long)] = o => new AttributeValue {N = ((long) o).ToString()},
                [typeof(long?)] = o => new AttributeValue {N = ((long?) o).Value.ToString()},
                [typeof(string)] = o => new AttributeValue {S = (string) o},
                [typeof(TimeSpan)] = o => new AttributeValue {S = ((TimeSpan) o).ToString()},
                [typeof(TimeSpan?)] = o => new AttributeValue {S = ((TimeSpan?) o).Value.ToString()}
            };
        }

        private static class AttributeNames
        {
            public const string Entity = "Entity";
            public const string EntityId = "EntityId";
            public const string PartitionId = "PartitionId";
        }

        private class DynamoDbDictionaryStorageEntity
        {
            public DynamoDbDictionaryStorageEntity(string entityId, string partitionId, T entity = null)
            {
                Entity = entity ?? new T();
                EntityId = entityId;
                PartitionId = partitionId;
            }

            public T Entity { get; }

            public string EntityId { get; }
            public string PartitionId { get; }
        }
    }
}