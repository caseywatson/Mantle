using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Mantle.Messaging.Azure
{
    public class AzureStorageQueueMessage<T> : Message<T>
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

            CanBeAbandoned = false;
            CanBeCompleted = true;
            CanBeKilled = false;
            CanGetDeliveryCount = false;
        }

        public override void Abandon()
        {
            throw new NotSupportedException(
                "Unable to abandon Azure storage queue message. Azure storage queue messages are automatically abandoned if not completed.");
        }

        public override void Complete()
        {
            sqClient.Delete(cqMessage);
        }

        public override int GetDeliveryCount()
        {
            return cqMessage.DequeueCount;
        }

        public override void Kill()
        {
            throw new NotSupportedException("Unable to kill Azure storage queue message.");
        }
    }
}