using System;
using Mantle.Azure;
using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.Azure
{
    public class AzureServiceBusSubscriptionSubscriberEndpointManager : AzureServiceBusEndpointManager,
                                                                        ISubscriberEndpointManager
    {
        private readonly AzureServiceBusSubscriptionSubscriberEndpoint endpoint;

        public AzureServiceBusSubscriptionSubscriberEndpointManager(
            AzureServiceBusSubscriptionSubscriberEndpoint endpoint, IAzureServiceBusConfiguration sbConfiguration)
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