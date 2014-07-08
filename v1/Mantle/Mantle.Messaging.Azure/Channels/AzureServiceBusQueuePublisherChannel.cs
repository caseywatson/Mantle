using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.Messaging.Interfaces;
using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.Azure.Channels
{
    public class AzureServiceBusQueuePublisherChannel<T> : BaseAzureServiceBusQueueChannel, IPublisherChannel<T>
    {
        [Configurable]
        public override bool AutoSetup { get; set; }

        [Configurable(IsRequired = true)]
        public override string ServiceBusConnectionString { get; set; }

        [Configurable(IsRequired = true)]
        public override string QueueName { get; set; }

        public void Publish(T message)
        {
            message.Require("message");
            QueueClient.Send(new BrokeredMessage(message));
        }
    }
}