using System;
using Mantle.WindowsServiceBus;

namespace Mantle.Messaging.WindowsServiceBus
{
    public class WinodwsServiceBusTopicPublisherEndpointManager : WindowsServiceBusEndpointManager,
                                                                  IPublisherEndpointManager
    {
        private readonly WindowsServiceBusTopicPublisherEndpoint endpoint;

        public WinodwsServiceBusTopicPublisherEndpointManager(WindowsServiceBusTopicPublisherEndpoint endpoint,
                                                              IWindowsServiceBusConfiguration sbConfiguration)
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