using System;
using Mantle.Azure;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Mantle.Messaging.Azure
{
    public class AzureStorageQueuePublisherClient : AzureStorageQueueClient, IPublisherClient
    {
        public AzureStorageQueuePublisherClient(AzureStorageQueuePublisherEndpoint endpoint,
                                                IAzureStorageConfiguration storageConfiguration)
            : base(endpoint, storageConfiguration)
        {
        }

        public void Publish<T>(T message)
        {
            try
            {
                CloudQueue.AddMessage(new CloudQueueMessage(message.SerializeToBytes()));
            }
            catch (Exception ex)
            {
                throw new MessagingException("Unable to send message. See inner exception for more details.", ex);
            }
        }
    }
}