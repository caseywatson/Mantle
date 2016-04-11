using Mantle.Extensions;
using Mantle.Messaging.Interfaces;
using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.Azure.Context
{
    public class AzureBrokeredMessageContext<T> : IMessageContext<T>
        where T : class
    {
        public AzureBrokeredMessageContext(BrokeredMessage brokeredMessage, T message)
        {
            brokeredMessage.Require(nameof(brokeredMessage));
            message.Require(nameof(message));

            BrokeredMessage = brokeredMessage;
            Message = message;
        }

        public BrokeredMessage BrokeredMessage { get; private set; }

        public int? DeliveryCount => BrokeredMessage.DeliveryCount;

        public bool IsAbandoned { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsDeadLettered { get; private set; }

        public T Message { get; private set; }

        public bool TryToAbandon()
        {
            try
            {
                BrokeredMessage.Abandon();
                return (IsAbandoned = true);
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
                return (IsCompleted = true);
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
                return (IsDeadLettered = true);
            }
            catch
            {
                return false;
            }
        }

        public bool TryToRenewLock()
        {
            try
            {
                BrokeredMessage.RenewLock();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}