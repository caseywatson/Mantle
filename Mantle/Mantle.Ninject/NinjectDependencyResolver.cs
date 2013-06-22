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

        public NinjectDependencyResolver(params INinjectModule[] ninjectModules)
        {
            if (ninjectModules == null)
                throw new ArgumentNullException("ninjectModules");

            kernel = new StandardKernel(ninjectModules);
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
                    String.Format("An error occurred while attempting to resolve the dependency [{0}].",
                                  typeof (T).FullName), ex);
            }
        }
    }
}