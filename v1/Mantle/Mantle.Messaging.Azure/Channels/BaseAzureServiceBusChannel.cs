using Mantle.FaultTolerance.Interfaces;
using Microsoft.ServiceBus;

namespace Mantle.Messaging.Azure.Channels
{
    public abstract class BaseAzureServiceBusChannel
    {
        private readonly ITransientFaultStrategy transientFaultStrategy;

        private NamespaceManager namespaceManager;

        protected BaseAzureServiceBusChannel(ITransientFaultStrategy transientFaultStrategy)
        {
            this.transientFaultStrategy = transientFaultStrategy;
        }

        public abstract string ServiceBusConnectionString { get; set; }

        public NamespaceManager NamespaceManager => GetNamespaceManager();

        private NamespaceManager GetNamespaceManager()
        {
            return (namespaceManager = (namespaceManager ??
                                        transientFaultStrategy.Try(
                                            () =>
                                                NamespaceManager.CreateFromConnectionString(ServiceBusConnectionString))));
        }
    }
}