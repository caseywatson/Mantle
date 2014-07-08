namespace Mantle.Messaging.Interfaces
{
    public interface IMessageContext<T>
    {
        T Message { get; }

        int? DeliveryCount { get; }

        bool TryToAbandon();
        bool TryToComplete();
        bool TryToDeadLetter();
    }
}