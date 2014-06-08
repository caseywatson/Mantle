using System;
using Mantle.WindowsServiceBus;

namespace Mantle.Messaging.WindowsServiceBus
{
    public class WindowsServiceBusQueueSubscriberEndpointManager : WindowsServiceBusQueueEndpointManager,
                                                                   ISubscriberEndpointManager
    {
        public WindowsServiceBusQueueSubscriberEndpointManager(WindowsServiceBusQueueSubscriberEndpoint endpoint,
                                                               IWindowsServiceBusConfiguration sbConfiguration)
            : base(endpoint, sbConfiguration)
        {
        }

        public void Create<T>()
        {
            throw new NotImplementedException();
        }
    }
}