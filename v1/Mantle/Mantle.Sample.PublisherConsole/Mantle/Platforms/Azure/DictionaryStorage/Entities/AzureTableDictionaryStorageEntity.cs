using System;
using System.Collections.Generic;
using Mantle.Extensions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace Mantle.Sample.PublisherConsole.Mantle.Platforms.Azure.DictionaryStorage.Entities
{
    public class AzureTableDictionaryStorageEntity<T> : ITableEntity
        where T : class, new()
    {
        private readonly TypeMetadata typeMetadata;

        public AzureTableDictionaryStorageEntity()
        {
            typeMetadata = new TypeMetadata(typeof (T));
        }

        public AzureTableDictionaryStorageEntity(TypeMetadata typeMetadata)
        {
            typeMetadata.Require("typeMetadata");

            this.typeMetadata = typeMetadata;
        }

        public T Data { get; set; }
        public string ETag { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        public void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            var t = new T();

            foreach (PropertyMetadata outputProperty in typeMetadata.Properties)
            {
                if (properties.ContainsKey(outputProperty.PropertyInfo.Name))
                {
                    EntityProperty inputProperty = properties[outputProperty.PropertyInfo.Name];
                    Type propertyType = outputProperty.PropertyInfo.PropertyType;

                    if ((propertyType == typeof (bool)) && (inputProperty.BooleanValue.HasValue))
                        outputProperty.PropertyInfo.SetValue(t, inputProperty.BooleanValue.Value);
                    else if ((propertyType == typeof (bool?)) && (inputProperty.BooleanValue.HasValue))
                        outputProperty.PropertyInfo.SetValue(t, inputProperty.BooleanValue);
                    else if ((propertyType == typeof (byte)) && (inputProperty.Int32Value.HasValue))
                        outputProperty.PropertyInfo.SetValue(t, ((byte) (inputProperty.Int32Value.Value)));
                    else if ((propertyType == typeof (byte?)) && (inputProperty.Int32Value.HasValue))
                        outputProperty.PropertyInfo.SetValue(t, ((byte) (inputProperty.Int32Value.Value)));
                    else if ((propertyType == typeof (byte[])) && (inputProperty.BinaryValue != null))
                        outputProperty.PropertyInfo.SetValue(t, inputProperty.BinaryValue);
                    else if ((propertyType == typeof (decimal)) && (inputProperty.DoubleValue.HasValue))
                        outputProperty.PropertyInfo.SetValue(t, ((decimal) (inputProperty.DoubleValue.Value)));
                    else if ((propertyType == typeof (decimal?)) && (inputProperty.DoubleValue.HasValue))
                        outputProperty.PropertyInfo.SetValue(t, ((decimal) (inputProperty.DoubleValue.Value)));
                    else if ((propertyType == typeof (DateTime)) && (inputProperty.DateTime.HasValue))
                        outputProperty.PropertyInfo.SetValue(t, inputProperty.DateTime.Value);
                    else if ((propertyType == typeof (DateTime?)) && (inputProperty.DateTime.HasValue))
                        outputProperty.PropertyInfo.SetValue(t, inputProperty.DateTime);
                    else if ((propertyType == typeof (double)) && (inputProperty.DoubleValue.HasValue))
                        outputProperty.PropertyInfo.SetValue(t, inputProperty.DoubleValue.Value);
                    else if ((propertyType == typeof (double?)) && (inputProperty.DoubleValue.HasValue))
                        outputProperty.PropertyInfo.SetValue(t, inputProperty.DoubleValue);
                    else if ((propertyType == typeof (float)) && (inputProperty.DoubleValue.HasValue))
                        outputProperty.PropertyInfo.SetValue(t, ((float) (inputProperty.DoubleValue.Value)));
                    else if ((propertyType == typeof (float?)) && (inputProperty.DoubleValue.HasValue))
                        outputProperty.PropertyInfo.SetValue(t, ((float) (inputProperty.DoubleValue.Value)));
                    else if ((propertyType == typeof (Guid)) && (inputProperty.GuidValue.HasValue))
                        outputProperty.PropertyInfo.SetValue(t, inputProperty.GuidValue.Value);
                    else if ((propertyType == typeof (Guid?)) && (inputProperty.GuidValue.HasValue))
                        outputProperty.PropertyInfo.SetValue(t, inputProperty.GuidValue);
                    else if ((propertyType == typeof (int)) && (inputProperty.Int32Value.HasValue))
                        outputProperty.PropertyInfo.SetValue(t, inputProperty.Int32Value.Value);
                    else if ((propertyType == typeof (int?)) && (inputProperty.Int32Value.HasValue))
                        outputProperty.PropertyInfo.SetValue(t, inputProperty.Int32Value);
                    else if ((propertyType == typeof (long)) && (inputProperty.Int64Value.HasValue))
                        outputProperty.PropertyInfo.SetValue(t, inputProperty.Int64Value.Value);
                    else if ((propertyType == typeof (long?)) && (inputProperty.Int64Value.HasValue))
                        outputProperty.PropertyInfo.SetValue(t, inputProperty.Int64Value);
                    else if ((propertyType == typeof (string)) && (inputProperty.StringValue != null))
                        outputProperty.PropertyInfo.SetValue(t, inputProperty.StringValue);
                    else if (inputProperty.StringValue != null)
                        outputProperty.PropertyInfo.SetValue(t,
                                                             JsonConvert.DeserializeObject(inputProperty.StringValue,
                                                                                           propertyType));
                }
            }

            Data = t;
        }

        public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var dictionary = new Dictionary<string, EntityProperty>();

            foreach (PropertyMetadata inputProperty in typeMetadata.Properties)
            {
                string propertyName = inputProperty.PropertyInfo.Name;
                Type propertyType = inputProperty.PropertyInfo.PropertyType;
                object propertyValue = inputProperty.PropertyInfo.GetValue(Data);

                if (propertyValue != null)
                {
                    if (propertyType == typeof (bool))
                        dictionary[propertyName] = new EntityProperty((bool) (propertyValue));
                    else if (propertyType == typeof (bool?))
                        dictionary[propertyName] = new EntityProperty((bool?) (propertyValue));
                    else if (propertyType == typeof (byte))
                        dictionary[propertyName] = new EntityProperty((byte) (propertyValue));
                    else if (propertyType == typeof (byte?))
                        dictionary[propertyName] = new EntityProperty((byte?) (propertyValue));
                    else if (propertyType == typeof (byte[]))
                        dictionary[propertyName] = new EntityProperty((byte[]) (propertyValue));
                    else if (propertyType == typeof (decimal))
                        dictionary[propertyName] = new EntityProperty((double) (decimal) (propertyValue));
                    else if (propertyType == typeof (decimal?))
                        dictionary[propertyName] = new EntityProperty((double?) (decimal?) (propertyValue));
                    else if (propertyType == typeof (DateTime))
                        dictionary[propertyName] = new EntityProperty((DateTime) (propertyValue));
                    else if (propertyType == typeof (DateTime?))
                        dictionary[propertyName] = new EntityProperty((DateTime?) (propertyValue));
                    else if (propertyType == typeof (double))
                        dictionary[propertyName] = new EntityProperty((double) (propertyValue));
                    else if (propertyType == typeof (double?))
                        dictionary[propertyName] = new EntityProperty((double?) (propertyValue));
                    else if (propertyType == typeof (float))
                        dictionary[propertyName] = new EntityProperty((float) (propertyValue));
                    else if (propertyType == typeof (float?))
                        dictionary[propertyName] = new EntityProperty((float?) (propertyValue));
                    else if (propertyType == typeof (Guid))
                        dictionary[propertyName] = new EntityProperty((Guid) (propertyValue));
                    else if (propertyType == typeof (Guid?))
                        dictionary[propertyName] = new EntityProperty((Guid?) (propertyValue));
                    else if (propertyType == typeof (int))
                        dictionary[propertyName] = new EntityProperty((int) (propertyValue));
                    else if (propertyType == typeof (int?))
                        dictionary[propertyName] = new EntityProperty((int?) (propertyValue));
                    else if (propertyType == typeof (long))
                        dictionary[propertyName] = new EntityProperty((long) (propertyValue));
                    else if (propertyType == typeof (long?))
                        dictionary[propertyName] = new EntityProperty((long?) (propertyValue));
                    else if (propertyType == typeof (string))
                        dictionary[propertyName] = new EntityProperty((string) (propertyValue));
                    else
                        dictionary[propertyName] =
                            new EntityProperty(JsonConvert.SerializeObject(propertyValue, Formatting.Indented));
                }
            }

            return dictionary;
        }
    }
}