using Mantle.BlobStorage.Azure.Clients;
using Mantle.BlobStorage.Interfaces;
using Mantle.Configuration.Configurers;
using Mantle.Ninject;
using Mantle.PhotoGallery.Web.Mantle.Constants;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Web.Mantle.Profiles.Azure
{
    public class BlobStorageModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBlobStorageClient>()
                .To<AzureBlobStorageClient>()
                .InTransientScope()
                .Named(BlobStorageClientNames.PhotoStorage)
                .ConfigureUsing(new ConnectionStringsConfigurer<AzureBlobStorageClient>(),
                                new AppSettingsConfigurer<AzureBlobStorageClient>());

            Bind<IBlobStorageClient>()
                .To<AzureBlobStorageClient>()
                .InTransientScope()
                .Named(BlobStorageClientNames.ThumbnailStorage)
                .ConfigureUsing(new ConnectionStringsConfigurer<AzureBlobStorageClient>(),
                                new AppSettingsConfigurer<AzureBlobStorageClient>());
        }
    }
}