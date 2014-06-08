using System;
using System.Collections.Generic;

namespace Mantle.Interfaces
{
    public interface IDependencyResolver
    {
        T Get<T>();
        T Get<T>(string name);

        object Get(Type type);
        object Get(Type type, string name);

        IEnumerable<T> GetAll<T>();

        void Release(object target);
    }
}