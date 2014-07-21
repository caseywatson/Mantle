using Mantle.BlobStorage.Interfaces;
using Mantle.Configuration.Configurers;
using Mantle.Ninject;
using Mantle.Sample.PublisherConsole.Mantle.Platforms.Azure.BlobStorage.Clients;
using Ninject.Modules;

namespace Mantle.Sample.PublisherConsole.Mantle.Profiles.Azure
{
    public class BlobStorageModule : NinjectModule
    {
        public override void Load()
        {
            // TODO: Set up your blob storage client(s).

            //Bind<IBlobStorageClient>()
            //    .To<AzureBlobStorageClient>()
            //    .InTransientScope()
            //    .Named("Your Blob Storage Client Name")
            //    .ConfigureUsing(
            //                    new CascadingConfigurer<AzureBlobStorageClient>(
            //                        new AppSettingsConfigurer<AzureBlobStorageClient>(),
            //                        new ConnectionStringsConfigurer<AzureBlobStorageClient>()));
        }
    }
}