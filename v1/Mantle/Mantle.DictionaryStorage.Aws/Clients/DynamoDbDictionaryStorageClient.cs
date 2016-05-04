using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Mantle.Aws.Interfaces;
using Mantle.Configuration.Attributes;
using Mantle.DictionaryStorage.Entities;
using Mantle.DictionaryStorage.Interfaces;
using Mantle.Extensions;
using Mantle.Interfaces;
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

        private AmazonDynamoDBClient dynamoDbClient;

        public DynamoDbDictionaryStorageClient(IAwsRegionEndpoints awsRegionEndpoints)
        {
            this.awsRegionEndpoints = awsRegionEndpoints;

            fromDynamoDbAttributeValue = GetFromDynamoDbAttributeValueConversions();
            toDynamoDbAttributeValue = GetToDynamoDbAttributeValueConversions();

            typeMetadata = new TypeMetadata<T>();

            AutoSetup = true;
            TableReadCapacityUnits = 10;
            TableWriteCapacityUnits = 10;
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

        [Configurable]
        public int TableReadCapacityUnits { get; set; }

        [Configurable]
        public int TableWriteCapacityUnits { get; set; }

        public AmazonDynamoDBClient AmazonDynamoDbClient => GetAmazonDynamoDbClient();

        public void DeleteEntity(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            AmazonDynamoDbClient.DeleteItem(TableName, new Dictionary<string, AttributeValue>
            {
                [AttributeNames.EntityId] = new AttributeValue {S = entityId},
                [AttributeNames.PartitionId] = new AttributeValue {S = partitionId}
            });
        }

        public bool DoesEntityExist(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            var getItemResult = AmazonDynamoDbClient.GetItem(TableName, new Dictionary<string, AttributeValue>
            {
                [AttributeNames.EntityId] = new AttributeValue {S = entityId},
                [AttributeNames.PartitionId] = new AttributeValue {S = partitionId}
            });

            return getItemResult.IsItemSet;
        }

        public IEnumerable<DictionaryStorageEntity<T>> LoadAllDictionaryStorageEntities(string partitionId)
        {
            const string partitionIdParameter = ":v_partitionId";

            partitionId.Require(nameof(partitionId));

            var queryRequest = new QueryRequest
            {
                TableName = TableName,
                KeyConditionExpression = $"{AttributeNames.PartitionId} = {partitionIdParameter}",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    [partitionIdParameter] = new AttributeValue { S = partitionId }
                }
            };

            var queryResponse = AmazonDynamoDbClient.Query(queryRequest);

            return queryResponse.Items.Select(ToDictionaryStorageEntity);
        }

        public void InsertOrUpdateDictionaryStorageEntities(IEnumerable<DictionaryStorageEntity<T>> entities)
        {
            entities.Require(nameof(entities));

            foreach (var entityChunk in entities.Chunk(25))
            {
                var writeRequests = entityChunk
                    .Select(e => new WriteRequest(new PutRequest(ToDocumentDictionary(e))))
                    .ToList();

                var batchRequest = new BatchWriteItemRequest(new Dictionary<string, List<WriteRequest>>
                {
                    [TableName] = writeRequests
                });

                AmazonDynamoDbClient.BatchWriteItem(batchRequest);
            }
        }

        public void InsertOrUpdateDictionaryStorageEntity(DictionaryStorageEntity<T> entity)
        {
            entity.Require(nameof(entity));

            AmazonDynamoDbClient.PutItem(TableName, ToDocumentDictionary(entity));
        }

        public DictionaryStorageEntity<T> LoadDictionaryStorageEntity(string entityId, string partitionId)
        {
            var getItemResult = AmazonDynamoDbClient.GetItem(TableName, new Dictionary<string, AttributeValue>
            {
                [AttributeNames.EntityId] = new AttributeValue { S = entityId },
                [AttributeNames.PartitionId] = new AttributeValue { S = partitionId }
            });

            if (getItemResult.IsItemSet)
                return ToDictionaryStorageEntity(getItemResult.Item);

            return null;
        }

        private Dictionary<string, AttributeValue> ToDocumentDictionary(DictionaryStorageEntity<T> entity)
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

        private DictionaryStorageEntity<T> ToDictionaryStorageEntity(Dictionary<string, AttributeValue> docDictionary)
        {
            var entityDictionary = GetEntityDictionary(docDictionary);

            var entity = new DictionaryStorageEntity<T>(
                GetEntityId(docDictionary), GetPartitionId(docDictionary), new T());

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

        private AmazonDynamoDBClient GetAmazonDynamoDbClient()
        {
            if (dynamoDbClient == null)
            {
                var awsRegionEndpoint = awsRegionEndpoints.GetRegionEndpointByName(AwsRegionName);

                if (awsRegionEndpoint == null)
                    throw new ConfigurationErrorsException($"[{AwsRegionName}] is not a knnown AWS region.");

                dynamoDbClient = new AmazonDynamoDBClient(AwsAccessKeyId, AwsSecretAccessKey, awsRegionEndpoint);

                if (AutoSetup)
                    SetupTable(dynamoDbClient);
            }

            return dynamoDbClient;
        }

        private void SetupTable(AmazonDynamoDBClient dynamoDbClient)
        {
            if (DoesTableExist(dynamoDbClient) == false)
            {
                var createTableRequest = new CreateTableRequest
                {
                    TableName = TableName,
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition
                        {
                            AttributeName = AttributeNames.EntityId,
                            AttributeType = ScalarAttributeType.S
                        },
                        new AttributeDefinition
                        {
                            AttributeName = AttributeNames.PartitionId,
                            AttributeType = ScalarAttributeType.S
                        }
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = AttributeNames.EntityId,
                            KeyType = KeyType.RANGE
                        },
                        new KeySchemaElement
                        {
                            AttributeName = AttributeNames.PartitionId,
                            KeyType = KeyType.HASH
                        }
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = TableReadCapacityUnits,
                        WriteCapacityUnits = TableWriteCapacityUnits
                    }
                };

                dynamoDbClient.CreateTable(createTableRequest);
                WaitUntilTableExists(dynamoDbClient);
            }
        }

        private void WaitUntilTableExists(AmazonDynamoDBClient dynamoDbClient)
        {
            while (true)
            {
                if (DoesTableExist(dynamoDbClient))
                    return;

                Thread.Sleep(5000);
            }
        }

        private bool DoesTableExist(AmazonDynamoDBClient dynamoDbClient)
        {
            try
            {
                return (dynamoDbClient.DescribeTable(TableName).Table?.TableStatus == TableStatus.ACTIVE);
            }
            catch (ResourceNotFoundException)
            {
                return false;
            }
        }

        private static class AttributeNames
        {
            public const string Entity = "Entity";
            public const string EntityId = "EntityId";
            public const string PartitionId = "PartitionId";
        }
    }
}