using System;
using Mantle.Azure;
using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.Azure
{
    public class AzureServiceBusSubscriptionSubscriberClient : AzureServiceBusClient, ISubscriberClient
    {
        private readonly SubscriptionClient subscriptionClient;

        public AzureServiceBusSubscriptionSubscriberClient(AzureServiceBusSubscriptionSubscriberEndpoint endpoint,
                                                           IAzureServiceBusConfiguration sbConfiguration)
            : base(sbConfiguration)
        {
            if (endpoint == null)
                throw new ArgumentNullException("endpoint");

            endpoint.Validate();

            try
            {
                if (NsManager.TopicExists(endpoint.TopicName) == false)
                    NsManager.CreateTopic(endpoint.TopicName);

                if (NsManager.SubscriptionExists(endpoint.TopicName, endpoint.SubscriptionName) == false)
                    NsManager.CreateSubscription(endpoint.TopicName, endpoint.SubscriptionName);

                subscriptionClient = SubscriptionClient.CreateFromConnectionString(sbConfiguration.ConnectionString,
                    endpoint.TopicName,
                    endpoint.SubscriptionName);
            }
            catch (Exception ex)
            {
                throw new MessagingException(
                    String.Format(
                        "An error occurred while attempting to access the specified Azure service bus subscription [{0}]. See inner exception for more details.",
                        endpoint.TopicName),
                    ex);
            }
        }

        public Message<T> Receive<T>()
        {
            return Receive<T>(TimeSpan.FromSeconds(30));
        }

        public Message<T> Receive<T>(TimeSpan timeout)
        {
            try
            {
                BrokeredMessage brokeredMessage = subscriptionClient.Receive(timeout);

                if (brokeredMessage == null)
                    return null;

                T payload;

                try
                {
                    payload = brokeredMessage.GetBody<T>();
                }
                catch
                {
                    payload = default(T);
                }

                return new AzureServiceBusMessage<T>(payload, brokeredMessage);
            }
            catch (Exception ex)
            {
                throw new MessagingException(
                    "An error occurred while attempting to read a message from the specified subscription. See inner exception for more details.",
                    ex);
            }
        }
    }
}