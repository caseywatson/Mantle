using System;

using Mantle.WindowsServiceBus;

using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.WindowsServiceBus
{
    public class WindowsServiceBusQueuePublisherClient : WindowsServiceBusQueueClient, IPublisherClient
    {
        public WindowsServiceBusQueuePublisherClient(WindowsServiceBusQueuePublisherEndpoint endpoint,
                                                   IWindowsServiceBusConfiguration sbConfiguration)
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