using Ninject.Modules;

namespace Mantle.Sample.AddressBook.Storage.Host.Mantle
{
    public class WorkerModule : NinjectModule
    {
        public override void Load()
        {
            // TODO: Replace with your own concrete worker implementation.
            // Bind<IWorker>().To<Worker>().InSingletonScope();
        }
    }
}