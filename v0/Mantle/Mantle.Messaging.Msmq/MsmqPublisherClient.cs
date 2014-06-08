using System;
using System.Messaging;

namespace Mantle.Messaging.Msmq
{
    public class MsmqPublisherClient : MsmqClient, IPublisherClient
    {
        public MsmqPublisherClient(MsmqPublisherEndpoint endpoint)
            : base(endpoint)
        {
        }

        public void Publish<T>(T message)
        {
            try
            {
                Queue.Send(new Message {BodyStream = message.Serialize()}, MessageQueueTransactionType.Single);
            }
            catch (Exception ex)
            {
                throw new MessagingException("Unable to send message. See inner exception for more details.", ex);
            }
        }
    }
}