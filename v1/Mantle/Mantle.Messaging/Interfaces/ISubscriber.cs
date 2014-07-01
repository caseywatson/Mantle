namespace Mantle.Messaging.Interfaces
{
    public interface ISubscriber
    {
    }

    public interface ISubscriber<T> : ISubscriber
    {
        void HandleMessage(IMessageContext<T> messageContext);
    }
}