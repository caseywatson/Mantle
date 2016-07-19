namespace Mantle.Messaging.Interfaces
{
    public interface IMessageContext<T>
        where T : class
    {
        int? DeliveryCount { get; }
        string Id { get; }
        T Message { get; }

        bool IsAbandoned { get; }
        bool IsCompleted { get; }
        bool IsDeadLettered { get; }

        bool TryToAbandon();
        bool TryToComplete();
        bool TryToDeadLetter();
        bool TryToRenewLock();
    }
}