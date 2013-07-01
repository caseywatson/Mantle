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
        }

        public override void Abandon()
        {
            // Do nothing... Azure will release the message automatically.
        }

        public override void Complete()
        {
            sqClient.Delete(cqMessage);
        }
    }
}