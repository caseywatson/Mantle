using System;
using Mantle.WindowsServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.WindowsServiceBus
{
    public class WindowsServiceBusQueueSubscriberClient : WindowsServiceBusQueueClient, ISubscriberClient
    {
        public WindowsServiceBusQueueSubscriberClient(WindowsServiceBusQueueSubscriberEndpoint endpoint,
                                                    IWindowsServiceBusConfiguration sbConfiguration)
            : base(endpoint, sbConfiguration)
        {
        }

        public Message<T> Receive<T>()
        {
            return Receive<T>(TimeSpan.FromSeconds(30));
        }

        public Message<T> Receive<T>(TimeSpan timeout)
        {
            try
            {
                BrokeredMessage brokeredMessage = QueueClient.Receive(timeout);

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

                return new WindowsServiceBusMessage<T>(payload, brokeredMessage);
            }
            catch (Exception ex)
            {
                throw new MessagingException(
                    "An error occurred while attempting to read a message from the specified queue. See inner exception for more details.",
                    ex);
            }
        }
    }
}