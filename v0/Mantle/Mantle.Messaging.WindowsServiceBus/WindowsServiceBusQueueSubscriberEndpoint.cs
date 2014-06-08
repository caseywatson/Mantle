using System;
using Mantle.WindowsServiceBus;

namespace Mantle.Messaging.WindowsServiceBus
{
    public class WindowsServiceBusQueueSubscriberEndpoint : WindowsServiceBusQueueEndpoint, ISubscriberEndpoint
    {
        private readonly IWindowsServiceBusConfiguration sbConfiguration;

        public WindowsServiceBusQueueSubscriberEndpoint(IWindowsServiceBusConfiguration sbConfiguration)
        {
            if (sbConfiguration == null)
                throw new ArgumentNullException("sbConfiguration");

            sbConfiguration.Validate();

            this.sbConfiguration = sbConfiguration;
        }

        public ISubscriberClient GetClient()
        {
            return new WindowsServiceBusQueueSubscriberClient(this, sbConfiguration);
        }

        public ISubscriberEndpointManager GetManager()
        {
            return new WindowsServiceBusQueueSubscriberEndpointManager(this, sbConfiguration);
        }
    }
}