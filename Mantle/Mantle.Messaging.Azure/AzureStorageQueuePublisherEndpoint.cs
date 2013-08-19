using Mantle.Azure;

namespace Mantle.Messaging.Azure
{
    public class AzureStorageQueuePublisherEndpoint : AzureStorageQueueEndpoint, IPublisherEndpoint
    {
        public AzureStorageQueuePublisherEndpoint(IAzureStorageConfiguration storageConfiguration)
            : base(storageConfiguration)
        {
        }

        public IPublisherClient GetClient()
        {
            return new AzureStorageQueuePublisherClient(this, StorageConfiguration);
        }

        public IPublisherEndpointManager GetManager()
        {
            return new AzureStorageQueuePublisherEndpointManager(this, StorageConfiguration);
        }
    }
}