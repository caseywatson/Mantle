using Mantle.Configuration.Configurers;
using Mantle.Messaging.Aws.Channels;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Messages;
using Mantle.Ninject;
using Mantle.PhotoGallery.Processor.Worker.Constants;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Processor.Console.Mantle.Aws
{
    public class MessagingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISubscriberChannel<MessageEnvelope>>()
                .To<AwsSqsSubscriberChannel<MessageEnvelope>>()
                .InTransientScope()
                .Named(ChannelNames.ProcessorChannel)
                .ConfigureUsing(new ConnectionStringsConfigurer<AwsSqsSubscriberChannel<MessageEnvelope>>(),
                                new AppSettingsConfigurer<AwsSqsSubscriberChannel<MessageEnvelope>>());

            Bind<IPublisherChannel<MessageEnvelope>>()
                .To<AwsSqsPublisherChannel<MessageEnvelope>>()
                .InTransientScope()
                .Named(ChannelNames.SaveImageCommandChannel)
                .ConfigureUsing(new ConnectionStringsConfigurer<AwsSqsPublisherChannel<MessageEnvelope>>(),
                                new AppSettingsConfigurer<AwsSqsPublisherChannel<MessageEnvelope>>());
        }
    }
}