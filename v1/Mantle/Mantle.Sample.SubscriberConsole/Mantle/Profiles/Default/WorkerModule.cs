using Mantle.Configuration.Configurers;
using Mantle.Hosting.Interfaces;
using Mantle.Messaging.Interfaces;
using Mantle.Ninject;
using Mantle.Sample.SubscriberConsole.Module.Models;
using Mantle.Sample.SubscriberConsole.Module.Subscribers;
using Mantle.Sample.SubscriberConsole.Module.Workers;
using Ninject.Modules;

namespace Mantle.Sample.SubscriberConsole.Mantle.Profiles.Default
{
    public class WorkerModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IWorker>()
                .To<SampleSubscriberWorker>()
                .InSingletonScope()
                .ConfigureUsing(
                                new CascadingConfigurer<SampleSubscriberWorker>(
                                    new AppSettingsConfigurer<SampleSubscriberWorker>(),
                                    new ConnectionStringsConfigurer<SampleSubscriberWorker>()));

            Bind<ISubscriber<SampleModel>>().To<SampleModelSubscriber>().InTransientScope();
        }
    }
}