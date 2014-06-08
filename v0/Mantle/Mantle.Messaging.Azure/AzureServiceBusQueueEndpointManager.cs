using System;
using Mantle.Azure;
using Microsoft.ServiceBus;

namespace Mantle.Messaging.Azure
{
    public abstract class AzureServiceBusQueueEndpointManager : AzureServiceBusEndpointManager, IEndpointManager
    {
        private readonly AzureServiceBusQueueEndpoint endpoint;

        protected AzureServiceBusQueueEndpointManager(AzureServiceBusQueueEndpoint endpoint,
                                                      IAzureServiceBusConfiguration sbConfiguration)
            : base(sbConfiguration)
        {
            if (endpoint == null)
                throw new ArgumentNullException("endpoint");

            endpoint.Validate();

            this.endpoint = endpoint;
        }

        public bool DoesExist()
        {
            return NsManager.QueueExists(endpoint.QueueName);
        }

        public void Create()
        {
            NsManager.CreateQueue(endpoint.QueueName);
        }
    }
}