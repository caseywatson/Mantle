using System;
using Mantle.Cache.Interfaces;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.Interfaces;
using StackExchange.Redis;

namespace Mantle.Cache.Azure.Clients
{
    public class AzureRedisCacheClient<T> : ICacheClient<T>
    {
        private readonly ISerializer<T> serializer;

        private ConnectionMultiplexer connectionMultiplexer;
        private IDatabase database;

        public AzureRedisCacheClient(ISerializer<T> serializer)
        {
            this.serializer = serializer;
        }

        public ConnectionMultiplexer ConnectionMultiplexer
        {
            get
            {
                if ((connectionMultiplexer != null) && (connectionMultiplexer.IsConnected))
                    return connectionMultiplexer;

                return (connectionMultiplexer = ConnectionMultiplexer.Connect(ConfigurationString));
            }
        }

        public IDatabase Database
        {
            get { return (database = (database ?? ConnectionMultiplexer.GetDatabase())); }
        }

        [Configurable(IsRequired = true)]
        public string ConfigurationString { get; set; }

        public void Add(T @object, string objectId, TimeSpan? cacheDuration = null)
        {
            objectId.Require("objectId");

            database.StringSet(objectId, serializer.Serialize(@object), cacheDuration);
        }

        public T Get(string objectId)
        {
            objectId.Require("objectId");

            var cachedObject = database.StringGet(objectId);

            if (String.IsNullOrEmpty(cachedObject))
                return default(T);

            return serializer.Deserialize(cachedObject);
        }
    }
}