using Mantle.BlobStorage.Aws.Clients;
using Mantle.BlobStorage.Interfaces;
using Mantle.Configuration.Configurers;
using Mantle.Ninject;
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
                .ConfigureUsing(new ConnectionStringsConfigurer<AwsS3BlobStorageClient>(),
                                new AppSettingsConfigurer<AwsS3BlobStorageClient>());
        }
    }
}