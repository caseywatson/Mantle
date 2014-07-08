using Mantle.Extensions;
using Mantle.Messaging.Interfaces;
using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.Azure.Context
{
    public class AzureBrokeredMessageContext<T> : IMessageContext<T>
    {
        public AzureBrokeredMessageContext(BrokeredMessage brokeredMessage, T message)
        {
            brokeredMessage.Require("brokeredMessage");
            message.Require("message");

            BrokeredMessage = brokeredMessage;
            Message = message;
        }

        public BrokeredMessage BrokeredMessage { get; private set; }
        public T Message { get; private set; }

        public int? DeliveryCount
        {
            get { return BrokeredMessage.DeliveryCount; }
        }

        public bool TryToAbandon()
        {
            try
            {
                BrokeredMessage.Abandon();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryToComplete()
        {
            try
            {
                BrokeredMessage.Complete();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryToDeadLetter()
        {
            try
            {
                BrokeredMessage.DeadLetter();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}