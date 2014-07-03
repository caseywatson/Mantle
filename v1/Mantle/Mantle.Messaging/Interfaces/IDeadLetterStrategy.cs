namespace Mantle.Messaging.Interfaces
{
    public interface IDeadLetterStrategy<T>
    {
        void HandleDeadLetterMessage(IMessageContext<T> messageContext);
    }
}