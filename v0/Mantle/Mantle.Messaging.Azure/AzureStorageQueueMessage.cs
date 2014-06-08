using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Mantle.Messaging.Azure
{
    public class AzureStorageQueueMessage<T> : Message<T>, ICanBeCompleted, IHaveADeliveryCount
    {
        private readonly CloudQueueMessage cqMessage;
        private readonly AzureStorageQueueSubscriberClient sqClient;

        public AzureStorageQueueMessage(T payload, AzureStorageQueueSubscriberClient sqClient,
                                        CloudQueueMessage cqMessage)
            : base(payload)
        {
            if (sqClient == null)
                throw new ArgumentNullException("sqClient");

            if (cqMessage == null)
                throw new ArgumentNullException("cqMessage");

            this.sqClient = sqClient;
            this.cqMessage = cqMessage;
        }

        public void Complete()
        {
            sqClient.Delete(cqMessage);
        }

        public int GetDeliveryCount()
        {
            return cqMessage.DequeueCount;
        }
    }
}