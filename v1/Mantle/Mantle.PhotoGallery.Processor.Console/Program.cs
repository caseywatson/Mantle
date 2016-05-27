using System.Linq;
using System.Reflection;
using Mantle.Hosting.Console;
using Mantle.Hosting.Interfaces;
using Mantle.Ninject;
using Mantle.Providers;
using Ninject;

namespace Mantle.PhotoGallery.Processor.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var profiles = ((args?.Length > 0) ? args : (new AppSettingProfileProvider().GetProfiles()));
            var kernel = new StandardKernel(assembly.LoadProfileNinjectModules(profiles).ToArray());
            var dependencyResolver = new NinjectDependencyResolver(kernel);

            MantleContext.Current = new MantleContext(dependencyResolver, profiles);

            var workerHost = new ConsoleWorkerHost(dependencyResolver.Get<IWorker>());

            workerHost.Start();
        }
    }
}