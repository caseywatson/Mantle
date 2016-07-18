using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.FaultTolerance.Interfaces;
using Mantle.Messaging.Interfaces;
using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.Azure.Channels
{
    public class AzureServiceBusQueuePublisherChannel<T> : BaseAzureServiceBusQueueChannel, IPublisherChannel<T>
        where T : class
    {
        private readonly ITransientFaultStrategy transientFaultStrategy;

        public AzureServiceBusQueuePublisherChannel(ITransientFaultStrategy transientFaultStrategy)
            : base(transientFaultStrategy)
        {
            this.transientFaultStrategy = transientFaultStrategy;
        }

        [Configurable]
        public override bool AutoSetup { get; set; }

        [Configurable(IsRequired = true)]
        public override string QueueName { get; set; }

        [Configurable(IsRequired = true)]
        public override string ServiceBusConnectionString { get; set; }

        public void Publish(T message)
        {
            message.Require(nameof(message));

            transientFaultStrategy.Try(() => QueueClient.Send(new BrokeredMessage(message)));
        }
    }
}