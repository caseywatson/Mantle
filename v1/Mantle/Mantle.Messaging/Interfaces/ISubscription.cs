using Mantle.Messaging.Messages;

namespace Mantle.Messaging.Interfaces
{
    public interface ISubscription<T>
    {
        bool HandleMessage(IMessageContext<MessageEnvelope> messageContext);
    }
}