using System;
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
            //Bind<IPublisherChannel<Object>>()
            //    .To<AzureServiceBusQueuePublisherChannel<Object>>()
            //    .InTransientScope()
            //    .Named("Default")
            //    .ConfigureUsing(
            //                    new CascadingConfigurer<AzureServiceBusQueuePublisherChannel<Object>>(
            //                        new AppSettingsConfigurer<AzureServiceBusQueuePublisherChannel<Object>>(),
            //                        new ConnectionStringsConfigurer<AzureServiceBusQueuePublisherChannel<Object>>()));

            //Bind<IPublisherChannel<Object>>()
            //    .To<AzureServiceBusTopicPublisherChannel<Object>>()
            //    .InTransientScope()
            //    .Named("Default")
            //    .ConfigureUsing(
            //                    new CascadingConfigurer<AzureServiceBusTopicPublisherChannel<Object>>(
            //                        new AppSettingsConfigurer<AzureServiceBusTopicPublisherChannel<Object>>(),
            //                        new ConnectionStringsConfigurer<AzureServiceBusTopicPublisherChannel<Object>>()));

            //Bind<IPublisherChannel<Object>>()
            //    .To<AzureStorageQueuePublisherChannel<Object>>()
            //    .InTransientScope()
            //    .Named("Default")
            //    .ConfigureUsing(
            //                    new CascadingConfigurer<AzureStorageQueuePublisherChannel<Object>>(
            //                        new AppSettingsConfigurer<AzureStorageQueuePublisherChannel<Object>>(),
            //                        new ConnectionStringsConfigurer<AzureStorageQueuePublisherChannel<Object>>()));
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