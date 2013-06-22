using Mantle.Storage;
using Mantle.Storage.Azure;
using Ninject.Modules;

namespace Mantle.Sample.Simple.Host.Role
{
    public class AzureStorageModule : NinjectModule
    {
        public override void Load()
        {
            // TODO: Setup Azure storage clients here.

            Bind<IStorageClient>()
                .To<AzureBlobStorageClient>()
                .InTransientScope()
                .OnActivation(c => c.Setup("SimpleStorage", "Enter the name of the Azure storage container here."));
        }
    }
}