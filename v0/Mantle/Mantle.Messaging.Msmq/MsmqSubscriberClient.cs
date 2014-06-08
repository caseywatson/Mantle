using System;
using System.Messaging;

namespace Mantle.Messaging.Msmq
{
    public class MsmqSubscriberClient : MsmqClient, ISubscriberClient
    {
        public MsmqSubscriberClient(MsmqSubscriberEndpoint endpoint)
            : base(endpoint)
        {
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
                Message queueMessage = Queue.Receive(timeout, transaction);

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
                    payload = default(T);
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