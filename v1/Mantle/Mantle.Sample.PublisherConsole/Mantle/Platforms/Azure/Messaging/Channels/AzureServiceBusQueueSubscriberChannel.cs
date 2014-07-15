using System;
using Mantle.Configuration.Attributes;
using Mantle.Messaging.Interfaces;
using Mantle.Sample.PublisherConsole.Mantle.Platforms.Azure.Messaging.Contexts;

namespace Mantle.Sample.PublisherConsole.Mantle.Platforms.Azure.Messaging.Channels
{
    public class AzureServiceBusQueueSubscriberChannel<T> : BaseAzureServiceBusQueueChannel, ISubscriberChannel<T>
        where T : class
    {
        [Configurable]
        public override bool AutoSetup { get; set; }

        [Configurable(IsRequired = true)]
        public override string QueueName { get; set; }

        [Configurable(IsRequired = true)]
        public override string ServiceBusConnectionString { get; set; }

        public IMessageContext<T> Receive(TimeSpan? timeout = null)
        {
            var message = ((timeout.HasValue)
                ? (QueueClient.Receive(timeout.Value))
                : (QueueClient.Receive()));

            if (message == null)
                return null;

            return new AzureBrokeredMessageContext<T>(message, message.GetBody<T>());
        }
    }
}