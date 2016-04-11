using System;

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
        bool TryToRenewLock();

        bool IsAbandoned { get; }
        bool IsCompleted { get; }
        bool IsDeadLettered { get; }
    }
}