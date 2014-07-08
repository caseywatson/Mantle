using System;
using Mantle.Configuration.Attributes;
using Mantle.Messaging.Azure.Context;
using Mantle.Messaging.Interfaces;
using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.Azure.Channels
{
    public class AzureServiceBusSubscriptionSubscriberChannel<T> : BaseAzureServiceBusChannel, ISubscriberChannel<T>
    {
        private SubscriptionClient subscriptionClient;

        public SubscriptionClient SubscriptionClient
        {
            get
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

        [Configurable]
        public bool AutoSetup { get; set; }

        [Configurable(IsRequired = true)]
        public override string ServiceBusConnectionString { get; set; }

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
    }
}