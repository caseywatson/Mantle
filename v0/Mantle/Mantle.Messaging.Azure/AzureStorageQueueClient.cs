using System;
using Mantle.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Mantle.Messaging.Azure
{
    public abstract class AzureStorageQueueClient
    {
        protected readonly CloudQueue CloudQueue;
        protected readonly CloudQueueClient CloudQueueClient;
        protected readonly CloudStorageAccount CloudStorageAccount;

        protected AzureStorageQueueClient(AzureStorageQueueEndpoint endpoint,
            IAzureStorageConfiguration storageConfiguration)
        {
            if (storageConfiguration == null)
                throw new ArgumentNullException("storageConfiguration");

            endpoint.Validate();
            storageConfiguration.Validate();

            CloudStorageAccount = CloudStorageAccount.Parse(storageConfiguration.ConnectionString);
            CloudQueueClient = CloudStorageAccount.CreateCloudQueueClient();
            CloudQueue = CloudQueueClient.GetQueueReference(endpoint.QueueName);

            CloudQueue.CreateIfNotExists();
        }
    }
}