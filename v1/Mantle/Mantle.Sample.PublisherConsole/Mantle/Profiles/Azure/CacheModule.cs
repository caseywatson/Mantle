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
            //Bind<ICacheClient<SampleModel>>()
            //    .To<AzureRedisCacheClient<SampleModel>>()
            //    .InTransientScope()
            //    .Named("AzRedisCache")
            //    .ConfigureUsing(
            //                    new CascadingConfigurer<AzureRedisCacheClient<SampleModel>>(
            //                        new AppSettingsConfigurer<AzureRedisCacheClient<SampleModel>>(),
            //                        new ConnectionStringsConfigurer<AzureRedisCacheClient<SampleModel>>()));

            Bind<ICacheClient<SampleModel>>()
                .To<AzureManagedCacheClient<SampleModel>>()
                .InTransientScope()
                .Named("AzManagedCache")
                .ConfigureUsing(
                                new CascadingConfigurer<AzureManagedCacheClient<SampleModel>>(
                                    new AppSettingsConfigurer<AzureManagedCacheClient<SampleModel>>(),
                                    new ConnectionStringsConfigurer<AzureManagedCacheClient<SampleModel>>()));
        }
    }
}