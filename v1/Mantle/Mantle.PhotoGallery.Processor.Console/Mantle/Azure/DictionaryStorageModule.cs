using Mantle.Configuration.Configurers;
using Mantle.DictionaryStorage.Azure.Clients;
using Mantle.DictionaryStorage.Clients;
using Mantle.DictionaryStorage.Interfaces;
using Mantle.DictionaryStorage.Redis.Clients;
using Mantle.Ninject;
using Mantle.PhotoGallery.PhotoProcessing.Models;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Processor.Console.Mantle.Azure
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

            Bind<AzureTableDictionaryStorageClient<PhotoMetadata>>()
                .ToSelf()
                .InTransientScope()
                .ConfigureUsing(new ConnectionStringsConfigurer<AzureTableDictionaryStorageClient<PhotoMetadata>>(),
                                new AppSettingsConfigurer<AzureTableDictionaryStorageClient<PhotoMetadata>>());

            Bind<IDictionaryStorageClient<PhotoMetadata>>()
                .To<LayeredDictionaryStorageClient<
                    PhotoMetadata,
                    RedisDictionaryStorageClient<PhotoMetadata>,
                    AzureTableDictionaryStorageClient<PhotoMetadata>>>()
                .InTransientScope();
        }
    }
}