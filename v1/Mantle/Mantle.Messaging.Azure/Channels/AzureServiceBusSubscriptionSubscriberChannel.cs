using System;
using System.Threading.Tasks;
using Mantle.Configuration.Attributes;
using Mantle.FaultTolerance.Interfaces;
using Mantle.Messaging.Azure.Context;
using Mantle.Messaging.Interfaces;
using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.Azure.Channels
{
    public class AzureServiceBusSubscriptionSubscriberChannel<T> : BaseAzureServiceBusChannel, ISubscriberChannel<T>
        where T : class
    {
        private readonly ITransientFaultStrategy transientFaultStrategy;

        private SubscriptionClient subscriptionClient;

        public AzureServiceBusSubscriptionSubscriberChannel(ITransientFaultStrategy transientFaultStrategy)
            : base(transientFaultStrategy)
        {
            this.transientFaultStrategy = transientFaultStrategy;
        }

        [Configurable(IsRequired = true)]
        public override string ServiceBusConnectionString { get; set; }

        [Configurable]
        public bool AutoSetup { get; set; }

        public SubscriptionClient SubscriptionClient => GetSubscriptionClient();

        [Configurable(IsRequired = true)]
        public string SubscriptionName { get; set; }

        [Configurable(IsRequired = true)]
        public string TopicName { get; set; }

        public IMessageContext<T> Receive(TimeSpan? timeout = null)
        {
            var message = ((timeout.HasValue)
                ? (transientFaultStrategy.Try(() => SubscriptionClient.Receive(timeout.Value)))
                : (transientFaultStrategy.Try(() => SubscriptionClient.Receive())));

            if (message == null)
                return null;

            return new AzureBrokeredMessageContext<T>(message, message.GetBody<T>());
        }


        public async Task<IMessageContext<T>> ReceiveAsync()
        {
            var message = await transientFaultStrategy.Try(() => SubscriptionClient.ReceiveAsync());

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
                    if (transientFaultStrategy.Try(
                        () => NamespaceManager.SubscriptionExists(TopicName, SubscriptionName) == false))
                    {
                        transientFaultStrategy.Try(
                            () => NamespaceManager.CreateSubscription(TopicName, SubscriptionName));
                    }
                }

                subscriptionClient = SubscriptionClient.CreateFromConnectionString(ServiceBusConnectionString,
                                                                                   TopicName, SubscriptionName);
            }

            return subscriptionClient;
        }
    }
}