using Mantle.Extensions;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Messages;

namespace Mantle.Messaging.Contexts
{
    public class SubscriptionMessageContext<T> : IMessageContext<T>
        where T : class
    {
        public SubscriptionMessageContext(IMessageContext<Message> originalMessageContext, T message)
        {
            originalMessageContext.Require("originalMessageContext");
            message.Require("message");

            OriginalMessageContext = originalMessageContext;
            Message = message;
        }

        public int? DeliveryCount
        {
            get { return OriginalMessageContext.DeliveryCount; }
        }

        public T Message { get; private set; }
        public IMessageContext<Message> OriginalMessageContext { get; private set; }

        public bool TryToAbandon()
        {
            return OriginalMessageContext.TryToAbandon();
        }

        public bool TryToComplete()
        {
            return OriginalMessageContext.TryToComplete();
        }

        public bool TryToDeadLetter()
        {
            return false;
        }
    }
}