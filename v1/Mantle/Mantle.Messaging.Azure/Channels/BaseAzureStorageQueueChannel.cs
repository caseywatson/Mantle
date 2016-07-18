using Mantle.FaultTolerance.Interfaces;
using Mantle.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Mantle.Messaging.Azure.Channels
{
    public abstract class BaseAzureStorageQueueChannel<T>
    {
        protected readonly ISerializer<T> Serializer;

        private readonly ITransientFaultStrategy transientFaultStrategy;

        private CloudQueue cloudQueue;
        private CloudQueueClient cloudQueueClient;
        private CloudStorageAccount cloudStorageAccount;

        protected BaseAzureStorageQueueChannel(ISerializer<T> serializer,
                                               ITransientFaultStrategy transientFaultStrategy)
        {
            Serializer = serializer;

            this.transientFaultStrategy = transientFaultStrategy;
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
                cloudQueue = transientFaultStrategy.Try(() => CloudQueueClient.GetQueueReference(QueueName));

                if (AutoSetup)
                    transientFaultStrategy.Try(() => cloudQueue.CreateIfNotExists());
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