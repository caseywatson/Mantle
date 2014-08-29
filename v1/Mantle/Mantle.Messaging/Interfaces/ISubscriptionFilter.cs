namespace Mantle.Messaging.Interfaces
{
    public interface ISubscriptionFilter<T>
        where T : class
    {
        bool OnHandledMessage(IMessageContext<T> messageContext);
        bool OnHandlingMessage(IMessageContext<T> messageContext);
    }
}