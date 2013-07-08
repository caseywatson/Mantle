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

            CanBeAbandoned = true;
            CanBeCompleted = true;
            CanBeKilled = true;
            CanGetDeliveryCount = true;
        }

        public override void Abandon()
        {
            if (sbMessage != null)
            {
                try
                {
                    sbMessage.Abandon();
                }
                finally
                {
                    sbMessage.Dispose();
                }
            }
        }

        public override void Complete()
        {
            if (sbMessage != null)
            {
                try
                {
                    sbMessage.Complete();
                }
                finally
                {
                    sbMessage.Dispose();
                }
            }
        }

        public override void Kill()
        {
            if (sbMessage != null)
            {
                try
                {
                    sbMessage.DeadLetter();
                }
                finally
                {
                    sbMessage.Dispose();
                }
            }
        }

        public override int GetDeliveryCount()
        {
            if (sbMessage != null)
                return sbMessage.DeliveryCount;

            return 0;
        }
    }
}