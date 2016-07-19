using System;
using Mantle.Extensions;
using Mantle.Messaging.Interfaces;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Mantle.Messaging.Azure.Context
{
    public class AzureCloudQueueMessageContext<T> : IMessageContext<T>
        where T : class
    {
        public AzureCloudQueueMessageContext(CloudQueue cloudQueue, CloudQueueMessage cloudQueueMessage, T message)
        {
            cloudQueue.Require(nameof(cloudQueue));
            cloudQueueMessage.Require(nameof(cloudQueueMessage));
            message.Require(nameof(message));

            CloudQueue = cloudQueue;
            CloudQueueMessage = cloudQueueMessage;
            Id = cloudQueueMessage.Id;
            Message = message;
        }

        public CloudQueue CloudQueue { get; }
        public CloudQueueMessage CloudQueueMessage { get; }

        public int? DeliveryCount => null;

        public string Id { get; set; }

        public bool IsAbandoned { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsDeadLettered { get; private set; }

        public T Message { get; }

        public bool TryToAbandon()
        {
            return false;
        }

        public bool TryToComplete()
        {
            CloudQueue.DeleteMessage(CloudQueueMessage);
            return (IsCompleted = true);
        }

        public bool TryToDeadLetter()
        {
            return false;
        }

        public bool TryToRenewLock()
        {
            try
            {
                CloudQueue.UpdateMessage(CloudQueueMessage, TimeSpan.FromMinutes(1), MessageUpdateFields.Visibility);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}