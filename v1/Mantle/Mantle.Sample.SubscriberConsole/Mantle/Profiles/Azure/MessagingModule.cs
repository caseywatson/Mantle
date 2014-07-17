using Mantle.Configuration.Configurers;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Messages;
using Mantle.Ninject;
using Mantle.Sample.SubscriberConsole.Mantle.Platforms.Azure.Messaging.Channels;
using Ninject.Modules;

namespace Mantle.Sample.SubscriberConsole.Mantle.Profiles.Azure
{
    public class MessagingModule : NinjectModule
    {
        public override void Load()
        {
            SetupPublisherChannels();
            SetupSubscriberChannels();
        }

        private void SetupPublisherChannels()
        {
            //Bind<IPublisherChannel<MessageEnvelope>>()
            //    .To<AzureServiceBusQueuePublisherChannel<MessageEnvelope>>()
            //    .InTransientScope()
            //    .Named("AzServiceBusQueue")
            //    .ConfigureUsing(
            //                    new CascadingConfigurer<AzureServiceBusQueuePublisherChannel<MessageEnvelope>>(
            //                        new AppSettingsConfigurer<AzureServiceBusQueuePublisherChannel<MessageEnvelope>>(),
            //                        new ConnectionStringsConfigurer
            //                            <AzureServiceBusQueuePublisherChannel<MessageEnvelope>>()));

            //Bind<IPublisherChannel<MessageEnvelope>>()
            //    .To<AzureServiceBusTopicPublisherChannel<MessageEnvelope>>()
            //    .InTransientScope()
            //    .Named("AzServiceBusTopic")
            //    .ConfigureUsing(
            //                    new CascadingConfigurer<AzureServiceBusTopicPublisherChannel<MessageEnvelope>>(
            //                        new AppSettingsConfigurer<AzureServiceBusTopicPublisherChannel<MessageEnvelope>>(),
            //                        new ConnectionStringsConfigurer
            //                            <AzureServiceBusTopicPublisherChannel<MessageEnvelope>>()));

            //Bind<IPublisherChannel<MessageEnvelope>>()
            //    .To<AzureStorageQueuePublisherChannel<MessageEnvelope>>()
            //    .InTransientScope()
            //    .Named("AzStorageQueue")
            //    .ConfigureUsing(
            //                    new CascadingConfigurer<AzureStorageQueuePublisherChannel<MessageEnvelope>>(
            //                        new AppSettingsConfigurer<AzureStorageQueuePublisherChannel<MessageEnvelope>>(),
            //                        new ConnectionStringsConfigurer<AzureStorageQueuePublisherChannel<MessageEnvelope>>()));
        }

        private void SetupSubscriberChannels()
        {
            Bind<ISubscriberChannel<MessageEnvelope>>()
                .To<AzureServiceBusQueueSubscriberChannel<MessageEnvelope>>()
                .InTransientScope()
                .Named("AzServiceBusQueue")
                .ConfigureUsing(
                                new CascadingConfigurer<AzureServiceBusQueueSubscriberChannel<MessageEnvelope>>(
                                    new AppSettingsConfigurer<AzureServiceBusQueueSubscriberChannel<MessageEnvelope>>(),
                                    new ConnectionStringsConfigurer
                                        <AzureServiceBusQueueSubscriberChannel<MessageEnvelope>>()));

            Bind<ISubscriberChannel<MessageEnvelope>>()
                .To<AzureServiceBusSubscriptionSubscriberChannel<MessageEnvelope>>()
                .InTransientScope()
                .Named("AzServiceBusSubscription")
                .ConfigureUsing(
                                new CascadingConfigurer<AzureServiceBusSubscriptionSubscriberChannel<MessageEnvelope>>(
                                    new AppSettingsConfigurer
                                        <AzureServiceBusSubscriptionSubscriberChannel<MessageEnvelope>>
                                        (),
                                    new ConnectionStringsConfigurer
                                        <AzureServiceBusSubscriptionSubscriberChannel<MessageEnvelope>>()));

            Bind<ISubscriberChannel<MessageEnvelope>>()
                .To<AzureStorageQueueSubscriberChannel<MessageEnvelope>>()
                .InTransientScope()
                .Named("AzStorageQueue")
                .ConfigureUsing(
                                new CascadingConfigurer<AzureStorageQueueSubscriberChannel<MessageEnvelope>>(
                                    new AppSettingsConfigurer<AzureStorageQueueSubscriberChannel<MessageEnvelope>>(),
                                    new ConnectionStringsConfigurer
                                        <AzureStorageQueueSubscriberChannel<MessageEnvelope>>()));
        }
    }
}