using Mantle.WindowsServiceBus;

namespace Mantle.Messaging.WindowsServiceBus
{
    public class WindowsServiceBusQueuePublisherEndpointManager : WindowsServiceBusQueueEndpointManager,
                                                                  IPublisherEndpointManager
    {
        public WindowsServiceBusQueuePublisherEndpointManager(WindowsServiceBusQueuePublisherEndpoint endpoint,
                                                              IWindowsServiceBusConfiguration sbConfiguration)
            : base(endpoint, sbConfiguration)
        {
        }
    }
}