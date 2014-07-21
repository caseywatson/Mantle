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
            // TODO: Set up your dictionary storage client(s).

            //Bind<IDictionaryStorageClient<object>>()
            //    .To<AzureTableDictionaryStorageClient<object>>()
            //    .InTransientScope()
            //    .Named("You Azure Table Dictionary Storage Client Name")
            //    .ConfigureUsing(
            //                    new CascadingConfigurer<AzureTableDictionaryStorageClient<object>>(
            //                        new AppSettingsConfigurer<AzureTableDictionaryStorageClient<object>>(),
            //                        new ConnectionStringsConfigurer<AzureTableDictionaryStorageClient<object>>()));
        }
    }
}