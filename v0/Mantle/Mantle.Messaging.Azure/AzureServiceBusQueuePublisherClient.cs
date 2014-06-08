using System;
using Mantle.Azure;
using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.Azure
{
    public class AzureServiceBusQueuePublisherClient : AzureServiceBusQueueClient, IPublisherClient
    {
        public AzureServiceBusQueuePublisherClient(AzureServiceBusQueuePublisherEndpoint endpoint,
                                                   IAzureServiceBusConfiguration sbConfiguration)
            : base(endpoint, sbConfiguration)
        {
        }

        public void Publish<T>(T message)
        {
            try
            {
                QueueClient.Send(new BrokeredMessage(message));
            }
            catch (Exception ex)
            {
                throw new MessagingException("Unable to send message. See inner exception for more details.", ex);
            }
        }
    }
}