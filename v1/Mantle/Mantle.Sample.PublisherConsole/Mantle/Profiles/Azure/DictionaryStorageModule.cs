using Mantle.Configuration.Configurers;
using Mantle.DictionaryStorage.Interfaces;
using Mantle.Ninject;
using Mantle.Sample.PublisherConsole.Mantle.Platforms.Azure.DictionaryStorage.Clients;
using Mantle.Sample.PublisherConsole.Module.Models;
using Ninject.Modules;

namespace Mantle.Sample.PublisherConsole.Mantle.Profiles.Azure
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