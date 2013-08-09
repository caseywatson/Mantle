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
        }

        public Message<T> Receive<T>()
        {
            return Receive<T>(TimeSpan.FromSeconds(30));
        }

        public Message<T> Receive<T>(TimeSpan timeout)
        {
            try
            {
                CloudQueueMessage cqMessage = CloudQueue.GetMessage();

                if (cqMessage == null)
                    return null;

                T payload;

                try
                {
                    payload = cqMessage.AsBytes.DeserializeBytes<T>();
                }
                catch
                {
                    payload = default(T);
                }

                return new AzureStorageQueueMessage<T>(payload, this, cqMessage);
            }
            catch (Exception ex)
            {
                throw new MessagingException(
                    "An error occurred while attempting to read a message from the specified queue. See inner exception for more details.",
                    ex);
            }
        }

        public void Delete(CloudQueueMessage cqMessage)
        {
            if (cqMessage == null)
                throw new ArgumentNullException("cqMessage");

            try
            {
                CloudQueue.DeleteMessage(cqMessage);
            }
            catch (Exception ex)
            {
                throw new MessagingException(
                    "An error occurred while attempting to remove a message from the specified queue. See inner exception for more details.",
                    ex);
            }
        }
    }
}