using System;
using Mantle.Azure;

namespace Mantle.Messaging.Azure
{
    public class AzureServiceBusQueueSubscriberEndpointManager : AzureServiceBusQueueEndpointManager,
                                                                 ISubscriberEndpointManager
    {
        public AzureServiceBusQueueSubscriberEndpointManager(AzureServiceBusQueueSubscriberEndpoint endpoint,
                                                             IAzureServiceBusConfiguration sbConfiguration)
            : base(endpoint, sbConfiguration)
        {
        }

        public void Create<T>()
        {
            throw new NotImplementedException();
        }
    }
}