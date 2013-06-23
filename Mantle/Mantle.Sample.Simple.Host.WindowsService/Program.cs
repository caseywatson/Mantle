using System.Reflection;
using Mantle.Ninject;
using Ninject;

namespace Mantle.Samples.Simple.Host.WindowsService
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var kernel = new StandardKernel();

            kernel.Load(Assembly.GetExecutingAssembly());

            var host = new WindowsServiceWorkerHost(new NinjectDependencyResolver(kernel));

            host.Start();
        }
    }
}