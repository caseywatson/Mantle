using System;
using Ninject;
using Ninject.Modules;

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
        }

        public NinjectDependencyResolver(params INinjectModule[] modules)
        {
            if (modules == null)
                throw new ArgumentNullException("modules");

            kernel = new StandardKernel(modules);
        }

        public T Get<T>()
        {
            try
            {
                return kernel.Get<T>();
            }
            catch (Exception ex)
            {
                throw new DependencyResolutionException(
                    String.Format(
                        "An error occurred while attempting to resolve dependency type [{0}]. See inner exception for additional details.",
                        typeof (T).FullName), ex);
            }
        }
    }
}