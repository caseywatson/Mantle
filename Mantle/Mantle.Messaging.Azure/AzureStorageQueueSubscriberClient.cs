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
                    throw new MessageDeserializationException<T>(
                        "Unable to deserialize the provided Azure storage queue message payload.",
                        new AzureStorageQueueMessage<T>(default(T), this, cqMessage));
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