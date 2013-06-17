using System;
using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.Azure
{
    public class AzureServiceBusMessage<T> : Message<T>
    {
        private readonly BrokeredMessage sbMessage;

        public AzureServiceBusMessage(T payload, BrokeredMessage sbMessage)
            : base(payload)
        {
            if (sbMessage == null)
                throw new ArgumentNullException("sbMessage");

            this.sbMessage = sbMessage;
        }

        public override void Abandon()
        {
            sbMessage.Abandon();
        }

        public override void Complete()
        {
            sbMessage.Complete();
        }
    }
}