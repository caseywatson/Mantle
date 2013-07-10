using System;
using System.Messaging;

namespace Mantle.Messaging.Msmq
{
    public class MsmqSubscriberClient : ISubscriberClient
    {
        private readonly MessageQueue queue;

        public MsmqSubscriberClient(MsmqSubscriberEndpoint endpoint)
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

        public Message<T> Receive<T>()
        {
            return Receive<T>(TimeSpan.FromMinutes(1));
        }

        public Message<T> Receive<T>(TimeSpan timeout)
        {
            var transaction = new MessageQueueTransaction();

            transaction.Begin();

            try
            {
                Message queueMessage = queue.Receive(timeout, transaction);

                if (queueMessage == null)
                    return null;

                queueMessage.BodyStream.Position = 0;

                T payload;

                try
                {
                    payload = queueMessage.BodyStream.Deserialize<T>();
                }
                catch
                {
                    throw new MessageDeserializationException<T>(
                        "Unable to deserialize the provided MSMQ message payload.",
                        new MsmqMessage<T>(default(T), transaction));
                }

                return new MsmqMessage<T>(payload, transaction);
            }
            catch (Exception ex)
            {
                transaction.Abort();
                throw new MessagingException(
                    "An error occurred while attempting to read a message from the specified queue. See inner exception for more details.",
                    ex);
            }
        }
    }
}