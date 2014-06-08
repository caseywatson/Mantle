using System;
using Mantle.WindowsServiceBus;

namespace Mantle.Messaging.WindowsServiceBus
{
    public class WindowsServiceBusQueuePublisherEndpoint : WindowsServiceBusQueueEndpoint, IPublisherEndpoint
    {
        private readonly IWindowsServiceBusConfiguration sbConfiguration;

        public WindowsServiceBusQueuePublisherEndpoint(IWindowsServiceBusConfiguration sbConfiguration)
        {
            if (sbConfiguration == null)
                throw new ArgumentNullException("sbConfiguration");

            sbConfiguration.Validate();

            this.sbConfiguration = sbConfiguration;
        }

        public IPublisherClient GetClient()
        {
            return new WindowsServiceBusQueuePublisherClient(this, sbConfiguration);
        }

        public IPublisherEndpointManager GetManager()
        {
            return new WindowsServiceBusQueuePublisherEndpointManager(this, sbConfiguration);
        }
    }
}