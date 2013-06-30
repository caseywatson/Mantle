using Mantle.Storage;
using Mantle.Storage.Azure;
using Ninject.Modules;

namespace Mantle.Sample.AddressBook.Storage.ConsoleHost.Mantle
{
    public class AzureStorageModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IStorageClient>()
                .To<AzureBlobStorageClient>()
                .InSingletonScope()
                .OnActivation(c => c.Setup("PersonStorage", "addressbook"));
        }
    }
}