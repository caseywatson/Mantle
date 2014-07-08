using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.Interfaces;
using Mantle.Messaging.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Mantle.Messaging.Azure.Channels
{
    public class AzureStorageBusQueuePublisherChannel<T> : IPublisherChannel<T>
    {
        private readonly ISerializer<T> serializer;

        private CloudQueue cloudQueue;
        private CloudQueueClient cloudQueueClient;
        private CloudStorageAccount cloudStorageAccount;

        public AzureStorageBusQueuePublisherChannel(ISerializer<T> serializer)
        {
            this.serializer = serializer;
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

        [Configurable]
        public bool AutoSetup { get; set; }

        [Configurable(IsRequired = true)]
        public string StorageConnectionString { get; set; }

        [Configurable(IsRequired = true)]
        public string QueueName { get; set; }

        public void Publish(T message)
        {
            message.Require("message");

            CloudQueue.AddMessage(new CloudQueueMessage(serializer.Serialize(message)));
        }
    }
}