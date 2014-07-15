using System;
using Mantle.Configuration.Attributes;
using Mantle.Messaging.Interfaces;
using Mantle.Sample.PublisherConsole.Mantle.Platforms.Azure.Messaging.Contexts;
using Microsoft.ServiceBus.Messaging;

namespace Mantle.Sample.PublisherConsole.Mantle.Platforms.Azure.Messaging.Channels
{
    public class AzureServiceBusSubscriptionSubscriberChannel<T> : BaseAzureServiceBusChannel, ISubscriberChannel<T>
        where T : class
    {
        private SubscriptionClient subscriptionClient;

        [Configurable(IsRequired = true)]
        public override string ServiceBusConnectionString { get; set; }

        [Configurable]
        public bool AutoSetup { get; set; }

        public SubscriptionClient SubscriptionClient
        {
            get { return GetSubscriptionClient(); }
        }

        [Configurable(IsRequired = true)]
        public string SubscriptionName { get; set; }

        [Configurable(IsRequired = true)]
        public string TopicName { get; set; }

        public IMessageContext<T> Receive(TimeSpan? timeout = null)
        {
            var message = ((timeout.HasValue)
                ? (SubscriptionClient.Receive(timeout.Value))
                : (SubscriptionClient.Receive()));

            if (message == null)
                return null;

            return new AzureBrokeredMessageContext<T>(message, message.GetBody<T>());
        }

        private SubscriptionClient GetSubscriptionClient()
        {
            if (subscriptionClient == null)
            {
                if (AutoSetup)
                {
                    if (NamespaceManager.SubscriptionExists(TopicName, SubscriptionName) == false)
                        NamespaceManager.CreateSubscription(TopicName, SubscriptionName);
                }

                subscriptionClient = SubscriptionClient.CreateFromConnectionString(ServiceBusConnectionString,
                                                                                   TopicName, SubscriptionName);
            }

            return subscriptionClient;
        }
    }
}