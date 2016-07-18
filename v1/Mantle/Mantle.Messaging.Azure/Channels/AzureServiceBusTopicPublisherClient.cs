using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.FaultTolerance.Interfaces;
using Mantle.Messaging.Interfaces;
using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.Azure.Channels
{
    public class AzureServiceBusTopicPublisherChannel<T> : BaseAzureServiceBusChannel, IPublisherChannel<T>
        where T : class
    {
        private readonly ITransientFaultStrategy transientFaultStrategy;

        private TopicClient topicClient;

        public AzureServiceBusTopicPublisherChannel(ITransientFaultStrategy transientFaultStrategy)
            : base(transientFaultStrategy)
        {
            this.transientFaultStrategy = transientFaultStrategy;
        }

        [Configurable(IsRequired = true)]
        public override string ServiceBusConnectionString { get; set; }

        [Configurable]
        public bool AutoSetup { get; set; }

        public TopicClient TopicClient => GetTopicClient();

        [Configurable(IsRequired = true)]
        public string TopicName { get; set; }

        public void Publish(T message)
        {
            message.Require(nameof(message));

            transientFaultStrategy.Try(() => TopicClient.Send(new BrokeredMessage(message)));
        }

        private TopicClient GetTopicClient()
        {
            if (topicClient == null)
            {
                if (AutoSetup)
                {
                    if (transientFaultStrategy.Try(() => NamespaceManager.TopicExists(TopicName) == false))
                        transientFaultStrategy.Try(() => NamespaceManager.CreateTopic(TopicName));
                }

                topicClient = TopicClient.CreateFromConnectionString(ServiceBusConnectionString, TopicName);
            }

            return topicClient;
        }
    }
}