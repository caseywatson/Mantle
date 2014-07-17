using Mantle.Configuration.Configurers;
using Mantle.DictionaryStorage.Interfaces;
using Mantle.Ninject;
using Mantle.Sample.SubscriberConsole.Mantle.Platforms.Azure.DictionaryStorage.Clients;
using Mantle.Sample.SubscriberConsole.Module.Models;
using Ninject.Modules;

namespace Mantle.Sample.SubscriberConsole.Mantle.Profiles.Azure
{
    public class DictionaryStorageModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDictionaryStorageClient<SampleModel>>()
                .To<AzureTableDictionaryStorageClient<SampleModel>>()
                .InTransientScope()
                .Named("AzDictionary")
                .ConfigureUsing(
                                new CascadingConfigurer<AzureTableDictionaryStorageClient<SampleModel>>(
                                    new AppSettingsConfigurer<AzureTableDictionaryStorageClient<SampleModel>>(),
                                    new ConnectionStringsConfigurer<AzureTableDictionaryStorageClient<SampleModel>>()));
        }
    }
}