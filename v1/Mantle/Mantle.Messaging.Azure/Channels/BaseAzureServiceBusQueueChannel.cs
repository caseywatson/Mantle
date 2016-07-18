using Mantle.FaultTolerance.Interfaces;
using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.Azure.Channels
{
    public abstract class BaseAzureServiceBusQueueChannel : BaseAzureServiceBusChannel
    {
        private readonly ITransientFaultStrategy transientFaultStrategy;

        private QueueClient queueClient;

        protected BaseAzureServiceBusQueueChannel(ITransientFaultStrategy transientFaultStrategy)
            : base(transientFaultStrategy)
        {
            this.transientFaultStrategy = transientFaultStrategy;
        }

        public abstract bool AutoSetup { get; set; }
        public abstract string QueueName { get; set; }

        public QueueClient QueueClient => GetQueueClient();

        private QueueClient GetQueueClient()
        {
            if (queueClient == null)
            {
                if (AutoSetup)
                {
                    if (transientFaultStrategy.Try(() => NamespaceManager.QueueExists(QueueName)) == false)
                        transientFaultStrategy.Try(() => NamespaceManager.CreateQueue(QueueName));
                }

                queueClient = transientFaultStrategy.Try(
                    () => QueueClient.CreateFromConnectionString(ServiceBusConnectionString, QueueName));
            }

            return queueClient;
        }
    }
}