using System;
using Mantle.Interfaces;

namespace Mantle
{
    public class DefaultDirectory<T> : IDirectory<T>
    {
        private readonly IDependencyResolver dependencyResolver;

        public DefaultDirectory(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }

        public T this[string name]
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentException("Name is required.", "name");

                return dependencyResolver.Get<T>(name);
            }
        }
    }
}