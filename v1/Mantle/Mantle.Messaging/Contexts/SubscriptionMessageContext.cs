using Mantle.Extensions;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Messages;

namespace Mantle.Messaging.Contexts
{
    public class SubscriptionMessageContext<T> : IMessageContext<T>
        where T : class
    {
        public SubscriptionMessageContext(IMessageContext<MessageEnvelope> originalMessageContext, T message,
                                          ISubscription<T> subscription)
        {
            originalMessageContext.Require(nameof(originalMessageContext));
            message.Require(nameof(message));
            subscription.Require(nameof(subscription));

            OriginalMessageContext = originalMessageContext;
            Message = message;
            Subscription = subscription;
        }

        public IMessageContext<MessageEnvelope> OriginalMessageContext { get; }
        public ISubscription<T> Subscription { get; private set; }

        public int? DeliveryCount => OriginalMessageContext.DeliveryCount;

        public string Id => OriginalMessageContext.Id;

        public T Message { get; }

        public bool TryToAbandon() => (IsAbandoned = OriginalMessageContext.TryToAbandon());
        public bool TryToComplete() => (IsCompleted = OriginalMessageContext.TryToComplete());
        public bool TryToDeadLetter() => (IsDeadLettered = OriginalMessageContext.TryToDeadLetter());
        public bool TryToRenewLock() => OriginalMessageContext.TryToRenewLock();

        public bool IsAbandoned { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsDeadLettered { get; private set; }
    }
}