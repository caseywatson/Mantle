using Mantle.Cache.Interfaces;
using Mantle.Configuration.Configurers;
using Mantle.Ninject;
using Mantle.Sample.PublisherConsole.Mantle.Platforms.Azure.Cache.Clients;
using Mantle.Sample.PublisherConsole.Module.Models;
using Ninject.Modules;

namespace Mantle.Sample.PublisherConsole.Mantle.Profiles.Azure
{
    public class CacheModule : NinjectModule
    {
        public override void Load()
        {
            // TODO: Set up your cache client(s).

            //Bind<ICacheClient<object>>()
            //    .To<AzureRedisCacheClient<object>>()
            //    .InTransientScope()
            //    .Named("Your Azure Redis Cache Client Name")
            //    .ConfigureUsing(
            //                    new CascadingConfigurer<AzureRedisCacheClient<object>>(
            //                        new AppSettingsConfigurer<AzureRedisCacheClient<object>>(),
            //                        new ConnectionStringsConfigurer<AzureRedisCacheClient<object>>()));

            //Bind<ICacheClient<object>>()
            //    .To<AzureManagedCacheClient<object>>()
            //    .InTransientScope()
            //    .Named("Your Azure Managed Cache Client Name")
            //    .ConfigureUsing(
            //                    new CascadingConfigurer<AzureManagedCacheClient<object>>(
            //                        new AppSettingsConfigurer<AzureManagedCacheClient<object>>(),
            //                        new ConnectionStringsConfigurer<AzureManagedCacheClient<object>>()));
        }
    }
}