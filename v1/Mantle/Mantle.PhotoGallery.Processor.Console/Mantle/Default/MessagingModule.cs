using Mantle.Configuration.Configurers;
using Mantle.Messaging.Configuration;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Strategies;
using Mantle.Ninject;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Processor.Console.Mantle.Default
{
    public class MessagingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISubscriptionConfiguration>()
                .To<DefaultSubscriptionConfiguration>()
                .InSingletonScope()
                .ConfigureUsing(new AppSettingsConfigurer<DefaultSubscriptionConfiguration>());

            Bind(typeof(IDeadLetterStrategy<>))
                .To(typeof(DefaultDeadLetterStrategy<>))
                .InTransientScope();
        }
    }
}