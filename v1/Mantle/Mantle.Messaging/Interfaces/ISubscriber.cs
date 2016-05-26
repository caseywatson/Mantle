using System;

namespace Mantle.Messaging.Interfaces
{
    public interface ISubscriber<T>
        where T : class
    {
        event Action<string> ErrorOccurred;
        event Action<string> MessageOccurred;

        void HandleMessage(IMessageContext<T> messageContext);
    }
}