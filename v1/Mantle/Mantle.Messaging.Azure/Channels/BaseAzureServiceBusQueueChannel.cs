using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.Azure.Channels
{
    public abstract class BaseAzureServiceBusQueueChannel : BaseAzureServiceBusChannel
    {
        private QueueClient queueClient;

        public abstract bool AutoSetup { get; set; }
        public abstract string QueueName { get; set; }

        public QueueClient QueueClient => GetQueueClient();

        private QueueClient GetQueueClient()
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
}