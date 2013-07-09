using Mantle.Hosting;
using Mantle.Sample.AddressBook.Storage.Worker;
using Ninject.Modules;

namespace Mantle.Sample.AddressBook.Storage.ConsoleHost.Mantle
{
    public class WorkerModule : NinjectModule
    {
        public override void Load()
        {
            // Bind<IWorker>().To<Worker>().InSingletonScope();
        }
    }
}