using System;
using System.Messaging;

namespace Mantle.Messaging.Msmq
{
    public class MsmqPublisherClient : IPublisherClient
    {
        private readonly MessageQueue queue;

        public MsmqPublisherClient(MsmqPublisherEndpoint endpoint)
        {
            if (endpoint == null)
                throw new ArgumentNullException("endpoint");

            endpoint.Validate();

            try
            {
                queue = new MessageQueue(endpoint.QueuePath);
            }
            catch (Exception ex)
            {
                throw new MessagingException(
                    "An error occurred while attempting to access the specified queue. See inner exception for more details.",
                    ex);
            }
        }

        public void Publish<T>(T message)
        {
            try
            {
                queue.Send(new Message {BodyStream = message.Serialize()}, MessageQueueTransactionType.Single);
            }
            catch (Exception ex)
            {
                throw new MessagingException("Unable to send message. See inner exception for more details.", ex);
            }
        }
    }
}