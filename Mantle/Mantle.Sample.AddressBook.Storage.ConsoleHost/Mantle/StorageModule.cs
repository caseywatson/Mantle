using Mantle.Storage;
using Ninject.Modules;

namespace Mantle.Sample.AddressBook.Storage.ConsoleHost.Mantle
{
    public class StorageModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IStorageClientDirectory>().To<StorageClientDirectory>().InSingletonScope();
        }
    }
}