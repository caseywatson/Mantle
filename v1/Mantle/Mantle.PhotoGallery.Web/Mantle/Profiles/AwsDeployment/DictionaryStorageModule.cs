using Mantle.Configuration.Configurers;
using Mantle.DictionaryStorage.Aws.Clients;
using Mantle.DictionaryStorage.Clients;
using Mantle.DictionaryStorage.Interfaces;
using Mantle.DictionaryStorage.Redis.Clients;
using Mantle.Ninject;
using Ninject.Modules;
using Mantle.PhotoGallery.PhotoProcessing.Models;

namespace Mantle.PhotoGallery.Web.Mantle.Profiles.AwsDeployment
{
    public class DictionaryStorageModule : NinjectModule
    {
        public override void Load()
        {
            Bind<RedisDictionaryStorageClient<PhotoMetadata>>()
                .ToSelf()
                .InTransientScope()
                .ConfigureUsing(new ConnectionStringsConfigurer<RedisDictionaryStorageClient<PhotoMetadata>>(),
                                new AppSettingsConfigurer<RedisDictionaryStorageClient<PhotoMetadata>>());

            Bind<DynamoDbDictionaryStorageClient<PhotoMetadata>>()
                .ToSelf()
                .InTransientScope()
                .ConfigureUsing(new ConnectionStringsConfigurer<DynamoDbDictionaryStorageClient<PhotoMetadata>>(),
                                new AppSettingsConfigurer<DynamoDbDictionaryStorageClient<PhotoMetadata>>());

            Bind<IDictionaryStorageClient<PhotoMetadata>>()
                .To<LayeredDictionaryStorageClient<
                    PhotoMetadata, 
                    RedisDictionaryStorageClient<PhotoMetadata>,
                    DynamoDbDictionaryStorageClient<PhotoMetadata>>>()
                .InTransientScope();
        }
    }
}
