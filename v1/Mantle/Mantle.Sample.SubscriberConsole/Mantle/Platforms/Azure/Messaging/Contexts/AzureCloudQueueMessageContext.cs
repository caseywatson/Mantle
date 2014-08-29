using System;
using Mantle.Extensions;
using Mantle.Messaging.Interfaces;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Mantle.Sample.SubscriberConsole.Mantle.Platforms.Azure.Messaging.Contexts
{
    public class AzureCloudQueueMessageContext<T> : IMessageContext<T>
        where T : class
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

        public int? DeliveryCount
        {
            get { return null; }
        }

        public bool IsAbandoned { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsDeadLettered { get; private set; }

        public T Message { get; private set; }

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