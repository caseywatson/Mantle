using Mantle.Configuration.Configurers;
using Mantle.DictionaryStorage.Azure.Clients;
using Mantle.DictionaryStorage.Interfaces;
using Mantle.Ninject;
using Mantle.PhotoGallery.PhotoProcessing.Models;
using Mantle.PhotoGallery.Web.Mantle.Constants;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Web.Mantle.Profiles.Azure
{
    public class DictionaryStorageModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<RedisDictionaryStorageClient<PhotoMetadata>>()
            //    .ToSelf()
            //    .InTransientScope()
            //    .Named(DictionaryStorageClientNames.PhotoMetadataCache)
            //    .ConfigureUsing(new ConnectionStringsConfigurer<RedisDictionaryStorageClient<PhotoMetadata>>(),
            //                    new AppSettingsConfigurer<RedisDictionaryStorageClient<PhotoMetadata>>());

            //Bind<AzureTableDictionaryStorageClient<PhotoMetadata>>()
            //    .ToSelf()
            //    .InTransientScope()
            //    .Named(DictionaryStorageClientNames.PhotoMetadata)
            //    .ConfigureUsing(new ConnectionStringsConfigurer<AzureTableDictionaryStorageClient<PhotoMetadata>>(),
            //                    new AppSettingsConfigurer<AzureTableDictionaryStorageClient<PhotoMetadata>>());

            //Bind<IDictionaryStorageClient<PhotoMetadata>>()
            //    .To<LayeredDictionaryStorageClient<
            //        PhotoMetadata,
            //        RedisDictionaryStorageClient<PhotoMetadata>,
            //        AzureTableDictionaryStorageClient<PhotoMetadata>>>()
            //    .InTransientScope();

            Bind<IDictionaryStorageClient<PhotoMetadata>>()
                .To<AzureTableDictionaryStorageClient<PhotoMetadata>>()
                .InTransientScope()
                .ConfigureUsing(new ConnectionStringsConfigurer<AzureTableDictionaryStorageClient<PhotoMetadata>>(),
                                new AppSettingsConfigurer<AzureTableDictionaryStorageClient<PhotoMetadata>>());
        }
    }
}