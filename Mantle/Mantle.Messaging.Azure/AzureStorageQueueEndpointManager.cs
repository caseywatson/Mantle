using System;
using Mantle.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Mantle.Messaging.Azure
{
    public abstract class AzureStorageQueueEndpointManager : IPublisherEndpointManager
    {
        protected readonly CloudQueue CloudQueue;
        protected readonly CloudQueueClient CloudQueueClient;
        protected readonly CloudStorageAccount CloudStorageAccount;

        protected AzureStorageQueueEndpointManager(AzureStorageQueueEndpoint endpoint,
                                                   IAzureStorageConfiguration storageConfiguration)
        {
            if (storageConfiguration == null)
                throw new ArgumentNullException("storageConfiguration");

            endpoint.Validate();
            storageConfiguration.Validate();

            CloudStorageAccount = CloudStorageAccount.Parse(storageConfiguration.ConnectionString);
            CloudQueueClient = CloudStorageAccount.CreateCloudQueueClient();
            CloudQueue = CloudQueueClient.GetQueueReference(endpoint.QueueName);
        }

        public bool DoesExist()
        {
            return CloudQueue.Exists();
        }

        public void Create()
        {
            CloudQueue.Create();
        }
    }
}