using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.Messaging.Interfaces;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.Azure.Channels
{
    public class AzureServiceBusQueuePublisherChannel<T> : IPublisherChannel<T>
    {
        private NamespaceManager namespaceManager;
        private QueueClient queueClient;

        public NamespaceManager NamespaceManager
        {
            get
            {
                return (namespaceManager = (namespaceManager ??
                                            NamespaceManager.CreateFromConnectionString(ServiceBusConnectionString)));
            }
        }

        public QueueClient QueueClient
        {
            get
            {
                if (queueClient == null)
                {
                    if (AutoSetup)
                    {
                        if (namespaceManager.QueueExists(QueueName) == false)
                            namespaceManager.CreateQueue(QueueName);
                    }

                    queueClient = QueueClient.CreateFromConnectionString(ServiceBusConnectionString, QueueName);
                }

                return queueClient;
            }
        }

        [Configurable]
        public bool AutoSetup { get; set; }

        [Configurable(IsRequired = true)]
        public string ServiceBusConnectionString { get; set; }

        [Configurable(IsRequired = true)]
        public string QueueName { get; set; }

        public void Publish(T message)
        {
            message.Require("message");

            QueueClient.Send(new BrokeredMessage(message));
        }
    }
}