using Mantle.Messaging.Messages;

namespace Mantle.Messaging.Interfaces
{
    public interface ISubscription<T>
        where T : class
    {
        ISubscriptionConfiguration<T> Configuration { get; }

        bool HandleMessage(IMessageContext<MessageEnvelope> messageContext);
    }
}