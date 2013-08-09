using System;

using Mantle.WindowsServiceBus;

using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.WindowsServiceBus
{
    public class WindowsServiceBusSubscriptionSubscriberClient : WindowsServiceBusClient, ISubscriberClient
    {
        private readonly SubscriptionClient subscriptionClient;

        public WindowsServiceBusSubscriptionSubscriberClient(WindowsServiceBusSubscriptionSubscriberEndpoint endpoint,
                                                           IWindowsServiceBusConfiguration sbConfiguration)
            : base(sbConfiguration)
        {
            if (endpoint == null)
                throw new ArgumentNullException("endpoint");

            endpoint.Validate();

            try
            {
                if (NsManager.TopicExists(endpoint.TopicName) == false)
                    throw new MessagingException(
                        String.Format(
                            "The Windows service bus topic [{0}] to which the specified subscription [{1}] subscribes to does not exist.",
                            endpoint.TopicName, endpoint.SubscriptionName));

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
                        "An error occurred while attempting to access the specified Windows service bus subscription [{0}]. See inner exception for more details.",
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
                    throw new MessageDeserializationException<T>(
                        "Unable to deserialize the provided Windows service bus brokered message payload.",
                        new WindowsServiceBusMessage<T>(default(T), brokeredMessage));
                }

                return new WindowsServiceBusMessage<T>(payload, brokeredMessage);
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