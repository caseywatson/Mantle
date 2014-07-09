namespace Mantle.Messaging.Interfaces
{
    public interface IDeadLetterStrategy<T>
        where T : class
    {
        void HandleDeadLetterMessage(IMessageContext<T> messageContext);
    }
}