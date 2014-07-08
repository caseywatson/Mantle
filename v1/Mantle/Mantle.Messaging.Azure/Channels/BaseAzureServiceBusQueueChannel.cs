using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.Azure.Channels
{
    public abstract class BaseAzureServiceBusQueueChannel : BaseAzureServiceBusChannel
    {
        private QueueClient queueClient;

        public QueueClient QueueClient
        {
            get
            {
                if (queueClient == null)
                {
                    if (AutoSetup)
                    {
                        if (NamespaceManager.QueueExists(QueueName) == false)
                            NamespaceManager.CreateQueue(QueueName);
                    }

                    queueClient = QueueClient.CreateFromConnectionString(ServiceBusConnectionString, QueueName);
                }

                return queueClient;
            }
        }

        public abstract bool AutoSetup { get; set; }
        public abstract string QueueName { get; set; }
    }
}