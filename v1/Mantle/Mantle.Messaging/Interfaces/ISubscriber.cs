namespace Mantle.Messaging.Interfaces
{
    public interface ISubscriber<T>
        where T : class
    {
        void HandleMessage(IMessageContext<T> messageContext);
    }
}