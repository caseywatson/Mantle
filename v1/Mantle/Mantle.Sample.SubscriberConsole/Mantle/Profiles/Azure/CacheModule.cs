using Mantle.Cache.Interfaces;
using Mantle.Configuration.Configurers;
using Mantle.Ninject;
using Mantle.Sample.SubscriberConsole.Mantle.Platforms.Azure.Cache.Clients;
using Mantle.Sample.SubscriberConsole.Module.Models;
using Ninject.Modules;

namespace Mantle.Sample.SubscriberConsole.Mantle.Profiles.Azure
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