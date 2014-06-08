using System;
using System.Collections.Generic;
using Mantle.Interfaces;
using Ninject;

namespace Mantle.Ninject
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            if (kernel == null)
                throw new ArgumentNullException("kernel");

            this.kernel = kernel;

            kernel.Bind<IDependencyResolver>().ToConstant(this).InSingletonScope();
        }

        public T Get<T>()
        {
            return kernel.Get<T>();
        }

        public T Get<T>(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Name is required.", "name");

            return kernel.Get<T>(name);
        }

        public object Get(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return kernel.Get(type);
        }

        public object Get(Type type, string name)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Name is required.", "name");

            return kernel.Get(type, name);
        }

        public IEnumerable<T> GetAll<T>()
        {
            return kernel.GetAll<T>();
        }

        public void Release(object target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            kernel.Release(target);
        }
    }
}