using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.Messaging.Interfaces;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.Azure.Channels
{
    public class AzureServiceBusTopicPublisherChannel<T> : IPublisherChannel<T>
    {
        private NamespaceManager namespaceManager;
        private TopicClient topicClient;

        public NamespaceManager NamespaceManager
        {
            get
            {
                return (namespaceManager = (namespaceManager ??
                                            NamespaceManager.CreateFromConnectionString(ServiceBusConnectionString)));
            }
        }

        public TopicClient TopicClient
        {
            get
            {
                if (topicClient == null)
                {
                    if (AutoSetup)
                    {
                        if (namespaceManager.TopicExists(TopicName) == false)
                            namespaceManager.CreateTopic(TopicName);
                    }

                    topicClient = TopicClient.CreateFromConnectionString(ServiceBusConnectionString, TopicName);
                }

                return topicClient;
            }
        }

        [Configurable]
        public bool AutoSetup { get; set; }

        [Configurable(IsRequired = true)]
        public string ServiceBusConnectionString { get; set; }

        [Configurable(IsRequired = true)]
        public string TopicName { get; set; }

        public void Publish(T message)
        {
            message.Require("message");
            TopicClient.Send(new BrokeredMessage(message));
        }
    }
}