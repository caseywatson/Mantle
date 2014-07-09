using System;
using System.Runtime.Caching;
using Mantle.Cache.Interfaces;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;

namespace Mantle.Cache.InMemory.Clients
{
    public class InMemoryCacheClient<T> : ICacheClient<T>
        where T : class
    {
        private MemoryCache cache;

        public InMemoryCacheClient()
        {
            SetupDefaultConfiguration();
        }

        public MemoryCache Cache
        {
            get { return GetCache(); }
        }

        [Configurable]
        public TimeSpan CacheExpiration { get; set; }

        [Configurable]
        public string CacheName { get; set; }

        [Configurable]
        public bool UseSlidingExpiration { get; set; }

        public void Put(T @object, string objectId, TimeSpan? cacheExpiration = null)
        {
            objectId.Require("objectId");

            var cachePolicy = new CacheItemPolicy();
            var expiration = (cacheExpiration ?? CacheExpiration);

            if (UseSlidingExpiration)
                cachePolicy.SlidingExpiration = expiration;
            else
                cachePolicy.AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow.Add(expiration));

            cache.Set(objectId, @object, cachePolicy);
        }

        public T Get(string objectId)
        {
            objectId.Require("objectId");

            if (cache.Contains(objectId))
                return ((T) (cache.Get(objectId)));

            return default(T);
        }

        private MemoryCache GetCache()
        {
            if (cache == null)
            {
                if (String.IsNullOrEmpty(CacheName))
                    cache = MemoryCache.Default;
                else
                    cache = new MemoryCache(CacheName);
            }

            return cache;
        }

        private void SetupDefaultConfiguration()
        {
            UseSlidingExpiration = true;
            CacheExpiration = TimeSpan.FromMinutes(10);
        }
    }
}