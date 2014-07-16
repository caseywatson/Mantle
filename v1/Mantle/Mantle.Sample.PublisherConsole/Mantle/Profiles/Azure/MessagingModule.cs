using Mantle.Configuration.Configurers;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Messages;
using Mantle.Ninject;
using Mantle.Sample.PublisherConsole.Mantle.Platforms.Azure.Messaging.Channels;
using Ninject.Modules;

namespace Mantle.Sample.PublisherConsole.Mantle.Profiles.Azure
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
            Bind<IPublisherChannel<MessageEnvelope>>()
                .To<AzureServiceBusQueuePublisherChannel<MessageEnvelope>>()
                .InTransientScope()
                .Named("AzServiceBusQueue")
                .ConfigureUsing(
                                new CascadingConfigurer<AzureServiceBusQueuePublisherChannel<MessageEnvelope>>(
                                    new AppSettingsConfigurer<AzureServiceBusQueuePublisherChannel<MessageEnvelope>>(),
                                    new ConnectionStringsConfigurer
                                        <AzureServiceBusQueuePublisherChannel<MessageEnvelope>>()));

            Bind<IPublisherChannel<MessageEnvelope>>()
                .To<AzureServiceBusTopicPublisherChannel<MessageEnvelope>>()
                .InTransientScope()
                .Named("AzServiceBusTopic")
                .ConfigureUsing(
                                new CascadingConfigurer<AzureServiceBusTopicPublisherChannel<MessageEnvelope>>(
                                    new AppSettingsConfigurer<AzureServiceBusTopicPublisherChannel<MessageEnvelope>>(),
                                    new ConnectionStringsConfigurer
                                        <AzureServiceBusTopicPublisherChannel<MessageEnvelope>>()));

            Bind<IPublisherChannel<MessageEnvelope>>()
                .To<AzureStorageQueuePublisherChannel<MessageEnvelope>>()
                .InTransientScope()
                .Named("AzStorageQueue")
                .ConfigureUsing(
                                new CascadingConfigurer<AzureStorageQueuePublisherChannel<MessageEnvelope>>(
                                    new AppSettingsConfigurer<AzureStorageQueuePublisherChannel<MessageEnvelope>>(),
                                    new ConnectionStringsConfigurer<AzureStorageQueuePublisherChannel<MessageEnvelope>>()));
        }

        private void SetupSubscriberChannels()
        {
            //Bind<ISubscriberChannel<Object>>()
            //    .To<AzureServiceBusQueueSubscriberChannel<Object>>()
            //    .InTransientScope()
            //    .Named("Default")
            //    .ConfigureUsing(
            //                    new CascadingConfigurer<AzureServiceBusQueueSubscriberChannel<Object>>(
            //                        new AppSettingsConfigurer<AzureServiceBusQueueSubscriberChannel<Object>>(),
            //                        new ConnectionStringsConfigurer<AzureServiceBusQueueSubscriberChannel<Object>>()));

            //Bind<ISubscriberChannel<Object>>()
            //    .To<AzureServiceBusSubscriptionSubscriberChannel<Object>>()
            //    .InTransientScope()
            //    .Named("Default")
            //    .ConfigureUsing(
            //                    new CascadingConfigurer<AzureServiceBusSubscriptionSubscriberChannel<Object>>(
            //                        new AppSettingsConfigurer<AzureServiceBusSubscriptionSubscriberChannel<Object>>(),
            //                        new ConnectionStringsConfigurer
            //                            <AzureServiceBusSubscriptionSubscriberChannel<Object>>()));

            //Bind<ISubscriberChannel<Object>>()
            //   .To<AzureStorageQueueSubscriberChannel<Object>>()
            //   .InTransientScope()
            //   .Named("Default")
            //   .ConfigureUsing(
            //                   new CascadingConfigurer<AzureStorageQueueSubscriberChannel<Object>>(
            //                       new AppSettingsConfigurer<AzureStorageQueueSubscriberChannel<Object>>(),
            //                       new ConnectionStringsConfigurer
            //                           <AzureStorageQueueSubscriberChannel<Object>>()));
        }
    }
}