using System;

namespace Mantle.Messaging.Msmq
{
    public abstract class MsmqEndpoint : Endpoint
    {
        public string QueuePath { get; set; }

        public void Configure(string name, string queuePath)
        {
            Name = name;
            QueuePath = queuePath;

            Validate();
        }

        public override void Validate()
        {
            base.Validate();

            if (String.IsNullOrEmpty(QueuePath))
                throw new MessagingException("MSMQ queue path is required.");
        }
    }
}