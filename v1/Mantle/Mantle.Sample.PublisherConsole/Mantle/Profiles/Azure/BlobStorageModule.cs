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
            //Bind<IBlobStorageClient>()
            //    .To<AzureBlobStorageClient>()
            //    .InTransientScope()
            //    .Named("Default")
            //    .ConfigureUsing(
            //                    new CascadingConfigurer<AzureBlobStorageClient>(
            //                        new AppSettingsConfigurer<AzureBlobStorageClient>(),
            //                        new ConnectionStringsConfigurer<AzureBlobStorageClient>()));
        }
    }
}