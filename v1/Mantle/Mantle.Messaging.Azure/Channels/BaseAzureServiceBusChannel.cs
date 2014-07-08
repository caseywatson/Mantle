using Microsoft.ServiceBus;

namespace Mantle.Messaging.Azure.Channels
{
    public abstract class BaseAzureServiceBusChannel
    {
        private NamespaceManager namespaceManager;

        public NamespaceManager NamespaceManager
        {
            get
            {
                return (namespaceManager = (namespaceManager ??
                                            NamespaceManager.CreateFromConnectionString(ServiceBusConnectionString)));
            }
        }

        public abstract string ServiceBusConnectionString { get; set; }
    }
}