using System;

namespace Mantle.Messaging.Msmq
{
    public abstract class MsmqEndpoint : Endpoint
    {
        public string QueuePath { get; set; }

        public virtual void Validate()
        {
            if (String.IsNullOrEmpty(QueuePath))
                throw new MessagingException("MSMQ queue path is required.");
        }
    }
}