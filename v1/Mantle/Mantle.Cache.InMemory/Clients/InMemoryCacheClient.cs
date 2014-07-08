using System;
using System.Runtime.Caching;
using Mantle.Cache.Interfaces;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;

namespace Mantle.Cache.InMemory.Clients
{
    public class InMemoryCacheClient<T> : ICacheClient<T>
    {
        private readonly TimeSpan defaultSlidingExpiration;

        private MemoryCache cache;

        public InMemoryCacheClient()
        {
            defaultSlidingExpiration = TimeSpan.FromMinutes(20);
        }

        [Configurable]
        public string CacheName { get; set; }

        [Configurable]
        public TimeSpan? AbsoluteExpiration { get; set; }

        [Configurable]
        public TimeSpan? SlidingExpiration { get; set; }

        public MemoryCache Cache
        {
            get
            {
                return (cache = (cache ??
                                 ((String.IsNullOrEmpty(CacheName))
                                     ? MemoryCache.Default
                                     : new MemoryCache(CacheName))));
            }
        }

        public void Add(T @object, string objectId, TimeSpan? cacheDuration = null)
        {
            objectId.Require("objectId");

            var cachePolicy = new CacheItemPolicy();

            if (cacheDuration.HasValue)
                cachePolicy.SlidingExpiration = cacheDuration.Value;
            else if (AbsoluteExpiration.HasValue)
                cachePolicy.AbsoluteExpiration = new DateTimeOffset(DateTime.Now, AbsoluteExpiration.Value);
            else if (SlidingExpiration.HasValue)
                cachePolicy.SlidingExpiration = SlidingExpiration.Value;
            else
                cachePolicy.SlidingExpiration = defaultSlidingExpiration;

            cache.Set(objectId, @object, cachePolicy);
        }

        public T Get(string objectId)
        {
            objectId.Require("objectId");

            if (cache.Contains(objectId))
                return ((T) (cache.Get(objectId)));

            return default(T);
        }
    }
}