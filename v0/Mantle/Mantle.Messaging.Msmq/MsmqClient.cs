using System;
using System.Messaging;

namespace Mantle.Messaging.Msmq
{
    public abstract class MsmqClient
    {
        protected MsmqClient(MsmqEndpoint endpoint)
        {
            if (endpoint == null)
                throw new ArgumentNullException("endpoint");

            endpoint.Validate();

            try
            {
                if (MessageQueue.Exists(endpoint.QueuePath) == false)
                    MessageQueue.Create(endpoint.QueuePath);

                Queue = new MessageQueue(endpoint.QueuePath);
            }
            catch (Exception ex)
            {
                throw new MessagingException(
                    "An error occurred while attempting to access the specified queue. See inner exception for more details.",
                    ex);
            }
        }

        protected MessageQueue Queue { get; private set; }
    }
}