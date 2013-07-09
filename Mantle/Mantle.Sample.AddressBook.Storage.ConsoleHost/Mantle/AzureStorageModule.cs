using Mantle.Storage;
using Mantle.Storage.Azure;
using Ninject.Modules;

namespace Mantle.Sample.AddressBook.Storage.ConsoleHost.Mantle
{
    public class AzureStorageModule : NinjectModule
    {
        public override void Load()
        {
            // TODO: Setup Azure storage clients here.

            //Bind<IStorageClient>()
            //    .To<AzureBlobStorageClient>()
            //    .InSingletonScope()
            //    .OnActivation(c => c.Configure("My Storage Client", "Replace this text with the name of your Azure storage container."));
        }
    }
}