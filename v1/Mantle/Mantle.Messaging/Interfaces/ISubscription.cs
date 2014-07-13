using Mantle.Messaging.Messages;

namespace Mantle.Messaging.Interfaces
{
    public interface ISubscription<T>
        where T : class
    {
        void HandleMessage(IMessageContext<Message> messageContext);
    }
}