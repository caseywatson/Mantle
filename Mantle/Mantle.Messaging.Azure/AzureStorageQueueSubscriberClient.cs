using System;
using Mantle.Azure;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Mantle.Messaging.Azure
{
    public class AzureStorageQueueSubscriberClient : AzureStorageQueueClient, ISubscriberClient
    {
        public AzureStorageQueueSubscriberClient(AzureStorageQueueSubscriberEndpoint endpoint,
                                                 IAzureStorageConfiguration storageConfiguration)
            : base(endpoint, storageConfiguration)
        {
            if (CloudQueue.Exists() == false)
                throw new MessagingException(
                    String.Format("The specified Azure storage queue [{0}] does not exist. Unable to subscribe.",
                        endpoint.QueueName));
        }

        public Message<T> Receive<T>()
        {
            return Receive<T>(TimeSpan.Zero);
        }

        public Message<T> Receive<T>(TimeSpan timeout)
        {
            CloudQueueMessage cqMessage = CloudQueue.GetMessage();

            if (cqMessage == null)
                return null;

            return new AzureStorageQueueMessage<T>(cqMessage.AsBytes.DeserializeBytes<T>(), this, cqMessage);
        }

        public void Delete(CloudQueueMessage cqMessage)
        {
            if (cqMessage == null)
                throw new ArgumentNullException("cqMessage");

            CloudQueue.DeleteMessage(cqMessage);
        }
    }
}