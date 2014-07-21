using Mantle.Configuration.Configurers;
using Mantle.Messaging.Interfaces;
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
            // TODO: Set up your publisher channel(s).

            //Bind<IPublisherChannel<object>>()
            //    .To<AzureServiceBusQueuePublisherChannel<object>>()
            //    .InTransientScope()
            //    .Named("Your Azure Service Bus Queue Publisher Channel Name")
            //    .ConfigureUsing(
            //                    new CascadingConfigurer<AzureServiceBusQueuePublisherChannel<object>>(
            //                        new AppSettingsConfigurer<AzureServiceBusQueuePublisherChannel<object>>(),
            //                        new ConnectionStringsConfigurer
            //                            <AzureServiceBusQueuePublisherChannel<object>>()));

            //Bind<IPublisherChannel<object>>()
            //    .To<AzureServiceBusTopicPublisherChannel<object>>()
            //    .InTransientScope()
            //    .Named("Your Azure Service Bus Topic Publisher Channel Name")
            //    .ConfigureUsing(
            //                    new CascadingConfigurer<AzureServiceBusTopicPublisherChannel<object>>(
            //                        new AppSettingsConfigurer<AzureServiceBusTopicPublisherChannel<object>>(),
            //                        new ConnectionStringsConfigurer
            //                            <AzureServiceBusTopicPublisherChannel<object>>()));

            //Bind<IPublisherChannel<object>>()
            //    .To<AzureStorageQueuePublisherChannel<object>>()
            //    .InTransientScope()
            //    .Named("Your Azure Storage Queue Publisher Channel Name")
            //    .ConfigureUsing(
            //                    new CascadingConfigurer<AzureStorageQueuePublisherChannel<object>>(
            //                        new AppSettingsConfigurer<AzureStorageQueuePublisherChannel<object>>(),
            //                        new ConnectionStringsConfigurer<AzureStorageQueuePublisherChannel<object>>()));
        }

        private void SetupSubscriberChannels()
        {
            // TODO: Set up your subscriber channel(s).

            //Bind<ISubscriberChannel<object>>()
            //    .To<AzureServiceBusQueueSubscriberChannel<object>>()
            //    .InTransientScope()
            //    .Named("Your Azure Service Bus Queue Subscriber Channel Name")
            //    .ConfigureUsing(
            //                    new CascadingConfigurer<AzureServiceBusQueueSubscriberChannel<object>>(
            //                        new AppSettingsConfigurer<AzureServiceBusQueueSubscriberChannel<object>>(),
            //                        new ConnectionStringsConfigurer<AzureServiceBusQueueSubscriberChannel<object>>()));

            //Bind<ISubscriberChannel<object>>()
            //    .To<AzureServiceBusSubscriptionSubscriberChannel<object>>()
            //    .InTransientScope()
            //    .Named("Your Azure Service Bus Subscription Subscriber Channel Name")
            //    .ConfigureUsing(
            //                    new CascadingConfigurer<AzureServiceBusSubscriptionSubscriberChannel<object>>(
            //                        new AppSettingsConfigurer<AzureServiceBusSubscriptionSubscriberChannel<object>>(),
            //                        new ConnectionStringsConfigurer
            //                            <AzureServiceBusSubscriptionSubscriberChannel<object>>()));

            //Bind<ISubscriberChannel<object>>()
            //   .To<AzureStorageQueueSubscriberChannel<object>>()
            //   .InTransientScope()
            //   .Named("Your Azure Storage Queue Subscriber Channel Name")
            //   .ConfigureUsing(
            //                   new CascadingConfigurer<AzureStorageQueueSubscriberChannel<object>>(
            //                       new AppSettingsConfigurer<AzureStorageQueueSubscriberChannel<object>>(),
            //                       new ConnectionStringsConfigurer
            //                           <AzureStorageQueueSubscriberChannel<object>>()));
        }
    }
}