using System.Reflection;
using Mantle.Console.Hosting;
using Mantle.Ninject;
using Ninject;

namespace Mantle.Sample.AddressBook.Storage.ConsoleHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            var host = new ConsoleWorkerHost(new NinjectDependencyResolver(kernel));
            host.Start();
        }
    }
}