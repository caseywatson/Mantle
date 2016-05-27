using Mantle.Configuration.Configurers;
using Mantle.Messaging.Azure.Channels;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Messages;
using Mantle.Ninject;
using Mantle.PhotoGallery.Processor.Worker.Constants;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Processor.Console.Mantle.Azure
{
    public class MessagingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISubscriberChannel<MessageEnvelope>>()
                .To<AzureStorageQueueSubscriberChannel<MessageEnvelope>>()
                .InTransientScope()
                .Named(ChannelNames.ProcessorChannel)
                .ConfigureUsing(new ConnectionStringsConfigurer<AzureStorageQueueSubscriberChannel<MessageEnvelope>>(),
                                new AppSettingsConfigurer<AzureStorageQueueSubscriberChannel<MessageEnvelope>>());

            Bind<IPublisherChannel<MessageEnvelope>>()
                .To<AzureStorageQueuePublisherChannel<MessageEnvelope>>()
                .InTransientScope()
                .Named(ChannelNames.SaveImageCommandChannel)
                .ConfigureUsing(new ConnectionStringsConfigurer<AzureStorageQueuePublisherChannel<MessageEnvelope>>(),
                                new AppSettingsConfigurer<AzureStorageQueuePublisherChannel<MessageEnvelope>>());
        }
    }
}