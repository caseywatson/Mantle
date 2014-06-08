using Mantle.Azure;

namespace Mantle.Messaging.Azure
{
    public class AzureStorageQueuePublisherEndpointManager : AzureStorageQueueEndpointManager
    {
        public AzureStorageQueuePublisherEndpointManager(AzureStorageQueuePublisherEndpoint endpoint,
                                                         IAzureStorageConfiguration storageConfiguration)
            : base(endpoint, storageConfiguration)
        {
        }
    }
}