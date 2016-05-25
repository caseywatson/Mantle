using System;
using System.Collections.Generic;
using Mantle.Extensions;
using Mantle.Interfaces;
using Ninject;
using Ninject.Planning.Bindings;

namespace Mantle.Ninject
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            kernel.Require("kernel");

            this.kernel = kernel;

            kernel.Bind<IDependencyResolver>().ToConstant(this).InSingletonScope();
        }

        public T Get<T>()
        {
            return kernel.Get<T>();
        }

        public T Get<T>(string name)
        {
            name.Require("name");

            return kernel.Get<T>(name);
        }

        public object Get(Type type)
        {
            type.Require("type");

            return kernel.Get(type);
        }

        public object Get(Type type, string name)
        {
            type.Require("type");
            name.Require("name");

            return kernel.Get(type, name);
        }

        public IEnumerable<T> GetAll<T>()
        {
            return kernel.GetAll<T>();
        }

        public void Release(object target)
        {
            target.Require("target");

            kernel.Release(target);
        }
    }
}