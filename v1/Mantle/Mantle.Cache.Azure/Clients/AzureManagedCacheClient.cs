using System;
using Mantle.Cache.Interfaces;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Microsoft.ApplicationServer.Caching;

namespace Mantle.Cache.Azure.Clients
{
    public class AzureManagedCacheClient<T> : ICacheClient<T>
    {
        private const int CachePort = 22233;

        private DataCache dataCache;

        public DataCache DataCache
        {
            get { return (dataCache = (dataCache ?? CreateDataCache())); }
        }

        [Configurable]
        public string CacheName { get; set; }

        [Configurable(IsRequired = true)]
        public string AuthorizationToken { get; set; }

        [Configurable(IsRequired = true)]
        public string CacheUrl { get; set; }

        public void Add(T @object, string objectId, TimeSpan? cacheDuration = null)
        {
            objectId.Require("objectId");

            if (cacheDuration == null)
                DataCache.Add(objectId, @object);
            else
                DataCache.Add(objectId, @object, cacheDuration.Value);
        }

        public T Get(string objectId)
        {
            objectId.Require("objectId");

            var cachedObject = DataCache.Get(objectId);

            if (cachedObject == null)
                return default(T);

            return ((T) (cachedObject));
        }

        private DataCache CreateDataCache()
        {
            var dcfConfiguration = new DataCacheFactoryConfiguration();

            dcfConfiguration.SecurityProperties = new DataCacheSecurity(AuthorizationToken);
            dcfConfiguration.Servers = new[] {new DataCacheServerEndpoint(CacheUrl, CachePort)};

            var factory = new DataCacheFactory(dcfConfiguration);

            if (String.IsNullOrEmpty(CacheName))
                return factory.GetDefaultCache();

            return factory.GetCache(CacheName);
        }
    }
}