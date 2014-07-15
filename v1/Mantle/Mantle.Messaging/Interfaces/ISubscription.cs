using Mantle.Messaging.Messages;

namespace Mantle.Messaging.Interfaces
{
    public interface ISubscription
    {
        bool HandleMessage(IMessageContext<MessageEnvelope> messageContext);
    }
}