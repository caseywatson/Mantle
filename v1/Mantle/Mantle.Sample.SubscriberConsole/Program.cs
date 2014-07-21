using System.Linq;
using System.Reflection;
using Mantle.Hosting.Console;
using Mantle.Hosting.Interfaces;
using Mantle.Ninject;
using Ninject;

namespace Mantle.Sample.SubscriberConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var kernel =
                new StandardKernel(Assembly.GetExecutingAssembly().LoadConfiguredProfileMantleModules().ToArray());

            var dependencyResolver = new NinjectDependencyResolver(kernel);

            MantleContext.Current = new MantleContext();
            MantleContext.Current.DependencyResolver = dependencyResolver;

            var workerHost = new ConsoleWorkerHost(dependencyResolver.Get<IWorker>());

            workerHost.Start();
        }
    }
}