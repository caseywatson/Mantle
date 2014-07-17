using Mantle.BlobStorage.Interfaces;
using Mantle.Configuration.Configurers;
using Mantle.Ninject;
using Mantle.Sample.SubscriberConsole.Mantle.Platforms.Azure.BlobStorage.Clients;
using Ninject.Modules;

namespace Mantle.Sample.SubscriberConsole.Mantle.Profiles.Azure
{
    public class BlobStorageModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBlobStorageClient>()
                .To<AzureBlobStorageClient>()
                .InTransientScope()
                .Named("AzureBlobs")
                .ConfigureUsing(
                                new CascadingConfigurer<AzureBlobStorageClient>(
                                    new AppSettingsConfigurer<AzureBlobStorageClient>(),
                                    new ConnectionStringsConfigurer<AzureBlobStorageClient>()));
        }
    }
}