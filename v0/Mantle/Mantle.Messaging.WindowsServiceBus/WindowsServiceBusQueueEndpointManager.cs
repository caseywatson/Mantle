using System;
using Mantle.WindowsServiceBus;

namespace Mantle.Messaging.WindowsServiceBus
{
    public abstract class WindowsServiceBusQueueEndpointManager : WindowsServiceBusEndpointManager, IEndpointManager
    {
        private readonly WindowsServiceBusQueueEndpoint endpoint;

        protected WindowsServiceBusQueueEndpointManager(WindowsServiceBusQueueEndpoint endpoint,
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
            return NsManager.QueueExists(endpoint.QueueName);
        }

        public void Create()
        {
            NsManager.CreateQueue(endpoint.QueueName);
        }
    }
}