using Mantle.Extensions;
using Mantle.Messaging.Interfaces;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Mantle.Messaging.Azure.Context
{
    public class AzureCloudQueueMessageContext<T> : IMessageContext<T>
    {
        public AzureCloudQueueMessageContext(CloudQueue cloudQueue, CloudQueueMessage cloudQueueMessage, T message)
        {
            cloudQueue.Require("cloudQueue");
            cloudQueueMessage.Require("cloudQueueMessage");
            message.Require("message");

            CloudQueue = cloudQueue;
            CloudQueueMessage = cloudQueueMessage;
            Message = message;
        }

        public CloudQueue CloudQueue { get; private set; }
        public CloudQueueMessage CloudQueueMessage { get; private set; }

        public T Message { get; private set; }

        public bool TryToAbandon()
        {
            return false;
        }

        public bool TryToComplete()
        {
            CloudQueue.DeleteMessage(CloudQueueMessage);
            return true;
        }

        public bool TryToDeadLetter()
        {
            return false;
        }


        public int? DeliveryCount
        {
            get { return null; }
        }
    }
}