using System;
using Mantle.WindowsServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.WindowsServiceBus
{
    public class AzureServiceBusSubscriptionSubscriberEndpointManager : WindowsServiceBusEndpointManager,
                                                                        ISubscriberEndpointManager
    {
        private readonly WindowsServiceBusSubscriptionSubscriberEndpoint endpoint;

        public AzureServiceBusSubscriptionSubscriberEndpointManager(
            WindowsServiceBusSubscriptionSubscriberEndpoint endpoint, IWindowsServiceBusConfiguration sbConfiguration)
            : base(sbConfiguration)
        {
            if (endpoint == null)
                throw new ArgumentNullException("endpoint");

            endpoint.Validate();

            this.endpoint = endpoint;
        }

        public void Create<T>()
        {
            NsManager.CreateSubscription(endpoint.TopicName, endpoint.SubscriptionName,
                                         new SqlFilter(String.Format("MantleType = '{0}'",
                                                                     typeof (T).GetMessagingTypeString())));
        }

        public bool DoesExist()
        {
            return NsManager.SubscriptionExists(endpoint.TopicName, endpoint.SubscriptionName);
        }

        public void Create()
        {
            NsManager.CreateSubscription(endpoint.TopicName, endpoint.SubscriptionName);
        }
    }
}