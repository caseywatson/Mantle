using Mantle.BlobStorage.Aws.Clients;
using Mantle.BlobStorage.Azure.Clients;
using Mantle.BlobStorage.Interfaces;
using Mantle.Configuration.Configurers;
using Mantle.Ninject;
using Mantle.PhotoGallery.Processor.Console.Constants;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Processor.Console.Mantle.Default
{
    public class BlobStorageModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBlobStorageClient>()
                .To<AwsS3BlobStorageClient>()
                .InTransientScope()
                .Named(PhotoSources.Aws)
                .ConfigureUsing(new ConnectionStringsConfigurer<AwsS3BlobStorageClient>(),
                                new AppSettingsConfigurer<AwsS3BlobStorageClient>());

            Bind<IBlobStorageClient>()
                .To<AzureBlobStorageClient>()
                .InTransientScope()
                .Named(PhotoSources.Azure)
                .ConfigureUsing(new ConnectionStringsConfigurer<AzureBlobStorageClient>(),
                                new AppSettingsConfigurer<AzureBlobStorageClient>());
        }
    }
}