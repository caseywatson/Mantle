using System;
using Mantle.Configuration.Configurers;
using Mantle.DictionaryStorage.Interfaces;
using Mantle.Ninject;
using Mantle.Sample.PublisherConsole.Mantle.Platforms.Azure.DictionaryStorage.Clients;
using Ninject.Modules;

namespace Mantle.Sample.PublisherConsole.Mantle.Profiles.Azure
{
    public class DictionaryStorageModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<IDictionaryStorageClient<Object>>()
            //    .To<AzureTableDictionaryStorageClient<Object>>()
            //    .InTransientScope()
            //    .Named("Default")
            //    .ConfigureUsing(
            //                    new CascadingConfigurer<AzureTableDictionaryStorageClient<Object>>(
            //                        new AppSettingsConfigurer<AzureTableDictionaryStorageClient<Object>>(),
            //                        new ConnectionStringsConfigurer<AzureTableDictionaryStorageClient<Object>>()));
        }
    }
}