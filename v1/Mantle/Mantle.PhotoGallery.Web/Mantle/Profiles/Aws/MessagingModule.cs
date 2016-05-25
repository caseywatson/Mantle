using Mantle.Configuration.Configurers;
using Mantle.Messaging.Aws.Channels;
using Mantle.Messaging.Azure.Channels;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Messages;
using Mantle.Ninject;
using Mantle.PhotoGallery.Web.Mantle.Constants;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Web.Mantle.Profiles.Aws
{
    public class MessagingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPublisherChannel<MessageEnvelope>>()
                .To<AzureStorageQueuePublisherChannel<MessageEnvelope>>()
                .InTransientScope()
                .Named(ChannelNames.CopyImageCommandChannel)
                .ConfigureUsing(new AppSettingsConfigurer<AzureStorageQueuePublisherChannel<MessageEnvelope>>());

            Bind<IPublisherChannel<MessageEnvelope>>()
                .To<AwsSqsPublisherChannel<MessageEnvelope>>()
                .InTransientScope()
                .Named(ChannelNames.SaveImageCommandChannel)
                .ConfigureUsing(new AppSettingsConfigurer<AwsSqsPublisherChannel<MessageEnvelope>>());

            Bind<IPublisherChannel<MessageEnvelope>>()
                .To<AzureStorageQueuePublisherChannel<MessageEnvelope>>()
                .InTransientScope()
                .Named(ChannelNames.UserCommandChannel)
                .ConfigureUsing(new AppSettingsConfigurer<AzureStorageQueuePublisherChannel<MessageEnvelope>>());
        }
    }
}