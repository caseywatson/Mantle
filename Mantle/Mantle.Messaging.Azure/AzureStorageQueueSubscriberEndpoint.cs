using Mantle.Azure;

namespace Mantle.Messaging.Azure
{
    public class AzureStorageQueueSubscriberEndpoint : AzureStorageQueueEndpoint, ISubscriberEndpoint
    {
        public AzureStorageQueueSubscriberEndpoint(IAzureStorageConfiguration storageConfiguration)
            : base(storageConfiguration)
        {
        }

        public ISubscriberClient GetClient()
        {
            return new AzureStorageQueueSubscriberClient(this, StorageConfiguration);
        }
    }
}