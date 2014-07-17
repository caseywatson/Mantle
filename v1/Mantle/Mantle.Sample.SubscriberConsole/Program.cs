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

            var workerHost =
                new ConsoleWorkerHost(new NinjectDependencyResolver(kernel).Get<IWorker>());

            workerHost.Start();
        }
    }
}