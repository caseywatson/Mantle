using System;
using Mantle.Azure;

namespace Mantle.Messaging.Azure
{
    public class AzureStorageQueueSubscriberEndpointManager : AzureStorageQueueEndpointManager,
                                                              ISubscriberEndpointManager
    {
        public AzureStorageQueueSubscriberEndpointManager(AzureStorageQueueSubscriberEndpoint endpoint,
                                                          IAzureStorageConfiguration storageConfiguration)
            : base(endpoint, storageConfiguration)
        {
        }

        public void Create<T>()
        {
            throw new NotImplementedException();
        }
    }
}