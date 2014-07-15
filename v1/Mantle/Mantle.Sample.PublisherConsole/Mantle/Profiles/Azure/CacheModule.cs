using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mantle.Cache.Interfaces;
using Mantle.Configuration.Configurers;
using Mantle.Ninject;
using Mantle.Sample.PublisherConsole.Mantle.Platforms.Azure.Cache.Clients;
using Ninject.Modules;

namespace Mantle.Sample.PublisherConsole.Mantle.Profiles.Azure
{
    public class CacheModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<ICacheClient<Object>>()
            //    .To<AzureRedisCacheClient<Object>>()
            //    .InTransientScope()
            //    .Named("Default")
            //    .ConfigureUsing(
            //                    new CascadingConfigurer<AzureRedisCacheClient<Object>>(
            //                        new AppSettingsConfigurer<AzureRedisCacheClient<Object>>(),
            //                        new ConnectionStringsConfigurer<AzureRedisCacheClient<Object>>()));

            //Bind<ICacheClient<Object>>()
            //    .To<AzureManagedCacheClient<Object>>()
            //    .InTransientScope()
            //    .Named("Default")
            //    .ConfigureUsing(
            //                    new CascadingConfigurer<AzureManagedCacheClient<Object>>(
            //                        new AppSettingsConfigurer<AzureManagedCacheClient<Object>>(),
            //                        new ConnectionStringsConfigurer<AzureManagedCacheClient<Object>>()));
        }
    }
}
