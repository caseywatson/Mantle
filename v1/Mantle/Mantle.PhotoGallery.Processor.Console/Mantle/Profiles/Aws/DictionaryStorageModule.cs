using Mantle.Configuration.Configurers;
using Mantle.DictionaryStorage.Aws.Clients;
using Mantle.DictionaryStorage.Interfaces;
using Mantle.Ninject;
using Mantle.PhotoGallery.PhotoProcessing.Models;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Processor.Console.Mantle.Profiles.Aws
{
    public class DictionaryStorageModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDictionaryStorageClient<PhotoMetadata>>()
                .To<DynamoDbDictionaryStorageClient<PhotoMetadata>>()
                .InTransientScope()
                .ConfigureUsing(new AppSettingsConfigurer<DynamoDbDictionaryStorageClient<PhotoMetadata>>(),
                                new ConnectionStringsConfigurer<DynamoDbDictionaryStorageClient<PhotoMetadata>>());
        }
    }
}