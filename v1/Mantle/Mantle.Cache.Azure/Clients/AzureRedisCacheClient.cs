using System;
using Mantle.Cache.Interfaces;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.Interfaces;
using StackExchange.Redis;

namespace Mantle.Cache.Azure.Clients
{
    public class AzureRedisCacheClient<T> : ICacheClient<T>
        where T : class
    {
        private readonly ISerializer<T> serializer;

        private ConnectionMultiplexer connectionMultiplexer;

        public AzureRedisCacheClient(ISerializer<T> serializer)
        {
            this.serializer = serializer;
        }

        [Configurable(IsRequired = true)]
        public string ConfigurationString { get; set; }

        public ConnectionMultiplexer ConnectionMultiplexer
        {
            get { return GetConnectionMultiplexer(); }
        }

        public IDatabase Database
        {
            get { return GetDatabase(); }
        }

        public void Put(T @object, string objectId, TimeSpan? cacheDuration = null)
        {
            @object.Require("object");
            objectId.Require("objectId");

            Database.StringSet(objectId, serializer.Serialize(@object), cacheDuration);
        }

        public T Get(string objectId)
        {
            objectId.Require("objectId");

            var cachedObject = Database.StringGet(objectId);

            if (String.IsNullOrEmpty(cachedObject))
                return default(T);

            return serializer.Deserialize(cachedObject);
        }

        private ConnectionMultiplexer GetConnectionMultiplexer()
        {
            if ((connectionMultiplexer == null) || (connectionMultiplexer.IsConnected == false))
                connectionMultiplexer = ConnectionMultiplexer.Connect(ConfigurationString);

            return connectionMultiplexer;
        }

        private IDatabase GetDatabase()
        {
            return ConnectionMultiplexer.GetDatabase();
        }
    }
}