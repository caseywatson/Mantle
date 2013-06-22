using System.Reflection;
using Mantle.Hosting;
using Ninject;

namespace Mantle.Sample.Simple.Host.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var kernel = new StandardKernel();

            kernel.Load(Assembly.GetExecutingAssembly());

            kernel.Get<IWorker>().Start();
        }
    }
}