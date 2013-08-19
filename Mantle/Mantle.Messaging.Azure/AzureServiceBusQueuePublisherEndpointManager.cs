using Mantle.Azure;

namespace Mantle.Messaging.Azure
{
    public class AzureServiceBusQueuePublisherEndpointManager : AzureServiceBusQueueEndpointManager,
                                                                IPublisherEndpointManager
    {
        public AzureServiceBusQueuePublisherEndpointManager(AzureServiceBusQueuePublisherEndpoint endpoint,
                                                            IAzureServiceBusConfiguration sbConfiguration)
            : base(endpoint, sbConfiguration)
        {
        }
    }
}