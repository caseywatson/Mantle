using System.Reflection;
using Mantle.Hosting.WindowsServices;
using Mantle.Ninject;
using Ninject;
using Topshelf.HostConfigurators;

namespace Mantle.Sample.AddressBook.WindowsServiceHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            var host = new WindowsServiceWorkerHost(new NinjectDependencyResolver(kernel), ConfigureService);
            host.Start();
        }

        private static void ConfigureService(HostConfigurator configuration)
        {
            // TODO: Provide the name, display name and description of your service.

            configuration.SetServiceName("Replace this text with the name of your service.");
            configuration.SetDisplayName("Replace this text with the display name of your service.");
            configuration.SetDescription("Replace this text with a description of your service.");

            // TODO: Include any service dependencies here.

            // configuration.DependsOnMsmq();
        }
    }
}