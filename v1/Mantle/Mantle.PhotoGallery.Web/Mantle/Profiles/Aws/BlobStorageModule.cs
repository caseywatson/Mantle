using Mantle.BlobStorage.Aws.Clients;
using Mantle.BlobStorage.Interfaces;
using Mantle.Configuration.Configurers;
using Mantle.Ninject;
using Mantle.PhotoGallery.Web.Mantle.Constants;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Web.Mantle.Profiles.Aws
{
    public class BlobStorageModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBlobStorageClient>()
                .To<AwsS3BlobStorageClient>()
                .InTransientScope()
                .Named(BlobStorageClientNames.PhotoStorage)
                .ConfigureUsing(new ConnectionStringsConfigurer<AwsS3BlobStorageClient>(),
                                new AppSettingsConfigurer<AwsS3BlobStorageClient>());

            Bind<IBlobStorageClient>()
               .To<AwsS3BlobStorageClient>()
               .InTransientScope()
               .Named(BlobStorageClientNames.ThumbnailStorage)
               .ConfigureUsing(new ConnectionStringsConfigurer<AwsS3BlobStorageClient>(),
                               new AppSettingsConfigurer<AwsS3BlobStorageClient>());
        }
    }
}