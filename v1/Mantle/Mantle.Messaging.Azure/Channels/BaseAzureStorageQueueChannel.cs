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

        public CloudStorageAccount CloudStorageAccount
        {
            get
            {
                return (cloudStorageAccount = (cloudStorageAccount ??
                                               CloudStorageAccount.Parse(StorageConnectionString)));
            }
        }

        public CloudQueueClient CloudQueueClient
        {
            get
            {
                return (cloudQueueClient = (cloudQueueClient ??
                                            CloudStorageAccount.CreateCloudQueueClient()));
            }
        }

        public CloudQueue CloudQueue
        {
            get
            {
                if (cloudQueue != null)
                {
                    cloudQueue = CloudQueueClient.GetQueueReference(QueueName);

                    if (AutoSetup)
                        cloudQueue.CreateIfNotExists();
                }

                return cloudQueue;
            }
        }

        public abstract bool AutoSetup { get; set; }

        public abstract string StorageConnectionString { get; set; }
        public abstract string QueueName { get; set; }
    }
}