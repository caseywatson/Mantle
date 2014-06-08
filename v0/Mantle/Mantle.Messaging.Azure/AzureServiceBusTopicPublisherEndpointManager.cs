using System;
using Mantle.Azure;

namespace Mantle.Messaging.Azure
{
    public class AzureServiceBusTopicPublisherEndpointManager : AzureServiceBusEndpointManager,
                                                                IPublisherEndpointManager
    {
        private readonly AzureServiceBusTopicPublisherEndpoint endpoint;

        public AzureServiceBusTopicPublisherEndpointManager(AzureServiceBusTopicPublisherEndpoint endpoint,
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
            return NsManager.TopicExists(endpoint.TopicName);
        }

        public void Create()
        {
            NsManager.CreateTopic(endpoint.TopicName);
        }
    }
}