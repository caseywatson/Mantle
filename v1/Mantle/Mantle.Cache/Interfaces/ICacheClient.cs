using System;

namespace Mantle.Cache.Interfaces
{
    public interface ICacheClient<T>
        where T : class
    {
        T Get(string objectId);
        void Put(T @object, string objectId, TimeSpan? cacheDuration = null);
    }
}