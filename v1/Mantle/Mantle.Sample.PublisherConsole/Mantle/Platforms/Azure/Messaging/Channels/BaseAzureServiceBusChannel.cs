using Microsoft.ServiceBus;

namespace Mantle.Sample.PublisherConsole.Mantle.Platforms.Azure.Messaging.Channels
{
    public abstract class BaseAzureServiceBusChannel
    {
        private NamespaceManager namespaceManager;

        public abstract string ServiceBusConnectionString { get; set; }

        public NamespaceManager NamespaceManager
        {
            get { return GetNamespaceManager(); }
        }

        private NamespaceManager GetNamespaceManager()
        {
            return (namespaceManager = (namespaceManager ??
                                        NamespaceManager.CreateFromConnectionString(ServiceBusConnectionString)));
        }
    }
}