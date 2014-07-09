namespace Mantle.Messaging.Interfaces
{
    public interface IMessageContext<T>
        where T : class
    {
        int? DeliveryCount { get; }
        T Message { get; }

        bool TryToAbandon();
        bool TryToComplete();
        bool TryToDeadLetter();
    }
}