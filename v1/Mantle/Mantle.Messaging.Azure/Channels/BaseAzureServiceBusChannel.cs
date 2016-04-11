using Microsoft.ServiceBus;

namespace Mantle.Messaging.Azure.Channels
{
    public abstract class BaseAzureServiceBusChannel
    {
        private NamespaceManager namespaceManager;

        public abstract string ServiceBusConnectionString { get; set; }

        public NamespaceManager NamespaceManager => GetNamespaceManager();

        private NamespaceManager GetNamespaceManager()
        {
            return (namespaceManager = (namespaceManager ??
                                        NamespaceManager.CreateFromConnectionString(ServiceBusConnectionString)));
        }
    }
}