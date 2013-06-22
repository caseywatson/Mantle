using System;

namespace Mantle.Messaging.Azure
{
    public abstract class AzureServiceBusQueueEndpoint : Endpoint
    {
        public string QueueName { get; set; }

        public void Setup(string name, string queueName)
        {
            Name = name;
            QueueName = queueName;

            Validate();
        }

        public override void Validate()
        {
            base.Validate();

            if (String.IsNullOrEmpty(QueueName))
                throw new MessagingException("Azure service bus queue name is required.");
        }
    }
}