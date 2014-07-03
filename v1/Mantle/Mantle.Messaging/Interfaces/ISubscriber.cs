namespace Mantle.Messaging.Interfaces
{
    public interface ISubscriber<T>
    {
        void HandleMessage(IMessageContext<T> messageContext);
    }
}