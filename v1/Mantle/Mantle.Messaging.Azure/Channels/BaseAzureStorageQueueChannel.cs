using Mantle.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Mantle.Messaging.Azure.Channels
{
    public abstract class BaseAzureStorageQueueChannel<T>
    {
        protected readonly ISerializer<T> Serializer;

        private CloudQueue cloudQueue;
        private CloudQueueClient cloudQueueClient;
        private CloudStorageAccount cloudStorageAccount;

        protected BaseAzureStorageQueueChannel(ISerializer<T> serializer)
        {
            Serializer = serializer;
        }

        public abstract bool AutoSetup { get; set; }

        public abstract string QueueName { get; set; }
        public abstract string StorageConnectionString { get; set; }

        public CloudQueue CloudQueue => GetCloudQueue();

        public CloudQueueClient CloudQueueClient => GetCloudQueueClient();

        public CloudStorageAccount CloudStorageAccount => GetCloudStorageAccount();

        private CloudQueue GetCloudQueue()
        {
            if (cloudQueue == null)
            {
                cloudQueue = CloudQueueClient.GetQueueReference(QueueName);

                if (AutoSetup)
                    cloudQueue.CreateIfNotExists();
            }

            return cloudQueue;
        }

        private CloudQueueClient GetCloudQueueClient()
        {
            return (cloudQueueClient = (cloudQueueClient ??
                                        CloudStorageAccount.CreateCloudQueueClient()));
        }

        private CloudStorageAccount GetCloudStorageAccount()
        {
            return (cloudStorageAccount = (cloudStorageAccount ??
                                           CloudStorageAccount.Parse(StorageConnectionString)));
        }
    }
}